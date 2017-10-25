using CS.Common.External.Interfaces;
using SFA.DAS.Payments.Reference.Commitments.Infrastructure.DependencyResolution;

namespace SFA.DAS.Payments.Reference.Commitments.IntegrationTests.StubbedInfrastructure
{
    internal class IntegrationApiClientFactory : ApiClientFactory
    {
        internal override Events.Api.Client.IEventsApi CreateClient(IExternalContext context)
        {
            return new StubbedEventsApi();
        }
    }
}
