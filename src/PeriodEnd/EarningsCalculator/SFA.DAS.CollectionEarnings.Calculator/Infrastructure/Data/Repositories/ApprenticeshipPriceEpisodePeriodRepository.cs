using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;

namespace SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Repositories
{
    public class ApprenticeshipPriceEpisodePeriodRepository : DcfsRepository, IApprenticeshipPriceEpisodePeriodRepository
    {
        private const string Destination = "Rulebase.AEC_ApprenticeshipPriceEpisode_Period";

        public ApprenticeshipPriceEpisodePeriodRepository(string connectionString)
            : base(connectionString)
        {
        }

        public void AddApprenticeshipPriceEpisodePeriod(ApprenticeshipPriceEpisodePeriodEntity[] periodEarnings)
        {
            ExecuteBatch(periodEarnings, Destination);
        }
    }
}