using SFA.DAS.ProviderPayments.Calc.Refunds.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.Infrastructure.Data
{
    public interface IProviderRepository
    {
        ProviderEntity[] GetAllProviders();
        ProviderEntity GetProvider(long ukprn);
    }
}