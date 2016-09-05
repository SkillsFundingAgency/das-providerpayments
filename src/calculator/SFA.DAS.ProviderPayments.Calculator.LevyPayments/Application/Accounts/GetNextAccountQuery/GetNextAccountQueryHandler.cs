using System;
using MediatR;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Accounts.GetNextAccountQuery
{
    public class GetNextAccountQueryHandler : IRequestHandler<GetNextAccountQueryRequest, GetNextAccountQueryResponse>
    {
        public GetNextAccountQueryResponse Handle(GetNextAccountQueryRequest message)
        {
            throw new NotImplementedException();
        }
    }
}
