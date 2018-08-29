using MediatR;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Payments.GetPaymentsDueForCommitmentQuery
{
    public class GetPaymentsDueForCommitmentQueryRequest : IRequest<GetPaymentsDueForCommitmentQueryResponse>
    {
        public long CommitmentId { get; set; }
        public string CommitmentVersionId { get; set; }
        public bool RefundPayments { get; set; }
    }
}