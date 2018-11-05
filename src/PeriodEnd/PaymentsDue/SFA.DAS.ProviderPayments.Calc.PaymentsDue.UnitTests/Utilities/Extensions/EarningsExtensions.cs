using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.Extensions
{
    public static class EarningsExtensions
    {
        public static decimal TotalAmount(this IEnumerable<RawEarning> earnings)
        {
            return earnings.Sum(x => x.TotalAmount());
        }

        public static decimal TotalAmount(this RawEarning earning)
        {
            return earning.TransactionType01 +
                   earning.TransactionType02 +
                   earning.TransactionType03 +
                   earning.TransactionType04 +
                   earning.TransactionType05 +
                   earning.TransactionType06 +
                   earning.TransactionType07 +
                   earning.TransactionType08 +
                   earning.TransactionType09 +
                   earning.TransactionType10 +
                   earning.TransactionType11 +
                   earning.TransactionType12 +
                   earning.TransactionType13 +
                   earning.TransactionType14 +
                   earning.TransactionType15 +
                   earning.TransactionType16;
        }

        public static int NumberOfNonZeroTransactions(this IEnumerable<RawEarning> earnings)
        {
            return earnings.Sum(x => x.NumberOfNonZeroTransactions());
        }

        public static int NumberOfNonZeroTransactions(this RawEarning earning)
        {
            return ((earning.TransactionType01 != 0) ? 1 : 0) +
                   ((earning.TransactionType02 != 0) ? 1 : 0) +
                   ((earning.TransactionType03 != 0) ? 1 : 0) +
                   ((earning.TransactionType04 != 0) ? 1 : 0) +
                   ((earning.TransactionType05 != 0) ? 1 : 0) +
                   ((earning.TransactionType06 != 0) ? 1 : 0) +
                   ((earning.TransactionType07 != 0) ? 1 : 0) +
                   ((earning.TransactionType08 != 0) ? 1 : 0) +
                   ((earning.TransactionType09 != 0) ? 1 : 0) +
                   ((earning.TransactionType10 != 0) ? 1 : 0) +
                   ((earning.TransactionType11 != 0) ? 1 : 0) +
                   ((earning.TransactionType12 != 0) ? 1 : 0) +
                   ((earning.TransactionType13 != 0) ? 1 : 0) +
                   ((earning.TransactionType14 != 0) ? 1 : 0) +
                   ((earning.TransactionType15 != 0) ? 1 : 0) +
                   ((earning.TransactionType16 != 0) ? 1 : 0);
        }
    }
}
