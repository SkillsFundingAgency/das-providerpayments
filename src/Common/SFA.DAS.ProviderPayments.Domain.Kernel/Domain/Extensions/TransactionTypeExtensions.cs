using SFA.DAS.Payments.DCFS.Domain;

namespace SFA.DAS.ProviderPayments.Domain.Kernel.Domain.Extensions
{
    public static class TransactionTypeExtensions
    {
        public static bool ValidForCensusDateType(this TransactionType source, CensusDateType censusDateType)
        {
            return censusDateType.ValidTransactionTypes().Contains(source);
        }
    }
}
