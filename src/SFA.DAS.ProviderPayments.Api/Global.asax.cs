using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using SFA.DAS.ProdiverPayments.Infrastructure.Logging;

namespace SFA.DAS.ProviderPayments.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            LoggingConfig.ConfigureLogging();

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
