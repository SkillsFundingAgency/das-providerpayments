using SFA.DAS.ProviderPayments.Api.Dto.Hal;

namespace SFA.DAS.ProviderPayments.Api.Dto
{
    public class AccountDto : HalResource<PaymentEntityLinks>
    {
        public string Id { get; set; }
    }
}
