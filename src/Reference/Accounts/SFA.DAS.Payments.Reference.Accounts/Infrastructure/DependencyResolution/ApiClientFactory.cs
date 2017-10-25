using CS.Common.External.Interfaces;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.Payments.Reference.Accounts.Context;

namespace SFA.DAS.Payments.Reference.Accounts.Infrastructure.DependencyResolution
{
    public class ApiClientFactory
    {
        protected ApiClientFactory()
        {
        }

        internal virtual IAccountApiClient CreateClient(IExternalContext context)
        {
            var configuration = new AccountApiConfiguration
            {
                ApiBaseUrl = context.Properties[KnownContextKeys.AccountsApiBaseUrl],
                ClientId = context.Properties[KnownContextKeys.AccountsApiClientId],
                ClientSecret = context.Properties[KnownContextKeys.AccountsApiClientSecret],
                IdentifierUri = context.Properties[KnownContextKeys.AccountsApiIdentifierUri],
                Tenant = context.Properties[KnownContextKeys.AccountsApiTenant]
            };
            return new AccountApiClient(configuration);
        }

        private static ApiClientFactory _instance;
        internal static ApiClientFactory Instance
        {
            get { return _instance ?? (_instance = new ApiClientFactory()); }
            set { _instance = value; }
        }

    }
}
