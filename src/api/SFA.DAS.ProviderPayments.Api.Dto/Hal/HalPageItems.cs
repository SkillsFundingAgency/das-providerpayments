using System.Collections.Generic;

namespace SFA.DAS.ProviderPayments.Api.Dto.Hal
{
    public class HalPageItems<T>
    {
        public IEnumerable<T> Items { get; set; }
    }
}