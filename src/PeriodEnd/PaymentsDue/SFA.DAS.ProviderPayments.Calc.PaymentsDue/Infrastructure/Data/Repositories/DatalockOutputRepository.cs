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
            SELECT PM.*
            FROM [DataLock].[PriceEpisodePeriodMatch] PM
            JOIN [DataLock].[PriceEpisodeMatch] M ON M.PriceEpisodeIdentifier = PM.PriceEpisodeIdentifier AND M.UkPrn = PM.UkPrn
            WHERE PM.Ukprn = @ukprn AND M.IsSuccess = 1";

            var result = Query<DatalockOutput>(sql, new { ukprn })
                .ToList();

            return result;
        }
    }
}