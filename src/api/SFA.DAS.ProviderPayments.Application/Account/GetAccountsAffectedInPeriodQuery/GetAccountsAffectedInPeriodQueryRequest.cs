using MediatR;

namespace SFA.DAS.ProviderPayments.Application.Account.GetAccountsAffectedInPeriodQuery
{
    public class GetAccountsAffectedInPeriodQueryRequest : IAsyncRequest<GetAccountsAffectedInPeriodQueryResponse>
    {
        public string PeriodCode { get; set; }
        public int PageNumber { get; set; }
    }
}
