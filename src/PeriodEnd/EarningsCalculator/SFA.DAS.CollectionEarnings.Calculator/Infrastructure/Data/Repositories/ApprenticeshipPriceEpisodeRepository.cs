using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;

namespace SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Repositories
{
    public class ApprenticeshipPriceEpisodeRepository : DcfsRepository, IApprenticeshipPriceEpisodeRepository
    {
        private const string Destination = "Rulebase.AEC_ApprenticeshipPriceEpisode";

        public ApprenticeshipPriceEpisodeRepository(string connectionString)
            : base(connectionString)
        {
        }

        public void AddApprenticeshipPriceEpisodes(ApprenticeshipPriceEpisodeEntity[] entities)
        {
            ExecuteBatch(entities, Destination);
        }
    }
}