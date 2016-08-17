using System;
using System.Linq;
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
        public async Task<IHttpActionResult> Get(string periodKey, int pageNumber = 1)
        {
            try
            {
                var page = await _accountsOrchestrator.GetPageOfAccountsAffectedInPeriod(periodKey, pageNumber);
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
    }
}
