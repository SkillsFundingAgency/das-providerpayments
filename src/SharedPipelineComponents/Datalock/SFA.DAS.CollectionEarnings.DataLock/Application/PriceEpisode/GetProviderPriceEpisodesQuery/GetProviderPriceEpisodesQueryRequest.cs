using MediatR;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.PriceEpisode.GetProviderPriceEpisodesQuery
{
    public class GetProviderPriceEpisodesQueryRequest : IRequest<GetProviderPriceEpisodesQueryResponse>
    {
        public long Ukprn { get; set; }
    }
}