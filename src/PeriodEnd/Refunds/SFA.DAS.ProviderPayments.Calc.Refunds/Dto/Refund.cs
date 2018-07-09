using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.Dto
{
    public class Refund : PaymentEntity
    {
        public long AccountId { get; set; }
    }
}