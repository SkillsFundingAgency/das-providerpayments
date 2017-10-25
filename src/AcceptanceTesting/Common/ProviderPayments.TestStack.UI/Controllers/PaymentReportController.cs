using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using NLog;
using ProviderPayments.TestStack.Application;
using ProviderPayments.TestStack.Domain.Mapping;
using ProviderPayments.TestStack.UI.Models;

namespace ProviderPayments.TestStack.UI.Controllers
{
    public class PaymentReportController : ControllerBase
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly IPaymentReportService _paymentReportService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public PaymentReportController(IPaymentReportService paymentReportService,
                                       IAccountService accountService,
                                       IMapper mapper)
            : base(Logger)
        {
            _paymentReportService = paymentReportService;
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task<ActionResult> Index()
        {
            var payments = await _paymentReportService.GetAllReportPayments();
            var accounts = await _accountService.GetAllAccounts();

            var model = new PaymentsReportModel
            {
                Payments = payments.ToArray(),
                Accounts = accounts.ToArray()
            };

            return View(model);
        }
    }
}