using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;
using SFA.DAS.Payments.Automation.WebUI.DependencyResolution;
using StructureMap;

namespace SFA.DAS.Payments.Automation.WebUI
{
    public class Global : HttpApplication
    {
        public static StructureMapDependencyScope StructureMapDependencyScope { get; set; }

        public static void End()
        {
            StructureMapDependencyScope.Dispose();
        }

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            IContainer container = IoC.Initialize();
            StructureMapDependencyScope = new StructureMapDependencyScope(container);
            //DependencyResolver.SetResolver(StructureMapDependencyScope);
            //DynamicModuleUtility.RegisterModule(typeof(StructureMapScopeModule));

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);            
        }
    }
}