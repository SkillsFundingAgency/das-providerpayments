using MediatR;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Earnings.GetEarningForCommitmentQuery
{
    public class GetEarningForCommitmentQueryRequest : IRequest<GetEarningForCommitmentQueryResponse>
    {
        public string CommitmentId { get; set; }
    }
}
