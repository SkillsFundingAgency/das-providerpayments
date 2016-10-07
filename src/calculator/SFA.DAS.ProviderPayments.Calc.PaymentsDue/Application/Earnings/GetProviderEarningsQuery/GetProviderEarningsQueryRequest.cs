using MediatR;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Earnings.GetProviderEarningsQuery
{
    public class GetProviderEarningsQueryRequest : IRequest<GetProviderEarningsQueryResponse>
    {
        public long Ukprn { get; set; }
        public int Period1Month { get; set; }
        public int Period1Year { get; set; }
        public string AcademicYear { get; set; }
    }
}