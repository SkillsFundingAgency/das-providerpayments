using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Infrastructure.Entities;

namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Infrastructure.Dcfs
{
    public class DcfsCollectionPeriodRepository : DcfsRepository, ICollectionPeriodRepository
    {
        private const string CollectionPeriodSource = "CollectionPeriods";

        public DcfsCollectionPeriodRepository(string connectionString) 
            : base(connectionString)
        {
        }

        public CollectionPeriodEntity GetOpenCollectionPeriod()
        {
            return QuerySingle<CollectionPeriodEntity>("SELECT [Name], CalendarMonth, CalendarYear FROM Reference.CollectionPeriods WHERE [Open] = 1");
        }
    }
}