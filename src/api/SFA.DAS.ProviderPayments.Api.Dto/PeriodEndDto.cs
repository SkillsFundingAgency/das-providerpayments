using System;
using SFA.DAS.ProviderPayments.Api.Dto.Hal;

namespace SFA.DAS.ProviderPayments.Api.Dto
{
    public class PeriodEndDto : HalResource<PeriodLinks>
    {
        public PeriodDto Period { get; set; }
        public decimal TotalValue { get; set; }
        public int NumberOfProviders { get; set; }
        public int NumberOfEmployers { get; set; }
        public DateTime PaymentRunDate { get; set; }
    }

    public class PeriodLinks : HalLinks
    {
        public HalLink Accounts { get; set; }
    }
}
