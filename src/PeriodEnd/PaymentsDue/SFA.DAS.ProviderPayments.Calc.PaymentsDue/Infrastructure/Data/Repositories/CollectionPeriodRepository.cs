using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories
{
    public class CollectionPeriodRepository : DcfsRepository, ICollectionPeriodRepository
    {
        private const string CollectionPeriodSource = "Reference.CollectionPeriods";

        private const string CollectionPeriodColumns = @"Id,
                                                        [Name],
                                                        CalendarMonth [Month],
                                                        CalendarYear [Year],
                                                        [Open],
                                                        AcademicYear";

        private const string SelectCollectionPeriods = "SELECT " + CollectionPeriodColumns + " FROM " + CollectionPeriodSource;

        
        private List<CollectionPeriodEntity> CollectionPeriods { get; set; } = new List<CollectionPeriodEntity>();
        private CollectionPeriodEntity _openCollectionPeriod = null;
        private bool _collectionPeriodLoaded = false;

        public CollectionPeriodRepository(string connectionString)
            : base(connectionString)
        {
        }

        private void LoadCollectionPeriods()
        {
            CollectionPeriods = LoadAllCollectionPeriods();
            _openCollectionPeriod = CollectionPeriods.First(x => x.Open);
            _collectionPeriodLoaded = true;
        }

        public CollectionPeriodEntity GetCurrentCollectionPeriod()
        {
            if (!_collectionPeriodLoaded)
            {
                LoadCollectionPeriods();
            }

            return _openCollectionPeriod;
        }

        private List<CollectionPeriodEntity> LoadAllCollectionPeriods()
        {
            return Query<CollectionPeriodEntity>(SelectCollectionPeriods).ToList();
        }

        public List<CollectionPeriodEntity> GetAllCollectionPeriods()
        {
            if (!_collectionPeriodLoaded)
            {
                LoadCollectionPeriods();
            }

            return CollectionPeriods;
        }
    }
}