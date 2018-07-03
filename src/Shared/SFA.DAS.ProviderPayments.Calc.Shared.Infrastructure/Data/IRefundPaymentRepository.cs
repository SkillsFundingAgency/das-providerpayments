using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data
{
    public interface IRefundPaymentRepository
    {
        void AddMany(List<RefundPaymentEntity> refunds);
    }

}