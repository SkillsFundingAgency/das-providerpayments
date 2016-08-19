using System;
using System.Threading.Tasks;
using System.Web.Http;
using SFA.DAS.ProviderPayments.Api.Orchestrators;
using SFA.DAS.ProviderPayments.Api.Orchestrators.OrchestratorExceptions;

namespace SFA.DAS.ProviderPayments.Api.Controllers.Api
{
    public class NotificationsController : ApiController
    {
        private readonly NotificationsOrchestrator _notificationsOrchestrator;

        public NotificationsController(NotificationsOrchestrator notificationsOrchestrator)
        {
            _notificationsOrchestrator = notificationsOrchestrator;
        }

        [HttpGet]
        public async Task<IHttpActionResult> PeriodEnd(int pageNumber = 1)
        {
            try
            {
                var pageOfResults = await _notificationsOrchestrator.GetPageOfPeriodEndNotifications(pageNumber);
                return Ok(pageOfResults);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException)
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
