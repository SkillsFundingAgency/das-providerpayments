using System;
using System.Collections.Generic;
using System.Linq;
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
            IHoldCommitmentInformation commitment = null)
        {
            var earningValidationResult = new EarningValidationResult();
            var earningsAsList = earnings.ToList();
            foreach (TransactionTypeGroup transactionTypeGroup in Enum.GetValues(typeof(TransactionTypeGroup)))
            {
                earningValidationResult
                    .AddPayableEarnings(EarningsToFundingDue<NonPayableEarning>(
                        earningsAsList,
                        commitment,
                        transactionTypeGroup));
            }

            return earningValidationResult;
        }

        public EarningValidationResult CreatePayableFundingDue(
            IEnumerable<RawEarning> earnings,
            TransactionTypeGroup cenususTypeGroup,
            IHoldCommitmentInformation commitment = null)
        {
            var earningValidationResult = new EarningValidationResult();
            var payables = EarningsToFundingDue<FundingDue>(earnings, commitment, cenususTypeGroup);
            earningValidationResult.AddPayableEarnings(payables);
            return earningValidationResult;
        }

        public EarningValidationResult CreateNonPayableFundingDue(
            IEnumerable<RawEarning> earnings,
            string reason,
            PaymentFailureType paymentFailureReason,
            IHoldCommitmentInformation commitment = null)
        {
            var nonPayables = new List<NonPayableEarning>();
            var earningsAsList = earnings.ToList();

            foreach (TransactionTypeGroup transactionTypeGroup in Enum.GetValues(typeof(TransactionTypeGroup)))
            {
                nonPayables.AddRange(EarningsToFundingDue<NonPayableEarning>(earningsAsList, commitment, transactionTypeGroup));
            }
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
            TransactionTypeGroup transactionTypeGroup)
            where T : FundingDue
        {
            var payableEarnings = new List<T>();
            foreach (var rawEarning in earnings)
            {
                payableEarnings.AddRange(ExpandEarning<T>(rawEarning, transactionTypeGroup, commitment));
            }

            return payableEarnings;
        }

        private List<T> ExpandEarning<T>(
           RawEarning earning,
           TransactionTypeGroup transactionTypeGroup,
           IHoldCommitmentInformation commitment = null
           )
           where T : FundingDue
        {
            var expandedList = new List<T>();
            foreach (TransactionType transactionTypeValue in Enum.GetValues(typeof(TransactionType)))
            {
                var transactionType = (int)transactionTypeValue;

                if (!transactionTypeValue.ValidForTransactionTypeGroup(transactionTypeGroup))
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
