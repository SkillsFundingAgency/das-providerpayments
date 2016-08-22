using System;
using System.Threading.Tasks;
using System.Web.Http;
using SFA.DAS.ProviderPayments.Api.Orchestrators.OrchestratorExceptions;

namespace SFA.DAS.ProviderPayments.Api.Controllers.Api
{
    public abstract class ApiControllerBase : ApiController
    {
        protected async Task<IHttpActionResult> ProcessRequest(Func<Task<IHttpActionResult>> action)
        {
            try
            {
                return await action.Invoke();
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
