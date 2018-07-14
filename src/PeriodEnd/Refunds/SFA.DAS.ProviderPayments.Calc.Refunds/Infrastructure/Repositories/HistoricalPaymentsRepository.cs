using System.Collections.Generic;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.Payments.DCFS.StructureMap.Infrastructure;
using SFA.DAS.ProviderPayments.Calc.Refunds.Infrastructure.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.Infrastructure.Repositories
{
    public interface IHistoricalPaymentsRepository
    {
        IEnumerable<HistoricalPaymentEntity> GetAllForProvider(long ukprn);
    }

    public class HistoricalPaymentsRepository : DcfsRepository, IHistoricalPaymentsRepository
    {
        private readonly string _collectionYear;

        public HistoricalPaymentsRepository(IHoldDcConfiguration configuration)
            : base(configuration.TransientConnectionString)
        {
            _collectionYear = configuration.CollectionYear;
        }

        public IEnumerable<HistoricalPaymentEntity> GetAllForProvider(long ukprn)
        {
            string sql = @"
            SELECT *
            FROM Reference.PaymentsHistory
            WHERE Ukprn = @ukprn
            AND CollectionPeriodName LIKE '" + _collectionYear + @"-R%'
            ";

            var result = Query<HistoricalPaymentEntity>(sql, new { ukprn });

            return result;
        }
    }
}