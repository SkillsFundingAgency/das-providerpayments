using System.Threading.Tasks;
using SFA.DAS.Payments.Automation.WebUI.Infrastructure;
using System.Web.Mvc;
using SFA.DAS.Payments.Automation.WebUI.Infrastructure.Assertions;
using SFA.DAS.Payments.Automation.WebUI.ViewModels;

namespace SFA.DAS.Payments.Automation.WebUI.Controllers
{
    [Authorize]
    public class AssertionsController : Controller
    {
        private const string DateFormat = "yyyyMMdd";
        private const string TimeFormat = "HHmmss";
        private const string DateTimeFormat = DateFormat + "T" + TimeFormat;

        private readonly IAssertionsService _service;
        public AssertionsController(IAssertionsService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(IlrSubmissionModel model)
        {
            if (!model.Specs.StartsWith("Feature:"))
            {
                model.Specs = "Feature: Default Feature for testing \n" + model.Specs;
            }

            var result = await _service.AssertPaymentResults(new IlrBuilderRequest
            {
                Gherkin = model.Specs,
                Ukprn = model.Ukprn,
                AcademicYear = model.AcademicYear,
                ShiftToMonth = model.ShiftMonth,
                ShiftToYear = model.ShiftYear
            }).ConfigureAwait(false);

            TempData["AssertionResults"] = new AssertionResults() { Results = result };

            return RedirectToAction("Index", "AssertionResults");
        }
    }
}

