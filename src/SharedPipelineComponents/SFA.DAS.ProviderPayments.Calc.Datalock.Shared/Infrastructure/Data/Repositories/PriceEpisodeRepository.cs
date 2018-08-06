using System.Collections.Generic;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.Datalock.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Datalock.Shared.Infrastructure.Data.Repositories
{
    public interface IPriceEpisodeRepository
    {
        IEnumerable<PriceEpisodeEntity> GetProviderPriceEpisodes(long ukprn);
    }

    public class PriceEpisodeRepository : DcfsRepository, IPriceEpisodeRepository
    {
        public PriceEpisodeRepository(string connectionString)
            : base(connectionString)
        {
        }

        public IEnumerable<PriceEpisodeEntity> GetProviderPriceEpisodes(long ukprn)
        {
            const string sql = @"
                    SELECT 
	                    Ukprn, 
	                    LearnRefNumber, 
	                    Uln, 
	                    NiNumber, 
	                    AimSeqNumber, 
	                    StandardCode, 
	                    ProgrammeType, 
	                    FrameworkCode, 
	                    PathwayCode, 
	                    StartDate, 
	                    NegotiatedPrice, 
	                    PriceEpisodeIdentifier, 
	                    EndDate, 
	                    FirstAdditionalPaymentThresholdDate, 
	                    SecondAdditionalPaymentThresholdDate
                    FROM  DataLock.vw_PriceEpisode
                    WHERE Ukprn = @ukprn
                ";
            return Query<PriceEpisodeEntity>(sql, new {ukprn});
        }
    }
}