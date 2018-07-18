using MediatR;

namespace SFA.DAS.Payments.Reference.Accounts.Application.GetPageOfAgreementsQuery
{
    public class GetPageOfAgreementsQueryRequest : IRequest<GetPageOfAgreementsQueryResponse>
    {
        public int PageNumber { get; set; }
    }
}
