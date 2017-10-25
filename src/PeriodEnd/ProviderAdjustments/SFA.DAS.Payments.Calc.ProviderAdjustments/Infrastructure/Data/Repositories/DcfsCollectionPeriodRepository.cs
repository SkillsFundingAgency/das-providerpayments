using SFA.DAS.Payments.Calc.ProviderAdjustments.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;

namespace SFA.DAS.Payments.Calc.ProviderAdjustments.Infrastructure.Data.Repositories
{
    public class DcfsCollectionPeriodRepository : DcfsRepository, ICollectionPeriodRepository
    {
        private const int OpenStatus = 1;

        private const string CollectionPeriodSource = "ProviderAdjustments.vw_CollectionPeriods";
        private const string CollectionPeriodColumns = "PeriodId, " +
                                              "Month, " +
                                              "Year, " +
                                              "Name";
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