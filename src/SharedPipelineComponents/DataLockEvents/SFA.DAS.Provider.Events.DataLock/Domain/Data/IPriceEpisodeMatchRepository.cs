using SFA.DAS.Provider.Events.DataLock.Domain.Data.Entities;

namespace SFA.DAS.Provider.Events.DataLock.Domain.Data
{
    public interface IPriceEpisodeMatchRepository
    {
        PriceEpisodeMatchEntity[] GetProviderPriceEpisodeMatches(long ukprn);
    }
}