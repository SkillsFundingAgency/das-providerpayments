using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.Payments.DCFS.Application;

namespace SFA.DAS.Payments.Reference.Accounts.Application.GetPageOfAccountLegalEntitiesQuery
{
    public class GetPageOfAccountLegalEntitiesQueryResponse : QueryResponse<AccountLegalEntityViewModel>
    {
        public bool HasMorePages { get; set; }
    }
}