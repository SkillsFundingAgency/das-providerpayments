using System.Web.Http;

namespace SFA.DAS.ProviderPayments.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            // Convention routes
            config.Routes.MapHttpRoute(
                name: "NotificationsApi",
                routeTemplate: "api/notifications/{action}/{pageNumber}",
                defaults: new { controller = "Notifications", pageNumber = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
