using System.Linq;
using SFA.DAS.Payments.DCFS.Application;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.RunDataLockValidationQuery
{
    public class RunDataLockValidationQueryResponse : Response
    {
        public ValidationError.ValidationError[] ValidationErrors { get; set; }
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
