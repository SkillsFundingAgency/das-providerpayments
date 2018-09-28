using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Domain
{
    public class DatalockValidationResult
    {
        public List<DatalockValidationError> ValidationErrors { get; } = new List<DatalockValidationError>();
        public List<DatalockValidationErrorByPeriod> ValidationErrorsByPeriod { get; } = new List<DatalockValidationErrorByPeriod>();
        public List<PriceEpisodePeriodMatchEntity> PriceEpisodePeriodMatches { get; } = new List<PriceEpisodePeriodMatchEntity>();
        public List<PriceEpisodeMatchEntity> PriceEpisodeMatches { get; } = new List<PriceEpisodeMatchEntity>();
        public List<DatalockOutputEntity> DatalockOutputEntities { get; } = new List<DatalockOutputEntity>();

        public DatalockValidationResult(List<DatalockValidationError> validationErrors,
            List<DatalockValidationErrorByPeriod> validationErrorsByPeriod,
            List<PriceEpisodePeriodMatchEntity> priceEpisodePeriodMatches,
            List<PriceEpisodeMatchEntity> priceEpisodeMatches,
            List<DatalockOutputEntity> datalockOutputEntities
            )
        {
            ValidationErrors = validationErrors ?? ValidationErrors;
            ValidationErrorsByPeriod = validationErrorsByPeriod ?? ValidationErrorsByPeriod;
            PriceEpisodePeriodMatches = priceEpisodePeriodMatches ?? PriceEpisodePeriodMatches;
            PriceEpisodeMatches = priceEpisodeMatches ?? PriceEpisodeMatches;
            DatalockOutputEntities = datalockOutputEntities ?? DatalockOutputEntities;
        }
    }
}