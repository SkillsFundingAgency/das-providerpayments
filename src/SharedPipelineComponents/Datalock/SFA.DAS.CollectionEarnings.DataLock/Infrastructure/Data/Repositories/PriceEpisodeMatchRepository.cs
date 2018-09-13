using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Repositories
{
    public class PriceEpisodeMatchRepository : DcfsRepository, IPriceEpisodeMatchRepository
    {
        private const string PriceEpisodeMatchDestination = "DataLock.PriceEpisodeMatch";

        public PriceEpisodeMatchRepository(string connectionString)
            : base(connectionString)
        {
        }

        public void AddPriceEpisodeMatches(PriceEpisodeMatchEntity[] priceEpisodeMatches)
        {
            ExecuteBatch(priceEpisodeMatches, PriceEpisodeMatchDestination);
        }
    }
}