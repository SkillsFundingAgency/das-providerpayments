using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data
{
    public interface IPriceEpisodeRepository
    {
        PriceEpisodeEntity[] GetProviderPriceEpisodes(long ukprn);
    }
}