using MediatR;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Earnings.GetEarningForCommitmentQuery
{
    public class GetEarningForCommitmentQueryRequest : IRequest<GetEarningForCommitmentQueryResponse>
    {
        public string CommitmentId { get; set; }
    }
}
