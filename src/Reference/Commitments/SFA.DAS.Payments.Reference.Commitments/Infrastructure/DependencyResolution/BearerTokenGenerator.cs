using System.Threading.Tasks;
using SFA.DAS.Events.Api.Client.Configuration;
using SFA.DAS.Http.TokenGenerators;

namespace SFA.DAS.Payments.Reference.Commitments.Infrastructure.DependencyResolution
{
    internal class BearerTokenGenerator : IGenerateBearerToken
    {
        private readonly IEventsApiClientConfiguration _configuration;

        public BearerTokenGenerator(IEventsApiClientConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Task<string> Generate()
        {
            return Task.FromResult(_configuration.ClientToken);
        }
    }
}