using System;
using FastMember;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Domain.Kernel.Domain.Extensions
{
    public static class RawEarningExtensions
    {
        private static readonly TypeAccessor FundingDueAccessor = TypeAccessor.Create(typeof(RawEarning));

        public static DateTime? DependantDate(this RawEarning earning, TransactionTypeGroup transactionTypeGroup)
        {
            var dependentProperty = transactionTypeGroup.DependentPropertyName();
            var dependentPropertyValue = FundingDueAccessor[earning, dependentProperty] as DateTime?;
            return dependentPropertyValue;
        }

        public static bool HasValidTransactionsForTransactionTypeGroup(
            this RawEarning earning,
            TransactionTypeGroup transactionTypeGroup)
        {
            // I hate this... but it makes Maths & English payments work when they cross years
            if (earning.IsBlankPeriodOneEarning(transactionTypeGroup)) return true;

            if (!earning.HasNonZeroTransactionsForGroup(transactionTypeGroup))
            {
                return false;
            }

            var dependentPropertyName = transactionTypeGroup.DependentPropertyName();
            if (dependentPropertyName == null)
            {
                return true;
            }

            var propertyValue = (DateTime?)FundingDueAccessor[earning, dependentPropertyName];
            if (propertyValue != null)
            {
                return true;
            }

            return false;
        }

        public static bool IsBlankPeriodOneEarning(this RawEarning earning, TransactionTypeGroup transactionTypeGroupToCheck)
        {
            if (earning.Period == 1 && transactionTypeGroupToCheck == TransactionTypeGroup.OnProgLearning)
            {
                var otherTransactions = false;
                foreach (TransactionTypeGroup transactionTypeGroup in Enum.GetValues(typeof(TransactionTypeGroup)))
                {
                    if (transactionTypeGroup == TransactionTypeGroup.OnProgLearning)
                    {
                        continue;
                    }

                    if (earning.HasValidTransactionsForTransactionTypeGroup(transactionTypeGroup))
                    {
                        otherTransactions = true;
                        break;
                    }
                }

                if (!otherTransactions)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool HasNonZeroTransactionsForGroup(
            this RawEarning earning,
            TransactionTypeGroup transactionTypeGroup)
        {
            var transactionsToCheck = transactionTypeGroup.ValidTransactionTypes();
            foreach (var transactionType in transactionsToCheck)
            {
                var transactionAmount = (decimal)FundingDueAccessor[earning, $"TransactionType{(int)transactionType:D2}"];
                if (transactionAmount != 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
