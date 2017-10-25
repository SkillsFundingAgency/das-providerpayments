using System.Collections.Generic;

namespace SFA.DAS.Payments.Automation.Domain.Specifications
{
    public class ProviderEarningsAndPayments
    {
        public string ProviderKey { get; set; } = Defaults.ProviderKey;
        public List<PeriodEarningAndPayments> EarningAndPaymentsByPeriod { get; set; } = new List<PeriodEarningAndPayments>();
    }
}