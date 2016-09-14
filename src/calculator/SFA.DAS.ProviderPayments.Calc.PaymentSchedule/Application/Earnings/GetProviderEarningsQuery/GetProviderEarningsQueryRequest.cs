using MediatR;

namespace SFA.DAS.ProviderPayments.Calc.PaymentSchedule.Application.Earnings.GetProviderEarningsQuery
{
    public class GetProviderEarningsQueryRequest : IRequest<GetProviderEarningsQueryResponse>
    {
        public long Ukprn { get; set; }
    }
}