using SFA.DAS.ProviderPayments.Api.Dto.Hal;

namespace SFA.DAS.ProviderPayments.Api.Dto
{
    public class PaymentEntityLinks : HalLinks
    {
        public HalLink Payments { get; set; }
    }
}