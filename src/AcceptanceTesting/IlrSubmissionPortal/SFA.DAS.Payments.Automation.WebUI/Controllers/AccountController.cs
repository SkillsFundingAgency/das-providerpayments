using System.Web.Mvc;
using System.Web.Security;
using SFA.DAS.Payments.Automation.WebUI.Infrastructure;
using SFA.DAS.Payments.Automation.WebUI.ViewModels;

namespace SFA.DAS.Payments.Automation.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public AccountController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!_authenticationService.AuthenticateUser(model.EmailAddress, model.Password))
            {
                model.IsFailedLogin = true;
                return View(model);
            }

            FormsAuthentication.SetAuthCookie(model.EmailAddress, false);
            if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "IlrBuilder");
        }
    }
}