using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

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

    }
}