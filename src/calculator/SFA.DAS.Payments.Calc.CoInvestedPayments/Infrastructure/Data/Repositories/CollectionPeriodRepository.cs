using SFA.DAS.ProviderPayments.Calc.Common.Infrastructure.Data;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.Infrastructure.Data.Repositories
{
    public class CollectionPeriodRepository : DcfsRepository, ICollectionPeriodRepository
    {
        private const int OpenStatus = 1;

        private const string CollectionPeriodSource = "CoInvestedPayments.vw_CollectionPeriods";
        private const string CollectionPeriodColumns = "Period_ID [PeriodId]," +
                                                       "Period [Month]," +
                                                       "Calendar_Year [Year]," +
                                                       "Collection_Period [Name]";
        private const string SelectCollectionPeriods = "SELECT " + CollectionPeriodColumns + " FROM " + CollectionPeriodSource;
        private const string SelectOpenCollectionPeriod = SelectCollectionPeriods + " WHERE Collection_Open = @CollectionOpen";

        public CollectionPeriodRepository(string connectionString)
            : base(connectionString)
        {
        }

        public CollectionPeriodEntity GetCurrentCollectionPeriod()
        {
            return QuerySingle<CollectionPeriodEntity>(SelectOpenCollectionPeriod, new { CollectionOpen = OpenStatus });
        }
    }
}