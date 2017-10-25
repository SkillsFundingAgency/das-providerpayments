using System.Web.Mvc;
using NLog;

namespace ProviderPayments.TestStack.UI.Controllers
{
    public class HomeController : ControllerBase
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public HomeController()
            : base(Logger)
        {
            
        }
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}