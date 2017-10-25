using CS.Common.External.Interfaces;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.Payments.Reference.Accounts.Infrastructure.DependencyResolution;

namespace SFA.DAS.Payments.Reference.Accounts.IntegrationTests.StubbedInfrastructure
{
    public class IntegrationApiClientFactory : ApiClientFactory
    {
        internal override IAccountApiClient CreateClient(IExternalContext context)
        {
            return new StubbedApiClient();
        }
    }
}
