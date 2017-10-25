using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;

namespace SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Repositories
{
    public class ApprenticeshipPriceEpisodePeriodisedValuesRepository : DcfsRepository, IApprenticeshipPriceEpisodePeriodisedValuesRepository
    {
        private const string Destination = "Rulebase.AEC_ApprenticeshipPriceEpisode_PeriodisedValues";

        public ApprenticeshipPriceEpisodePeriodisedValuesRepository(string connectionString)
            : base(connectionString)
        {
        }

        public void AddApprenticeshipPriceEpisodePeriodisedValues(ApprenticeshipPriceEpisodePeriodisedValuesEntity[] periodisedValues)
        {
            ExecuteBatch(periodisedValues, Destination);
        }
    }
}