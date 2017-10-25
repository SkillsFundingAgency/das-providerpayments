using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using NLog;
using ProviderPayments.TestStack.Application;
using ProviderPayments.TestStack.Domain;
using ProviderPayments.TestStack.Domain.Mapping;
using ProviderPayments.TestStack.UI.Models;

namespace ProviderPayments.TestStack.UI.Controllers
{
    public class AccountController : ControllerBase
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private static readonly Random Random = new Random();

        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AccountController(IAccountService accountService,
                                 IMapper mapper)
            : base(Logger)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task<ActionResult> Index()
        {
            var accounts = await _accountService.GetAllAccounts();

            return View(accounts);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new AccountModel
            {
                Id = Random.Next(1, int.MaxValue),
                Balance = 0.00m
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AccountModel model)
        {
            try
            {
                _logger.Info("Account creation requested");

                var account = _mapper.Map<Account>(model);

                _logger.Info($"Attempting to create account Id={account.Id}, Name={account.Name}, Balance={account.Balance}");
                await _accountService.CreateAccount(account);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(long id)
        {
            var account = await _accountService.GetAccountById(id);
            if (account == null)
            {
                return HttpNotFound();
            }

            var model = _mapper.Map<AccountModel>(account);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(long id, AccountModel model)
        {
            try
            {
                _logger.Info($"Update account {id} requested");

                var account = _mapper.Map<Account>(model);
                account.Id = id;

                _logger.Info($"Attempting to update account {id} Name={account.Name}, Balance={account.Balance}");
                await _accountService.UpdateAccount(account);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(long accountId)
        {
            try
            {
                _logger.Info($"Delete account {accountId} requested");

                await _accountService.DeleteAccount(accountId);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500, ex.Message);
            }
        }

        private long CreateNuid()
        {
            var rdm = new Random();
            return rdm.Next(10000000, 99999999);
        }

    }
}