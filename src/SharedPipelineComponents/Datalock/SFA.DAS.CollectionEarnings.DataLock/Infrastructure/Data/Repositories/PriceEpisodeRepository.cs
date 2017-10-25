using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;

namespace SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Repositories
{
    public class PriceEpisodeRepository : DcfsRepository, IPriceEpisodeRepository
    {
        private const string PriceEpisodeSource = "DataLock.vw_PriceEpisode";
        private const string PriceEpisodeColumns = "Ukprn," +
                                              "LearnRefNumber," +
                                              "Uln," +
                                              "NiNumber," +
                                              "AimSeqNumber," +
                                              "StandardCode," +
                                              "ProgrammeType," +
                                              "FrameworkCode," +
                                              "PathwayCode," +
                                              "StartDate," +
                                              "NegotiatedPrice," +
                                              "PriceEpisodeIdentifier," +
                                              "EndDate," +
                                              "FirstAdditionalPaymentThresholdDate," +
                                              "SecondAdditionalPaymentThresholdDate" ;
        private const string SelectPriceEpisodes = "SELECT " + PriceEpisodeColumns + " FROM " + PriceEpisodeSource;
        private const string SelectProviderPriceEpisodes = SelectPriceEpisodes + " WHERE Ukprn = @Ukprn";

        public PriceEpisodeRepository(string connectionString)
            : base(connectionString)
        {
        }

        public PriceEpisodeEntity[] GetProviderPriceEpisodes(long ukprn)
        {
            return Query<PriceEpisodeEntity>(SelectProviderPriceEpisodes, new { ukprn });
        }
    }
}