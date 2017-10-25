namespace SFA.DAS.Payments.Automation.WebUI.Infrastructure
{
    public interface IAuthenticationService
    {
        bool AuthenticateUser(string emailAddress, string password);
    }
}