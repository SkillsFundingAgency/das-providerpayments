using CS.Common.External.Interfaces;
using SFA.DAS.Http;
using SFA.DAS.Payments.Reference.Commitments.Infrastructure.Configuration;

namespace SFA.DAS.Payments.Reference.Commitments.Infrastructure.DependencyResolution
{
    internal class ApiClientFactory
    {
        protected ApiClientFactory()
        {
        }

        internal virtual Events.Api.Client.IEventsApi CreateClient(IExternalContext context)
        {
            var configuration = new EventsApiClientConfiguration(context);

            var client = new HttpClientBuilder();
            client.WithBearerAuthorisationHeader(new BearerTokenGenerator(configuration));
            return new Events.Api.Client.EventsApi(client, configuration);   
        }

        private static ApiClientFactory _instance;
        internal static ApiClientFactory Instance
        {
            get { return _instance ?? (_instance = new ApiClientFactory()); }
            set { _instance = value; }
        }

    }
}
