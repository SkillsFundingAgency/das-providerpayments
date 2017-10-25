using SFA.DAS.Payments.Automation.WebUI.ViewModels;
using System.Web.Mvc;

namespace SFA.DAS.Payments.Automation.WebUI.Controllers
{
    public class AssertionResultsController : Controller
    {
        // GET: AssertionResults
        public ActionResult Index()
        {
            var result = (AssertionResults) TempData["AssertionResults"];
            return View(result);
        }
    }
}