using MediatR;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments.GetPaymentHistoryQuery
{
    public class GetPaymentHistoryQueryRequest : IRequest<GetPaymentHistoryQueryResponse>
    {
        public long Ukprn { get; set; }
        public long Uln { get; set; }
        public long? StandardCode { get; set; }
        public int? ProgrammeType { get; set; }
        public int? FrameworkCode { get; set; }
        public int? PathwayCode { get; set; }
    }
}
