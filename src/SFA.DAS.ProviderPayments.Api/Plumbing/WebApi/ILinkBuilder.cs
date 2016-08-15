namespace SFA.DAS.ProviderPayments.Api.Plumbing.WebApi
{
    public interface ILinkBuilder
    {
        string GetPeriodEndNotificationPageLink(int pageNumber);
    }
}