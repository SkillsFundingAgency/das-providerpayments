using System.Collections.Generic;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Services
{
    interface IValidateDatalocks
    {
        
    }

    class DatalockValidationService : IValidateDatalocks
    {
    }

    class DatalockValidationResult
    {
        public IEnumerable<ValidationErrorEntity> ValidationErrors { get; set; }
        public IEnumerable<PriceEpisodePeriodMatchEntity> PriceEpisodePeriodMatches { get; set; }
        public IEnumerable<PriceEpisodeMatchEntity> PriceEpisodeMatches { get; set; }
        public IEnumerable<DatalockOutputEntity> DatalockOutputEntities { get; set; }
    }
}
