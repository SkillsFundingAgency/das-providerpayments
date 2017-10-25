using System;
using MediatR;

namespace SFA.DAS.Payments.Reference.Accounts.Application.GetPageOfAccountsQuery
{
    public class GetPageOfAccountsQueryRequest : IRequest<GetPageOfAccountsQueryResponse>
    {
        public int PageNumber { get; set; }
        public DateTime CorrelationDate { get; set; }
    }
}
