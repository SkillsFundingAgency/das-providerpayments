using System.Web.Http.Routing;

namespace SFA.DAS.ProviderPayments.Api.Plumbing.WebApi
{
    public class LinkBuilder : ILinkBuilder
    {
        private const string NotificationRouteName = "NotificationsApi";
        private const string AccountListRouteName = "AccountListApi";
        private const string AccountPaymentRouteName = "AccountPaymentsApi";

        private readonly UrlHelper _urlHelper;

        public LinkBuilder(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }


        public string GetPeriodEndNotificationPageLink(int pageNumber)
        {
            return _urlHelper.Link(NotificationRouteName, new { pageNumber });
        }

        public string GetPeriodEndAccountsPageLink(int pageNumber)
        {
            return _urlHelper.Link(AccountListRouteName, new { pageNumber });
        }
        public string GetAccountPaymentsLink(string periodCode, string accountId)
        {
            return _urlHelper.Link(AccountPaymentRouteName, new { periodCode, accountId });
        }
    }
}