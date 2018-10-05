using System.Collections.Generic;
using FastMember;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Extensions;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services
{
    public class EarningValidationService 
    {
        private static readonly HashSet<int> PotentialLevyTransactionTypes = new HashSet<int> { 1, 2, 3 };
        private static readonly TypeAccessor FundingDueAccessor = TypeAccessor.Create(typeof(RawEarning));

        public EarningValidationResult CreatePayableEarnings(
            IEnumerable<RawEarning> earnings,
            IHoldCommitmentInformation commitment = null,
            int cenususType = -1)
        {
            var earningValidationResult = new EarningValidationResult();
            
            var payables = GetFundingDueForNonZeroTransactionTypes(earnings, commitment, cenususType);

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
                if (IgnoreTransactionTypeForASpecficCensusDateType(censusType, transactionType))
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

                if (!PotentialLevyTransactionTypes.Contains(transactionType))
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

        private static bool IgnoreTransactionTypeForASpecficCensusDateType(int censusDateType, int transactionType)
        {
            // TODO These all need to be enums
            if (censusDateType == -1)
            {
                return false;
            }

            if (censusDateType == 1 && (transactionType == 2 ||
                                      transactionType == 3 ||
                                      transactionType == 4 ||
                                      transactionType == 5 ||
                                      transactionType == 6 ||
                                      transactionType == 7
                ))
            {
                return true;
            }

            if (censusDateType == 2 && (transactionType != 4 && transactionType != 5))

            {
                return true;
            }

            if (censusDateType == 3 && (transactionType != 6 && transactionType != 7))
            {
                return true;
            }

            if (censusDateType == 4 && (transactionType != 2 && 
                                        transactionType != 3 &&
                                        transactionType != 9 &&
                                        transactionType != 10))
            {
                return true;
            }

            if (censusDateType == 5 && (transactionType != 16))
            {
                return true;
            }

            return false;
        }
    }
}
