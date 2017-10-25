using MediatR;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Earnings.GetProviderEarningsQuery
{
    public class GetProviderEarningsQueryRequest : IRequest<GetProviderEarningsQueryResponse>
    {
        public long Ukprn { get; set; }
        public string AcademicYear { get; set; }
    }
}