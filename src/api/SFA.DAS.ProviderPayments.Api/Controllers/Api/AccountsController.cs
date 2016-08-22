using System.Threading.Tasks;
using System.Web.Http;
using SFA.DAS.ProviderPayments.Api.Orchestrators;

namespace SFA.DAS.ProviderPayments.Api.Controllers.Api
{
    public class AccountsController : ApiControllerBase
    {
        private readonly AccountsOrchestrator _accountsOrchestrator;

        public AccountsController(AccountsOrchestrator accountsOrchestrator)
        {
            _accountsOrchestrator = accountsOrchestrator;
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get(string periodCode, int pageNumber = 1)
        {
            return await ProcessRequest(async () =>
            {
                var page = await _accountsOrchestrator.GetPageOfAccountsAffectedInPeriod(periodCode, pageNumber);
                return Ok(page);
            });
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetPayments(string periodCode, string accountId, int pageNumber = 1)
        {
            return await ProcessRequest(async () =>
            {
                var page = await _accountsOrchestrator.GetPageOfPaymentsForAccountInPeriod(periodCode, accountId, pageNumber);
                return Ok(page);
            });
        }
    }
}
