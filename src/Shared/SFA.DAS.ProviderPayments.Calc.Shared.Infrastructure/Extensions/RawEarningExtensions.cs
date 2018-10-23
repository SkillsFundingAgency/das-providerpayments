using FastMember;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Common.Domain.Extensions
{
    public static class RawEarningExtensions
    {
        private static readonly TypeAccessor FundingDueAccessor = TypeAccessor.Create(typeof(RawEarning));

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
