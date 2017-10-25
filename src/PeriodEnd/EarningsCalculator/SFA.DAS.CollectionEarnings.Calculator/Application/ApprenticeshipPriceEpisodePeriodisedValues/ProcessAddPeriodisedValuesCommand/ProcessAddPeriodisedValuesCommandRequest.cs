using MediatR;

namespace SFA.DAS.CollectionEarnings.Calculator.Application.ApprenticeshipPriceEpisodePeriodisedValues.ProcessAddPeriodisedValuesCommand
{
    public class ProcessAddPeriodisedValuesCommandRequest : IRequest
    {
         public ApprenticeshipPriceEpisodePeriodisedValues[] PeriodisedValues { get; set; }
    }
}