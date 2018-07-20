using MediatR;

namespace SFA.DAS.Payments.Reference.Accounts.Application.GetPageOfAccountLegalEntitiesQuery
{
    public class GetPageOfAccountLegalEntitiesQueryRequest : IRequest<GetPageOfAccountLegalEntitiesQueryResponse>
    {
        public int PageNumber { get; set; }
    }
}
