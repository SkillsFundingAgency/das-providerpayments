using System.Collections.Generic;
using System.Linq;
using FastMember;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Extensions;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services
{
    public class EarningValidationService 
    {
        private static readonly List<int> OnProgTransactionTypes = new List<int> { 1, 2, 3 };
        private static readonly TypeAccessor FundingDueAccessor = TypeAccessor.Create(typeof(RawEarning));

        public EarningValidationResult CreatePayableEarningsButHoldBackCompletionPaymentIfNecessary(
            IEnumerable<RawEarning> earnings,
            IHoldCommitmentInformation commitment = null,
            CompletionPaymentEvidence completionPaymentEvidence = null,
            int cenususType = -1)
        {
            var earningValidationResult = new EarningValidationResult();
            var reasonToHoldBack = "";
            var holdBack = false;
            if (completionPaymentEvidence != null)
            {
                holdBack = HoldBackCompletionPayment(completionPaymentEvidence, out reasonToHoldBack);
            }

            var payables = GetFundingDueForNonZeroTransactionTypes(earnings, commitment, cenususType);

            if (holdBack)
            {
                var nonPaybles = payables.Where(x => x.TransactionType == (int) TransactionType.Completion).Select(x=>
                {
                    var nonPay = new NonPayableEarning(x);
                    nonPay.PaymentFailureMessage = reasonToHoldBack;
                    nonPay.PaymentFailureReason = PaymentFailureType.HeldBackCompletionPayment;
                    return nonPay;
                });

                earningValidationResult.AddNonPayableEarnings(nonPaybles);
                payables = payables.Where(x => x.TransactionType != (int)TransactionType.Completion).ToList();
            }

            earningValidationResult.AddPayableEarnings(payables);
            return earningValidationResult;
        }

        public EarningValidationResult CreateNonPayableEarningsForNonZeroTransactionTypes(
            IEnumerable<RawEarning> earnings,
            string reason,
            PaymentFailureType paymentFailureReason,
            IHoldCommitmentInformation commitment = null)
        {
            var nonPayables = GetNonPayableEarningsForNonZeroTransactionTypes(earnings, reason, paymentFailureReason, commitment);
            var earningValidationResult = new EarningValidationResult();
            earningValidationResult.AddNonPayableEarnings(nonPayables);
            return earningValidationResult;
        }

        public EarningValidationResult AddPeriodToIgnore(int periods)
        {
            var earningValidationResult = new EarningValidationResult();
            earningValidationResult.PeriodsToIgnore.Add(periods);
            return earningValidationResult;
        }

        private List<FundingDue> GetFundingDueForNonZeroTransactionTypes(
            IEnumerable<RawEarning> earnings,
            IHoldCommitmentInformation commitment,
            int datalockType)
        {
            var payableEarnings = new List<FundingDue>();
            foreach (var rawEarning in earnings)
            {
                payableEarnings.AddRange(GetPayableEarnings(rawEarning, commitment, datalockType));
            }

            return payableEarnings;
        }

        private List<NonPayableEarning> GetNonPayableEarningsForNonZeroTransactionTypes(
            IEnumerable<RawEarning> earnings,
            string reason,
            PaymentFailureType paymentFailureReason,
            IHoldCommitmentInformation commitment = null)
        {
            var nonPayableEarnings = new List<NonPayableEarning>();
            foreach (var rawEarning in earnings)
            {
                nonPayableEarnings.AddRange(GetNonPayableEarnings(rawEarning, reason, paymentFailureReason, commitment));
            }
            return nonPayableEarnings;
        }

        private List<FundingDue> GetPayableEarnings(
            RawEarning rawEarnings,
            IHoldCommitmentInformation commitmentInformation = null,
            int censusType = -1)
        {
            var payableEarnings = new List<FundingDue>();
            for (var transactionType = 1; transactionType <= 15; transactionType++)
            {
                if (IgnoreTransactionTypeForASpecficCensusType(censusType, transactionType))
                {
                    continue;
                }

                var propertyName = $"TransactionType{transactionType:D2}";
                var amountDue = (decimal)FundingDueAccessor[rawEarnings, propertyName];
                if (amountDue == 0)
                {
                    continue;
                }
                var fundingDue = new FundingDue(rawEarnings);
                fundingDue.TransactionType = transactionType;

                if (!OnProgTransactionTypes.Contains(transactionType))
                {
                    fundingDue.SfaContributionPercentage = 1;
                }

                // Doing this to prevent a huge switch statement
                fundingDue.AmountDue = amountDue;
                commitmentInformation?.CopyCommitmentInformationTo(fundingDue);
                payableEarnings.Add(fundingDue);
            }

            return payableEarnings;
        }

        private List<NonPayableEarning> GetNonPayableEarnings(RawEarning rawEarnings,
            string reason,
            PaymentFailureType paymentFailureReason,
            IHoldCommitmentInformation commitmentInformation = null)
        {
            var nonPayableEarnings = new List<NonPayableEarning>();
            for (var transactionType = 1; transactionType <= 15; transactionType++)
            {
                var amountDue = (decimal)FundingDueAccessor[rawEarnings, $"TransactionType{transactionType:D2}"];
                if (amountDue == 0)
                {
                    continue;
                }

                var nonPayableEarning = new NonPayableEarning(rawEarnings);
                nonPayableEarning.TransactionType = transactionType;

                // Doing this to prevent a huge switch statement
                nonPayableEarning.AmountDue = amountDue;
                commitmentInformation?.CopyCommitmentInformationTo(nonPayableEarning);

                nonPayableEarning.PaymentFailureMessage = reason;
                nonPayableEarning.PaymentFailureReason = paymentFailureReason;
                nonPayableEarnings.Add(nonPayableEarning);
            }

            return nonPayableEarnings;
        }

        private static bool IgnoreTransactionTypeForASpecficCensusType(int censusType, int transactionType)
        {
            // TODO These all need to be enums
            if (censusType == -1)
            {
                return false;
            }

            if (censusType == 1 && (transactionType == 2 ||
                                      transactionType == 4 ||
                                      transactionType == 5 ||
                                      transactionType == 6 ||
                                      transactionType == 7
                ))
            {
                return true;
            }

            if (censusType == 2 && (transactionType != 4 && transactionType != 5))
            {
                return true;
            }

            if (censusType == 3 && (transactionType != 6 && transactionType != 7))
            {
                return true;
            }

            if (censusType == 4 && (transactionType != 2))
            {
                return true;
            }

            return false;
        }

        private bool HoldBackCompletionPayment(CompletionPaymentEvidence completionPaymentEvidence, out string reason)
        {
            if (completionPaymentEvidence.State == CompletionPaymentEvidenceState.ErrorOnIlr)
            {
                reason = "Error on PMR records in ILR";
                return true;
            }

            if (completionPaymentEvidence.State == CompletionPaymentEvidenceState.ExemptRedundancy ||
                completionPaymentEvidence.State == CompletionPaymentEvidenceState.ExemptOwnDelivery ||
                completionPaymentEvidence.State == CompletionPaymentEvidenceState.ExemptOtherReason)
            {
                reason = "";
                return false;
            }

            if (decimal.Round(completionPaymentEvidence.IlrEvidenceEmployerPayment) <
                decimal.Floor(completionPaymentEvidence.TotalHistoricEmployerPayment))
            {
                reason = "Historic Evidence does not show enough employer payments were made";
                return true;
            }

            reason = "";
            return false;
        }

    }
}
