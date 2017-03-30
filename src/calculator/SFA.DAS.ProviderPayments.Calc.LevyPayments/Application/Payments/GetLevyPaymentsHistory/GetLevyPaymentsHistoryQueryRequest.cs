using MediatR;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Payments.GetLevyPaymentsHistoryQuery
{
    public class GetLevyPaymentsHistoryQueryRequest : IRequest<GetLevyPaymentsHistoryQueryResponse>
    {
        public int DeliveryMonth { get; set; }
        public int DeliveryYear { get; set; }
        public int TransactionType { get; set; }
        public long CommitmentId { get; set; }
    }
}