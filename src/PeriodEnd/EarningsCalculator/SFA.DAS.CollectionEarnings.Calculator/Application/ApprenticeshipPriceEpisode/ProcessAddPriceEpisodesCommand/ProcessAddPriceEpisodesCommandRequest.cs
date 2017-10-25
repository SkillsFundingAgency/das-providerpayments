using MediatR;

namespace SFA.DAS.CollectionEarnings.Calculator.Application.ApprenticeshipPriceEpisode.ProcessAddPriceEpisodesCommand
{
    public class ProcessAddPriceEpisodesCommandRequest : IRequest
    {
         public ApprenticeshipPriceEpisode[] PriceEpisodes { get; set; }
    }
}