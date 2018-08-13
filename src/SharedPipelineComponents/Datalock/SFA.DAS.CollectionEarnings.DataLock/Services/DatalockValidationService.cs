using System.Collections.Generic;
using SFA.DAS.CollectionEarnings.DataLock.Application.DasAccount;
using SFA.DAS.CollectionEarnings.DataLock.Domain;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Services
{
    public interface IValidateDatalocks
    {
        DatalockValidationResult Validate(ProviderCommitments providerCommitments, List<RawEarning> priceEpisodes, IEnumerable<DasAccount> accounts);
    }

    public class DatalockValidationService : IValidateDatalocks
    {
        public DatalockValidationResult Validate(
            ProviderCommitments providerCommitments, 
            List<RawEarning> priceEpisodes, 
            IEnumerable<DasAccount> accounts)
        {
            foreach (var uln in providerCommitments.AllUlns())
            {
                var learner = providerCommitments.CommitmentsForLearner(uln);
            }

            return new DatalockValidationResult();
        }
    }

    public class DatalockValidationResult
    {
        public IEnumerable<DatalockValidationError> ValidationErrors { get; set; }
        public IEnumerable<PriceEpisodePeriodMatchEntity> PriceEpisodePeriodMatches { get; set; }
        public IEnumerable<PriceEpisodeMatchEntity> PriceEpisodeMatches { get; set; }
        public IEnumerable<DatalockOutputEntity> DatalockOutputEntities { get; set; }
    }
}
