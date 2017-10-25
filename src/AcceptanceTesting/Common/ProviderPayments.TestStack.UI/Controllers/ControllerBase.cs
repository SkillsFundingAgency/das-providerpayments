using System.Web.Mvc;
using NLog;

namespace ProviderPayments.TestStack.UI.Controllers
{
    public abstract class ControllerBase : Controller
    {
        internal readonly ILogger _logger;

        protected ControllerBase(ILogger logger)
        {
            _logger = logger;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            _logger.Error(filterContext.Exception);
            base.OnException(filterContext);
        }
    }
}