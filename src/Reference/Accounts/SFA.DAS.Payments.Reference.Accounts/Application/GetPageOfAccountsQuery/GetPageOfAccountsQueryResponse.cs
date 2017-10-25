using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.Payments.DCFS.Application;

namespace SFA.DAS.Payments.Reference.Accounts.Application.GetPageOfAccountsQuery
{
    public class GetPageOfAccountsQueryResponse : QueryResponse<AccountWithBalanceViewModel>
    {
        public bool HasMorePages { get; set; }
    }
}