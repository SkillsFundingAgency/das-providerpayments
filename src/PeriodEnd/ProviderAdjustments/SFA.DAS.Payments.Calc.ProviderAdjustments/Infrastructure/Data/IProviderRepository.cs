using SFA.DAS.Payments.Calc.ProviderAdjustments.Infrastructure.Data.Entities;

namespace SFA.DAS.Payments.Calc.ProviderAdjustments.Infrastructure.Data
{
    public interface IProviderRepository
    {
        ProviderEntity[] GetAllProviders();
    }
}