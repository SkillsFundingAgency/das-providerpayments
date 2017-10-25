using MediatR;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.PriceEpisodeMatch.AddPriceEpisodeMatchesCommand
{
    public class AddPriceEpisodeMatchesCommandRequest : IRequest
    {
         public PriceEpisodeMatch[] PriceEpisodeMatches { get; set; }
    }
}