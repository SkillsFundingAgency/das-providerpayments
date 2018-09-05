using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Utilities.Extensions
{
    public static class RawEarningTestExtensions
    {
        public static void ResetEarnings(this RawEarning earning)
        {
            earning.TransactionType01 = 0;
            earning.TransactionType02 = 0;
            earning.TransactionType03 = 0;
            earning.TransactionType04 = 0;
            earning.TransactionType05 = 0;
            earning.TransactionType06 = 0;
            earning.TransactionType07 = 0;
            earning.TransactionType08 = 0;
            earning.TransactionType09 = 0;
            earning.TransactionType10 = 0;
            earning.TransactionType11 = 0;
            earning.TransactionType12 = 0;
            earning.TransactionType15 = 0;
        }
    }
}
