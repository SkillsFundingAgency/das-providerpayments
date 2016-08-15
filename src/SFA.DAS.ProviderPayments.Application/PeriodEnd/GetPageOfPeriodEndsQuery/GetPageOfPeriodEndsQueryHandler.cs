using System;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.ProdiverPayments.Application.PeriodEnd.GetPageOfPeriodEndsQuery
{
    public class GetPageOfPeriodEndsQueryHandler : IAsyncRequestHandler<GetPageOfPeriodEndsQueryRequest, GetPageOfPeriodEndsQueryResponse>
    {
        public Task<GetPageOfPeriodEndsQueryResponse> Handle(GetPageOfPeriodEndsQueryRequest message)
        {
            throw new NotImplementedException();
        }
    }
}
