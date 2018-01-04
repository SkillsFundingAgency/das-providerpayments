using SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data.Repositories
{
    public class CollectionPeriodRepository : DcfsRepository, ICollectionPeriodRepository
    {
        public CollectionPeriodRepository(string connectionString)
            : base(connectionString)
        {
        }

        public CollectionPeriodEntity GetCurrentCollectionPeriod()
        {
            return QuerySingleByProc<CollectionPeriodEntity>("CoInvestedPayments.GetOpenCollectionPeriods");
        }
    }
}