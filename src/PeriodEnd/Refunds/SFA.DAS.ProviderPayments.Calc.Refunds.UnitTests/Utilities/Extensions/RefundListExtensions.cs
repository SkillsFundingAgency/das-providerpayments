using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.UnitTests.Utilities.Extensions
{
    public static class RefundListExtensions
    {
        public static RequiredPaymentEntity Latest(this List<RequiredPaymentEntity> source)
        {
            var highestYear = source.Max(x => x.DeliveryYear);
            var refundsForHighestYear = source.Where(x => x.DeliveryYear == highestYear).ToList();
            var latestRefundMonth = refundsForHighestYear.Max(x => x.DeliveryMonth);
            var latestRefund = refundsForHighestYear.FirstOrDefault(x => x.DeliveryMonth == latestRefundMonth);
            return latestRefund;
        }
    }
}
