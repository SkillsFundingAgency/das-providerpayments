using System.Web.Mvc;
using NLog;

namespace ProviderPayments.TestStack.UI.Plumbing.Mvc
{
    public class DebugLogAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Log(filterContext, "Executing action $controller.$action for $url");
        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Log(filterContext, "Executed action $controller.$action for $url");
        }
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            Log(filterContext, "Executing result $controller.$action for $url");
        }
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            Log(filterContext, "Executed result $controller.$action for $url");
        }



        private void Log(ControllerContext context, string message)
        {
            var logger = GetControllerLogger(context);
            logger?.Debug(DetokenizeMessage(context, message));
        }

        private string DetokenizeMessage(ControllerContext context, string message)
        {
            return message
                .Replace("$controller", context.RouteData.Values["controller"].ToString())
                .Replace("$action", context.RouteData.Values["action"].ToString())
                .Replace("$url", context.HttpContext.Request.Url.AbsoluteUri);
        }
        private ILogger GetControllerLogger(ControllerContext context)
        {
            if (context.Controller is Controllers.ControllerBase)
            {
                return ((Controllers.ControllerBase) context.Controller)._logger;
            }
            return null;
        }

    }
}