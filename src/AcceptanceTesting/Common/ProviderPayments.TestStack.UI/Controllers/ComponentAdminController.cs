using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using NLog;
using ProviderPayments.TestStack.Application;
using ProviderPayments.TestStack.UI.Models;

namespace ProviderPayments.TestStack.UI.Controllers
{
    public class ComponentAdminController : ControllerBase
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly IComponentService _componentService;
        private readonly IProcessService _processService;

        public ComponentAdminController(IComponentService componentService, IProcessService processService)
            : base(Logger)
        {
            _componentService = componentService;
            _processService = processService;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var components = await _componentService.GetComponents();
            return View(new ComponentAdminModel
            {
                Components = components.ToArray()
            });
        }

        [HttpPost]
        public async Task<ActionResult> Index(HttpPostedFileBase componentFile)
        {
            var zip = await componentFile.InputStream.ReadAllBytesAsync();

            await _componentService.UpdateComponent(zip);

            return await Index();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RebuildDedsTables(int componentType)
        {
            var id = await _processService.RebuildDedsDatabase((Domain.ComponentType)componentType);

            return RedirectToAction("SubmissionStatus", "Process", new { id });
        }
    }
}