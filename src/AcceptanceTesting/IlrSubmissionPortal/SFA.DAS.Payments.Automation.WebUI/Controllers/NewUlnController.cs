using SFA.DAS.Payments.Automation.WebUI.Infrastructure;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.IO.Compression;
using SFA.DAS.Payments.Automation.Application.GherkinSpecs;
using SFA.DAS.Payments.Automation.WebUI.ViewModels;

namespace SFA.DAS.Payments.Automation.WebUI.Controllers
{
    [Authorize]
    public class NewUlnController : Controller
    {
        private IUlnService _service;
        public NewUlnController(IUlnService service)
        {
            _service = service;

        }
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(int? count)
        {
            var result = new NewUlnViewModel();
            if (ModelState.IsValid)
            {
                var ulnsCount = count.HasValue ? count.Value : 1;
                var data =  _service.GetNewUlns(ulnsCount);

                result.Ulns.AddRange(data);
            }
            return View(result);
        }

    }

}