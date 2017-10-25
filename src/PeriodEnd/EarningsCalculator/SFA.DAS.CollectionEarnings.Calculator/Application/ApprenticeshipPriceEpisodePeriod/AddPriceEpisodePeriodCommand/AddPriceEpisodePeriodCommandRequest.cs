using MediatR;

namespace SFA.DAS.CollectionEarnings.Calculator.Application.ApprenticeshipPriceEpisodePeriod.AddPriceEpisodePeriodCommand
{
    public class AddPriceEpisodePeriodCommandRequest : IRequest
    {
         public ApprenticeshipPriceEpisodePeriod[] PeriodEarnings { get; set; }
    }
}