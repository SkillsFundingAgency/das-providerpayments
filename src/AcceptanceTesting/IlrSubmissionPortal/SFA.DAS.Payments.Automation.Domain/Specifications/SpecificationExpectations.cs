using System.Collections.Generic;

namespace SFA.DAS.Payments.Automation.Domain.Specifications
{
    public class SpecificationExpectations
    {
        public List<ProviderEarningsAndPayments> EarningsAndPayments { get; set; } = new List<ProviderEarningsAndPayments>();
    }
}