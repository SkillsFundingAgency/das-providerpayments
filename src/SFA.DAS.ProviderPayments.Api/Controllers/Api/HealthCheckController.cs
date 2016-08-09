using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SFA.DAS.ProviderPayments.Api.Controllers.Api
{
    public class HealthCheckController : ApiController
    {

        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok();
        }
    }
}
