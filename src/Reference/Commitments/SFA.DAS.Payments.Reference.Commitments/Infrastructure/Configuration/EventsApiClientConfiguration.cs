using CS.Common.External.Interfaces;
using SFA.DAS.Events.Api.Client.Configuration;

namespace SFA.DAS.Payments.Reference.Commitments.Infrastructure.Configuration
{
    public class EventsApiClientConfiguration : IEventsApiClientConfiguration
    {

        public EventsApiClientConfiguration(IExternalContext context)
        {
            BaseUrl = context.Properties[ImportCommitmentsContextKeys.BaseUrl];
            ClientToken = context.Properties[ImportCommitmentsContextKeys.ClientToken];
        }

        public string BaseUrl { get; set; }
        public string ClientToken { get; set; }
    }
}
