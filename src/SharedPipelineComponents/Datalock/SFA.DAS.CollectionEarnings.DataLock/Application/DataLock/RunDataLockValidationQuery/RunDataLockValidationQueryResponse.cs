using System.Linq;
using SFA.DAS.Payments.DCFS.Application;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.RunDataLockValidationQuery
{
    public class RunDataLockValidationQueryResponse : Response
    {
        public DatalockValidationError[] ValidationErrors { get; set; }
        public PriceEpisodeMatch.PriceEpisodeMatch[] PriceEpisodeMatches { get; set; }
        public PriceEpisodePeriodMatch.PriceEpisodePeriodMatch[] PriceEpisodePeriodMatches { get; set; }

        public bool HasAnyValidationErrors()
        {
            return ValidationErrors != null && ValidationErrors.Any();
        }

        public bool HasAnyPriceEpisodeMatches()
        {
            return PriceEpisodeMatches != null && PriceEpisodeMatches.Any();
        }

        public bool HasAnyPriceEpisodePeriodMatches()
        {
            return PriceEpisodePeriodMatches != null && PriceEpisodePeriodMatches.Any();
        }
    }
}
