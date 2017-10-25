namespace SFA.DAS.Payments.Automation.WebUI.ViewModels
{
    public class LoginViewModel
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public bool IsFailedLogin { get; set; }
    }
}