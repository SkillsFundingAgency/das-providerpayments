using System.Web.Http.Routing;

namespace SFA.DAS.ProviderPayments.Api.Plumbing.WebApi
{
    public class LinkBuilder : ILinkBuilder
    {
        private const string NotificationRouteName = "NotificationsApi";

        private readonly UrlHelper _urlHelper;

        public LinkBuilder(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public string GetPeriodEndNotificationPageLink(int pageNumber)
        {
            return _urlHelper.Link(NotificationRouteName, new { pageNumber });
        }
    }
}