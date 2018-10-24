using System;
using System.Collections.Generic;
using FastMember;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Dto;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Extensions;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Domain.Kernel.Domain.Extensions;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services
{
    public class EarningToFundingDueService
    {
        private static readonly HashSet<TransactionType> PotentialLevyTransactionTypes =
            new HashSet<TransactionType> { TransactionType.Learning, TransactionType.Completion, TransactionType.Balancing };
        private static readonly TypeAccessor FundingDueAccessor = TypeAccessor.Create(typeof(RawEarning));

        public EarningValidationResult CreatePayableFundingDue(
            IEnumerable<RawEarning> earnings,
            IHoldCommitmentInformation commitment = null,
            CensusDateType cenususType = CensusDateType.All)
        {
            var earningValidationResult = new EarningValidationResult();
            var payables = EarningsToFundingDue<FundingDue>(earnings, commitment, cenususType);
            earningValidationResult.AddPayableEarnings(payables);
            return earningValidationResult;
        }

        public EarningValidationResult CreateNonPayableFundingDue(
            IEnumerable<RawEarning> earnings,
            string reason,
            PaymentFailureType paymentFailureReason,
            IHoldCommitmentInformation commitment = null)
        {
            var nonPayables = EarningsToFundingDue<NonPayableEarning>(earnings, commitment, CensusDateType.All);
            nonPayables.ForEach(x =>
            {
                x.PaymentFailureMessage = reason;
                x.PaymentFailureReason = paymentFailureReason;
            });
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

        private List<T> EarningsToFundingDue<T>(
            IEnumerable<RawEarning> earnings,
            IHoldCommitmentInformation commitment,
            CensusDateType censusDateType)
            where T : FundingDue, new()
        {
            var payableEarnings = new List<T>();
            foreach (var rawEarning in earnings)
            {
                payableEarnings.AddRange(ExpandEarning<T>(rawEarning, commitment, censusDateType));
            }

            return payableEarnings;
        }

        private List<T> ExpandEarning<T>(
           RawEarning earning,
           IHoldCommitmentInformation commitment = null,
           CensusDateType censusDateType = CensusDateType.All)
           where T : FundingDue, new()
        {
            var expandedList = new List<T>();
            foreach (TransactionType transactionTypeValue in Enum.GetValues(typeof(TransactionType)))
            {
                var transactionType = (int)transactionTypeValue;

                if (!transactionTypeValue.ValidForCensusDateType(censusDateType))
                {
                    continue;
                }

                var amountDue = (decimal)FundingDueAccessor[earning, $"TransactionType{transactionType:D2}"];
                if (amountDue == 0)
                {
                    continue;
                }

                var constructor = typeof(T).GetConstructor(new[] { typeof(RawEarning) });
                var funding = constructor?.Invoke(new object[] { earning }) as T;

                if (funding == null)
                {
                    throw new Exception($"Could not create {typeof(T).FullName} - failing process");
                }
                funding.TransactionType = transactionType;

                if (TransactionTypeIsSfaOnly(transactionTypeValue))
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

        private static bool TransactionTypeIsSfaOnly(TransactionType transactionType)
        {
            return !PotentialLevyTransactionTypes.Contains(transactionType);
        }
    }
}
