using System;
using MediatR;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Earnings.GetEarningForCommitmentQuery
{
    public class GetEarningForCommitmentQueryHandler : IRequestHandler<GetEarningForCommitmentQueryRequest, GetEarningForCommitmentQueryResponse>
    {
        public GetEarningForCommitmentQueryResponse Handle(GetEarningForCommitmentQueryRequest message)
        {
            throw new NotImplementedException();
        }
    }
}