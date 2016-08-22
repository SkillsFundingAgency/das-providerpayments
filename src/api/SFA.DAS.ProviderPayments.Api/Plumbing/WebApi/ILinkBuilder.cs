namespace SFA.DAS.ProviderPayments.Api.Plumbing.WebApi
{
    public interface ILinkBuilder
    {
        string GetPeriodEndNotificationPageLink(int pageNumber);

        string GetPeriodEndAccountsPageLink(string periodCode, int pageNumber);
        string GetAccountPaymentsLink(string periodCode, string accountId, int pageNumber);
    }
}