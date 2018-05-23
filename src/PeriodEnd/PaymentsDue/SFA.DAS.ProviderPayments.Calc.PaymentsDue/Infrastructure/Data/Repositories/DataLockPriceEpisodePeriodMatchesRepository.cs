using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories
{
    public interface IDataLockPriceEpisodePeriodMatchesRepository
    {
        List<DataLockPriceEpisodePeriodMatchEntity> GetAllForProvider(long ukprn);
    }

    public class DataLockPriceEpisodePeriodMatchesRepository: DcfsRepository, IDataLockPriceEpisodePeriodMatchesRepository
    {
        public DataLockPriceEpisodePeriodMatchesRepository(string connectionString) : base(connectionString)
        {
        }

        public List<DataLockPriceEpisodePeriodMatchEntity> GetAllForProvider(long ukprn)
        {
            const string sql = @"
            SELECT *
            FROM [DataLock].[PriceEpisodePeriodMatch]
            WHERE Ukprn = @ukprn";

            var result = Query<DataLockPriceEpisodePeriodMatchEntity>(sql, new { ukprn })
                .ToList();

            return result;
        }
    }
}