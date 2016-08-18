using System;
using System.Threading.Tasks;
using System.Web.Http;
using SFA.DAS.ProviderPayments.Api.Orchestrators;
using SFA.DAS.ProviderPayments.Api.Orchestrators.OrchestratorExceptions;

namespace SFA.DAS.ProviderPayments.Api.Controllers.Api
{
    public class AccountsController : ApiController
    {
        private readonly AccountsOrchestrator _accountsOrchestrator;

        public AccountsController(AccountsOrchestrator accountsOrchestrator)
        {
            _accountsOrchestrator = accountsOrchestrator;
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get(string periodCode, int pageNumber = 1)
        {
            try
            {
                var page = await _accountsOrchestrator.GetPageOfAccountsAffectedInPeriod(periodCode, pageNumber);
                return Ok(page);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex) when (ex is PageNotFoundException || ex is PeriodNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetPayments(string periodCode, string accountId, int pageNumber = 1)
        {
            var temp = await Task.FromResult<object>(null);

            return null;
        }
    }
}
