using System.Collections.Generic;

namespace SFA.DAS.ProviderPayments.Application
{
    public abstract class PagedQueryResponse<TItem> : QueryResponse
    {
        public int TotalNumberOfPages { get; set; }
        public long TotalNumberOfItems { get; set; }
        public IEnumerable<TItem> Items { get; set; }
    }
}
