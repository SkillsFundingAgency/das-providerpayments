using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Infrastructure.Entities;

namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Infrastructure
{
    public interface ICollectionPeriodRepository
    {
        CollectionPeriodEntity GetOpenCollectionPeriod();
    }
}