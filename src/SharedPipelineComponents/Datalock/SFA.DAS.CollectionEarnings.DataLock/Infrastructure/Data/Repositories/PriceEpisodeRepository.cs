using System.Data;
using Dapper;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;

namespace SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Repositories
{
    public class PriceEpisodeRepository : DcfsRepository, IPriceEpisodeRepository
    {
        public PriceEpisodeRepository(string connectionString)
            : base(connectionString)
        {
        }

        public PriceEpisodeEntity[] GetProviderPriceEpisodes(long ukprn)
        {
            var parameters = new DynamicParameters();
            parameters.Add("ukprn", ukprn, DbType.Int64);
            return QueryByProc<PriceEpisodeEntity>("DataLock.GetPriceEpisodesByUkprn", parameters,
                DataLockTask.CommandTimeout);
        }
    }
}