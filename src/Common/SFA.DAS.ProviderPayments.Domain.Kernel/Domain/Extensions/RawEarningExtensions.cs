using System;
using FastMember;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Domain.Kernel.Domain.Extensions
{
    public static class RawEarningExtensions
    {
        private static readonly TypeAccessor FundingDueAccessor = TypeAccessor.Create(typeof(RawEarning));

        public static bool HasValidTransactionsForCensusDateType(
            this RawEarning earning,
            CensusDateType censusDateType)
        {
            if (!earning.HasNonZeroTransactionsForCensusDateType(censusDateType))
            {
                return false;
            }

            var dependentPropertyName = censusDateType.DependentPropertyName();
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

        public static bool HasNonZeroTransactionsForCensusDateType(
            this RawEarning earning,
            CensusDateType censusDateType)
        {
            var transactionsToCheck = censusDateType.ValidTransactionTypes();
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
