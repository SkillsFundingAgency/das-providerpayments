using MediatR;

namespace SFA.DAS.ProdiverPayments.Application.PeriodEnd.GetPageOfPeriodEndsQuery
{
    public class GetPageOfPeriodEndsQueryRequest : IAsyncRequest<GetPageOfPeriodEndsQueryResponse>
    {
        public int PageNumber { get; set; }
    }
}
