using System.Web.Mvc;
using ProviderPayments.TestStack.UI.Plumbing.Mvc;

namespace ProviderPayments.TestStack.UI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new DebugLogAttribute());
        }
    }
}