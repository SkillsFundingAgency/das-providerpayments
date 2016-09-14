using SFA.DAS.ProviderPayments.Calc.PaymentSchedule.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentSchedule.Infrastructure.Data
{
    public interface ICollectionPeriodRepository
    {
        CollectionPeriodEntity GetCurrentCollectionPeriod();
    }
}