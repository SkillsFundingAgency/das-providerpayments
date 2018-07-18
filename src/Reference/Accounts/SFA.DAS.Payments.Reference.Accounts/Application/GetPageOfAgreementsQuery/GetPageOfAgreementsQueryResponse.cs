using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.Payments.DCFS.Application;

namespace SFA.DAS.Payments.Reference.Accounts.Application.GetPageOfAgreementsQuery
{
    public class GetPageOfAgreementsQueryResponse : QueryResponse<AccountLegalEntityViewModel>
    {
        public bool HasMorePages { get; set; }
    }
}