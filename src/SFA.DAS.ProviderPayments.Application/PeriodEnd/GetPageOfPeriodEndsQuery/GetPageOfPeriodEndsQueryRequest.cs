using MediatR;

namespace SFA.DAS.ProviderPayments.Application.PeriodEnd.GetPageOfPeriodEndsQuery
{
    public class GetPageOfPeriodEndsQueryRequest : IAsyncRequest<GetPageOfPeriodEndsQueryResponse>
    {
        public int PageNumber { get; set; }
    }
}
