using MediatR;

namespace SFA.DAS.ProviderPayments.Application.Account.GetPaymentsForAccountInPeriodQuery
{
    public class GetPaymentsForAccountInPeriodQueryRequest : IAsyncRequest<GetPaymentsForAccountInPeriodQueryResponse>
    {
        public string PeriodCode { get; set; }
        public string AccountId { get; set; }
        public int PageNumber { get; set; }
    }
}
