using SFA.DAS.ProviderPayments.Calc.Common.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Repositories
{
    public class DcfsCollectionPeriodRepository : DcfsRepository, ICollectionPeriodRepository
    {
        private const int OpenStatus = 1;

        private const string CollectionPeriodSource = "LevyPayments.vw_CollectionPeriods";
        private const string CollectionPeriodColumns = "Period_ID [PeriodId]," +
                                                       "Period [Month]," +
                                                       "Calendar_Year [Year]," +
                                                       "Collection_Period [Name]";
        private const string SelectCollectionPeriods = "SELECT " + CollectionPeriodColumns + " FROM " + CollectionPeriodSource;
        private const string SelectOpenCollectionPeriod = SelectCollectionPeriods + " WHERE Collection_Open = @CollectionOpen";

        public DcfsCollectionPeriodRepository(string connectionString)
            : base(connectionString)
        {
        }

        public CollectionPeriodEntity GetCurrentCollectionPeriod()
        {
            return QuerySingle<CollectionPeriodEntity>(SelectOpenCollectionPeriod, new { CollectionOpen = OpenStatus });
        }
    }
}