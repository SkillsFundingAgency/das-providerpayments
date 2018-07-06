using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data
{
    public interface IRequiredPaymentRepository
    {
        IEnumerable<RequiredPaymentEntity> GetRefundsForProvider(long ukprn);
    }
}