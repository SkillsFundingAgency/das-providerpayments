using System.Collections.Generic;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.Refunds.Infrastructure.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Refunds.Infrastructure.Repositories
{
    public interface IHistoricalPaymentsRepository
    {
        IEnumerable<HistoricalPaymentEntity> GetAllForProvider(long ukprn);
    }

    public class HistoricalPaymentsRepository : DcfsRepository, IHistoricalPaymentsRepository
    {
        public HistoricalPaymentsRepository(string connectionString) 
            : base(connectionString) { }

        public IEnumerable<HistoricalPaymentEntity> GetAllForProvider(long ukprn)
        {
            const string sql = @"
            SELECT *
            FROM Reference.PaymentsHistory
            WHERE Ukprn = @ukprn
            ";

            var result = Query<HistoricalPaymentEntity>(sql, new { ukprn });

            return result;
        }
    }
}