using System;
using MediatR;

namespace SFA.DAS.Payments.Reference.Accounts.Application.GetPageOfAccountLegalEntitiesQuery
{
    public class GetPageOfAccountLegalEntitiesQueryHandler : 
        IRequestHandler<GetPageOfAccountLegalEntitiesQueryRequest, GetPageOfAccountLegalEntitiesQueryResponse>
    {
        public GetPageOfAccountLegalEntitiesQueryResponse Handle(GetPageOfAccountLegalEntitiesQueryRequest message)
        {
            throw new NotImplementedException();
        }
    }
}