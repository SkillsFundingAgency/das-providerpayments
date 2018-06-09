using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories
{
    public interface IDatalockOutputRepository
    {
        List<DatalockOutput> GetAllForProvider(long ukprn);
    }

    public class DatalockOutputRepository: DcfsRepository, IDatalockOutputRepository
    {
        public DatalockOutputRepository(string connectionString) : base(connectionString)
        {
        }

        public List<DatalockOutput> GetAllForProvider(long ukprn)
        {
            const string sql = @"
            SELECT *
            FROM [DataLock].[PriceEpisodePeriodMatch]
            WHERE Ukprn = @ukprn";

            var result = Query<DatalockOutput>(sql, new { ukprn })
                .ToList();

            return result;
        }
    }
}