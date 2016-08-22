using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace SFA.DAS.ProviderPayments.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var jsonFormatters = config.Formatters.OfType<JsonMediaTypeFormatter>();
            foreach (var formatter in jsonFormatters)
            {
                formatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                formatter.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                formatter.SerializerSettings.Converters.Add(new StringEnumConverter());
            }

            // Web API routes
            config.MapHttpAttributeRoutes();

            // Convention routes
            config.Routes.MapHttpRoute(
                name: "NotificationsApi",
                routeTemplate: "api/notifications/{action}/{pageNumber}",
                defaults: new { controller = "Notifications", pageNumber = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "AccountListApi",
                routeTemplate: "api/periodends/{periodCode}/accounts/{pageNumber}",
                defaults: new { controller = "Accounts", pageNumber = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "AccountPaymentsApi",
                routeTemplate: "api/periodends/{periodCode}/accounts/{accountId}/payments/{pageNumber}",
                defaults: new { controller = "Accounts", pageNumber = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
