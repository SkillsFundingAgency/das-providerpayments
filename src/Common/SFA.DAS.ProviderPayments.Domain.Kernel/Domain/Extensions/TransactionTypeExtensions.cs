using SFA.DAS.Payments.DCFS.Domain;

namespace SFA.DAS.ProviderPayments.Domain.Kernel.Domain.Extensions
{
    public static class TransactionTypeExtensions
    {
        public static bool ValidForTransactionTypeGroup(this TransactionType source, TransactionTypeGroup transactionTypeGroup)
        {
            return transactionTypeGroup.ValidTransactionTypes().Contains(source);
        }
    }
}
