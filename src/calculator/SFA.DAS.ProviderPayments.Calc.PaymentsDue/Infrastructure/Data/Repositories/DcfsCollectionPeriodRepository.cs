using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories
{
    public class DcfsCollectionPeriodRepository : DcfsRepository, ICollectionPeriodRepository
    {
        private const int OpenStatus = 1;

        private const string CollectionPeriodSource = "PaymentsDue.vw_CollectionPeriods";
        private const string CollectionPeriodColumns = "Period_ID [PeriodId]," +
                                              "Period [Month]," +
                                              "Calendar_Year [Year]," +
                                              "Collection_Period PeriodName";
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

        public List<CollectionPeriodEntity> GetAllCollectionPeriods()
        {
            return Query<CollectionPeriodEntity>(SelectCollectionPeriods).ToList();
        }
    }
}