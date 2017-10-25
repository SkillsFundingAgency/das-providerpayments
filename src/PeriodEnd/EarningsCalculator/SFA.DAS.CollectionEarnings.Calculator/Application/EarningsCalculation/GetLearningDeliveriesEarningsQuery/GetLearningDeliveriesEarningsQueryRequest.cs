using MediatR;
using SFA.DAS.CollectionEarnings.Calculator.Application.LearningDeliveryToProcess;

namespace SFA.DAS.CollectionEarnings.Calculator.Application.EarningsCalculation.GetLearningDeliveriesEarningsQuery
{
    public class GetLearningDeliveriesEarningsQueryRequest : IRequest<GetLearningDeliveriesEarningsQueryResponse>
    {
         public LearningDelivery[] LearningDeliveries { get; set; } 
    }
}