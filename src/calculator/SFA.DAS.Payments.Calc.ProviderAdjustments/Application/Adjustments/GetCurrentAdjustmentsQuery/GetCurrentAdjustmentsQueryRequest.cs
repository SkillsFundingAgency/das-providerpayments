using MediatR;

namespace SFA.DAS.Payments.Calc.ProviderAdjustments.Application.Adjustments.GetCurrentAdjustmentsQuery
{
    public class GetCurrentAdjustmentsQueryRequest : IRequest<GetCurrentAdjustmentsQueryResponse>
    {
        public long Ukprn { get; set; }
    }
}