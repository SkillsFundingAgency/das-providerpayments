using System.Collections.Generic;

namespace SFA.DAS.ProdiverPayments.Domain.Data.Entities
{
    public class PageOfEntities<T>
    {
        public IEnumerable<T> Items { get; set; }
        public long TotalNumberOfItems { get; set; }
        public int TotalNumberOfPages { get; set; }
    }
}
