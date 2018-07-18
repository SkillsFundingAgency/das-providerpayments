using System;
using MediatR;
using SFA.DAS.EAS.Account.Api.Client;

namespace SFA.DAS.Payments.Reference.Accounts.Application.GetPageOfAgreementsQuery
{
    public class GetPageOfAgreementsQueryHandler : IRequestHandler<GetPageOfAgreementsQueryRequest, GetPageOfAgreementsQueryResponse>
    {
        public GetPageOfAgreementsQueryResponse Handle(GetPageOfAgreementsQueryRequest message)
        {
            throw new NotImplementedException();
        }
    }
}