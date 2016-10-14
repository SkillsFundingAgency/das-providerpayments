using MediatR;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments.GetPaymentHistoryQuery
{
    public class GetPaymentHistoryQueryRequest : IRequest<GetPaymentHistoryQueryResponse>
    {
        public long Ukprn { get; set; }
        public long CommitmentId { get; set; }
    }
}
