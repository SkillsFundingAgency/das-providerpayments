using MediatR;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.Application.Earnings.GetProviderEarningsQuery
{
    public class GetProviderEarningsQueryRequest : IRequest<GetProviderEarningsQueryResponse>
    {
        public long Ukprn { get; set; }
    }
}