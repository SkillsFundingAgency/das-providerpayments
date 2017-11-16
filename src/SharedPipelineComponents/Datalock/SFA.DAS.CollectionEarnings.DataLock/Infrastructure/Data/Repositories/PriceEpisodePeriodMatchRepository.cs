using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;

namespace SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Repositories
{
    public class PriceEpisodePeriodMatchRepository : DcfsRepository, IPriceEpisodePeriodMatchRepository
    {
        private const string PriceEpisodePeriodMatchDestination = "DataLock.PriceEpisodePeriodMatch";

        private const string DeleteExtraPriceEpisodePeriodMatchesProc = "DataLock.DeleteExtraPriceEpisodeperiodMatches";

        public PriceEpisodePeriodMatchRepository(string connectionString)
            : base(connectionString)
        {
        }

        public void AddPriceEpisodePeriodMatches(PriceEpisodePeriodMatchEntity[] priceEpisodePeriodMatches)
        {
            ExecuteBatch(priceEpisodePeriodMatches, PriceEpisodePeriodMatchDestination);
        }

        public void RemoveExtraPriceEpisodePeriodMatches()
        {
            ExecuteByProc(DeleteExtraPriceEpisodePeriodMatchesProc);
        }
    }
}