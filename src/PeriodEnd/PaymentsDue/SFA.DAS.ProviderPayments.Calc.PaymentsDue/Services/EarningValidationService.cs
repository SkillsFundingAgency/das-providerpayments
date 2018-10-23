using System;
using System.Collections.Generic;
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

        public EarningValidationResult IgnorePeriod(int period)
        {
            var earningValidationResult = new EarningValidationResult();
            earningValidationResult.PeriodsToIgnore.Add(period);
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
            int censusDateType = -1)
        {
            var payableEarnings = ExpandEarning<FundingDue>(rawEarnings, commitmentInformation, censusDateType);
            
            return payableEarnings;
        }

        private List<NonPayableEarning> GetNonPayableEarnings(RawEarning rawEarnings,
            string reason,
            PaymentFailureType paymentFailureReason,
            IHoldCommitmentInformation commitmentInformation = null)
        {
            var nonPayableEarnings = ExpandEarning<NonPayableEarning>(rawEarnings, commitmentInformation);
            nonPayableEarnings.ForEach(x =>
            {
                x.PaymentFailureMessage = reason;
                x.PaymentFailureReason = paymentFailureReason;
            });
            
            return nonPayableEarnings;
        }

        private List<T> ExpandEarning<T>(
            RawEarning earning,
            IHoldCommitmentInformation commitment = null,
            int censusDateType = -1)
            where T : FundingDue, new()
        {
            var expandedList = new List<T>();
            foreach (var transactionTypeValue in Enum.GetValues(typeof(TransactionType)))
            {
                var transactionType = (int) transactionTypeValue;

                if (IgnoreTransactionTypeForASpecficCensusDateType(censusDateType, transactionType))
                {
                    continue;
                }

                var amountDue = (decimal) FundingDueAccessor[earning, $"TransactionType{transactionType:D2}"];
                if (amountDue == 0)
                {
                    continue;
                }

                var constructor = typeof(T).GetConstructor(new[] {typeof(RawEarning)});
                var funding = constructor.Invoke(new object[] {earning}) as T;
                funding.TransactionType = transactionType;

                if (!PotentialLevyTransactionTypes.Contains(transactionType))
                {
                    funding.SfaContributionPercentage = 1;
                }

                // Doing this to prevent a huge switch statement
                funding.AmountDue = amountDue;
                commitment?.CopyCommitmentInformationTo(funding);

                expandedList.Add(funding);
            }

            return expandedList;
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
                                        transactionType == 7 ||
                                        transactionType == 9 ||
                                        transactionType == 10
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
