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
    public class BuildEarningValidationResult 
    {

        private static readonly List<int> OnProgTransactionTypes = new List<int> { 1, 2, 3 };
        private static readonly TypeAccessor FundingDueAccessor = TypeAccessor.Create(typeof(RawEarning));
        private EarningValidationResult earningValidationResult;

        public BuildEarningValidationResult()
        {
            earningValidationResult = new EarningValidationResult();
        }

        public EarningValidationResult CreatEarningValidationResult() => earningValidationResult;

        public void AddPayableEarningsButHoldBackCompletionPaymentIfNecessary(
            IEnumerable<RawEarning> earnings,
            IHoldCommitmentInformation commitment = null,
            CompletionPaymentEvidence completionPaymentEvidence = null,
            int datalockType = -1)
        {

            var reasonToHoldBack = "";
            var holdBack = false;
            if (completionPaymentEvidence != null)
            {
                holdBack = HoldBackCompletionPayment(completionPaymentEvidence, out reasonToHoldBack);
            }

            var payables = GetFundingDueForNonZeroTransactionTypes(earnings, commitment, datalockType);

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
        }

        public void AddNonPayableEarningsForNonZeroTransactionTypes(
            IEnumerable<RawEarning> earnings,
            string reason,
            PaymentFailureType paymentFailureReason,
            IHoldCommitmentInformation commitment = null)
        {
            var nonPayables = GetNonPayableEarningsForNonZeroTransactionTypes(earnings, reason, paymentFailureReason, commitment);
            earningValidationResult.AddNonPayableEarnings(nonPayables);
        }

        public void AddPeriodToIgnore(int periods)
        {
            earningValidationResult.PeriodsToIgnore.Add(periods);
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
            int datalockType = -1)
        {
            var payableEarnings = new List<FundingDue>();
            for (var transactionType = 1; transactionType <= 15; transactionType++)
            {
                if (datalockType != -1 && IgnoreTransactionType(datalockType, transactionType))
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

        private static bool IgnoreTransactionType(int datalockType, int transactionType)
        {

            // TODO: These should be enums i think
            if (datalockType == 2 && (transactionType != 4 && transactionType != 5))
            {
                return true;
            }

            if (datalockType == 3 && (transactionType != 6 && transactionType != 7))
            {
                return true;
            }

            if (datalockType == 1 && (transactionType == 4 ||
                                      transactionType == 5 ||
                                      transactionType == 6 ||
                                      transactionType == 7))
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
