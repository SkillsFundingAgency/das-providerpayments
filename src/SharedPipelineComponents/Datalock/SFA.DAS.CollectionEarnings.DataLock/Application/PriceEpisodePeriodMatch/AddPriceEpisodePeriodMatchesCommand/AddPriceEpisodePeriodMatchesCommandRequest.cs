using MediatR;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.PriceEpisodePeriodMatch.AddPriceEpisodePeriodMatchesCommand
{
    public class AddPriceEpisodePeriodMatchesCommandRequest : IRequest
    {
         public PriceEpisodePeriodMatch[] PriceEpisodePeriodMatches { get; set; }
    }
}