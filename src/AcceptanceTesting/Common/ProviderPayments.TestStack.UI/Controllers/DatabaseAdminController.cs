using System.Threading.Tasks;
using System.Web.Mvc;
using ProviderPayments.TestStack.Application;

namespace ProviderPayments.TestStack.UI.Controllers
{
    public class DatabaseAdminController : Controller
    {
        private readonly IDatabaseService _databaseService;

        public DatabaseAdminController(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [HttpGet]
        public ActionResult Clean()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmClean()
        {
            await _databaseService.CleanDeds();
            return View();
        }
    }
}