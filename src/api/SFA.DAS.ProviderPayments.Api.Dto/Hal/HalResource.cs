using Newtonsoft.Json;

namespace SFA.DAS.ProviderPayments.Api.Dto.Hal
{
    public abstract class HalResource<TLinks> where TLinks : HalLinks
    {
        [JsonProperty("_links")]
        public TLinks Links { get; set; }
    }
}
