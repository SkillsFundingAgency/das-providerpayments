using MediatR;

namespace SFA.DAS.Payments.Calc.ProviderAdjustments.Application.Adjustments.GetPreviousAdjustmentsQuery
{
    public class GetPreviousAdjustmentsQueryRequest : IRequest<GetPreviousAdjustmentsQueryResponse>
    {
        public long Ukprn { get; set; }
    }
}