using System;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Accounts.GetNextAccountQuery
{
    public class GetNextAccountQueryHandler : IAsyncRequestHandler<GetNextAccountQueryRequest, GetNextAccountQueryResponse>
    {
        public Task<GetNextAccountQueryResponse> Handle(GetNextAccountQueryRequest message)
        {
            throw new NotImplementedException();
        }
    }
}
