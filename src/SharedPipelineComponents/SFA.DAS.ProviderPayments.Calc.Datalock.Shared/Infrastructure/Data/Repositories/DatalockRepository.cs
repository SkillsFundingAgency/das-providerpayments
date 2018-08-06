using System.Collections.Generic;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.Datalock.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Datalock.Shared.Infrastructure.Data.Repositories
{
    public interface IPriceEpisodePeriodMatchRepository
    {
        void AddPriceEpisodePeriodMatches(IEnumerable<PriceEpisodePeriodMatchEntity> priceEpisodePeriodMatches);
    }

    public interface IPriceEpisodeMatchRepository
    {
        void AddPriceEpisodeMatches(IEnumerable<PriceEpisodeMatchEntity> priceEpisodeMatches);
    }

    public interface IValidationErrorRepository
    {
        void AddValidationErrors(IEnumerable<ValidationErrorEntity> validationErrors);
    }

    public class DatalockRepository : DcfsRepository, 
        IPriceEpisodePeriodMatchRepository, IPriceEpisodeMatchRepository, IValidationErrorRepository
    {
        public DatalockRepository(string connectionString) : base(connectionString)
        {}

        private const string PriceEpisodePeriodMatchDestination = "DataLock.PriceEpisodePeriodMatch";
        private const string PriceEpisodeMatchDestination = "DataLock.PriceEpisodeMatch";
        private const string ValidationErrorDestination = "DataLock.ValidationError";

        public void AddPriceEpisodePeriodMatches(IEnumerable<PriceEpisodePeriodMatchEntity> priceEpisodePeriodMatches)
        {
            ExecuteBatch(priceEpisodePeriodMatches, PriceEpisodePeriodMatchDestination);
        }

        public void AddPriceEpisodeMatches(IEnumerable<PriceEpisodeMatchEntity> priceEpisodeMatches)
        {
            ExecuteBatch(priceEpisodeMatches, PriceEpisodeMatchDestination);
        }

        public void AddValidationErrors(IEnumerable<ValidationErrorEntity> validationErrors)
        {
            ExecuteBatch(validationErrors, ValidationErrorDestination);
        }
    }
}
