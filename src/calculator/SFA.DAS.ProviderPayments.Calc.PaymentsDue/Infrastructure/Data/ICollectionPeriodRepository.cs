using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data
{
    public interface ICollectionPeriodRepository
    {
        CollectionPeriodEntity GetCurrentCollectionPeriod();
    }
}