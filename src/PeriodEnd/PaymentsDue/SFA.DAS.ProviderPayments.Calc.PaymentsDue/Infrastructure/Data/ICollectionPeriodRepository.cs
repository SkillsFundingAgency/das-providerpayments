using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using System.Collections.Generic;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data
{
    public interface ICollectionPeriodRepository
    {
        CollectionPeriodEntity GetCurrentCollectionPeriod();
        List<CollectionPeriodEntity> GetAllCollectionPeriods();
    }
}