using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Extensions
{
    public static class EarningsExtensions
    {
        public static bool HasNonZeroTransactions(this RawEarning source)
        {
            return source.TransactionType01 != 0 ||
                   source.TransactionType02 != 0 ||
                   source.TransactionType03 != 0 ||
                   source.TransactionType04 != 0 ||
                   source.TransactionType05 != 0 ||
                   source.TransactionType06 != 0 ||
                   source.TransactionType07 != 0 ||
                   source.TransactionType08 != 0 ||
                   source.TransactionType09 != 0 ||
                   source.TransactionType10 != 0 ||
                   source.TransactionType11 != 0 ||
                   source.TransactionType12 != 0 ||
                   source.TransactionType13 != 0 ||
                   source.TransactionType14 != 0 ||
                   source.TransactionType15 != 0 ||
                   source.TransactionType16 != 0;
        }
    }
}
