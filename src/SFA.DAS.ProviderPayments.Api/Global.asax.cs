using System.Web.Http;
using SFA.DAS.ProviderPayments.Infrastructure.Logging;

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
