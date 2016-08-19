namespace SFA.DAS.ProviderPayments.Api.Plumbing.WebApi
{
    public interface ILinkBuilder
    {
        string GetPeriodEndNotificationPageLink(int pageNumber);

        string GetPeriodEndAccountsPageLink(int pageNumber);
        string GetAccountPaymentsLink(string periodCode, string accountId, int pageNumber);
    }
}