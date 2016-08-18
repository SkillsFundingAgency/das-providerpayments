using Newtonsoft.Json;

namespace SFA.DAS.ProviderPayments.Api.Dto.Hal
{
    public class HalPage<T>
    {
        [JsonProperty("_links")]
        public HalPageLinks Links { get; set; }

        public long Count { get; set; }

        [JsonProperty("_embedded")]
        public HalPageItems<T> Content { get; set; }
    }
}
