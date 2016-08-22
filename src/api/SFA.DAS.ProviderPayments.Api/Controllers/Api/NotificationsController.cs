using System.Threading.Tasks;
using System.Web.Http;
using SFA.DAS.ProviderPayments.Api.Orchestrators;

namespace SFA.DAS.ProviderPayments.Api.Controllers.Api
{
    public class NotificationsController : ApiControllerBase
    {
        private readonly NotificationsOrchestrator _notificationsOrchestrator;

        public NotificationsController(NotificationsOrchestrator notificationsOrchestrator)
        {
            _notificationsOrchestrator = notificationsOrchestrator;
        }

        [HttpGet]
        public async Task<IHttpActionResult> PeriodEnd(int pageNumber = 1)
        {
            return await ProcessRequest(async () =>
            {
                var pageOfResults = await _notificationsOrchestrator.GetPageOfPeriodEndNotifications(pageNumber);
                return Ok(pageOfResults);
            });
        }
    }
}
