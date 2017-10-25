using SFA.DAS.Payments.Automation.WebUI.Infrastructure;
using SFA.DAS.Payments.Automation.WebUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SFA.DAS.Payments.Automation.WebUI.Controllers
{
    public class UlnsListController : Controller
    {
        private IUlnService _service;
        public UlnsListController(IUlnService service)
        {
            _service = service;

        }
        // GET: UlsList
        public ViewResult Index()
        {
            var items = _service.GetAllUsedUlns();
            var result = new UsedUlnViewModel
            {
                UsedUlns = items
            };

            return View(result);
        }
    }
}