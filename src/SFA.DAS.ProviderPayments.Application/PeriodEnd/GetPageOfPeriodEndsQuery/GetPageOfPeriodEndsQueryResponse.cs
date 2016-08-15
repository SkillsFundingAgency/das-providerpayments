using System.Collections.Generic;

namespace SFA.DAS.ProdiverPayments.Application.PeriodEnd.GetPageOfPeriodEndsQuery
{
    public class GetPageOfPeriodEndsQueryResponse
    {
        public int TotalNumberOfItems { get; set; }
        public int TotalNumberOfPages { get; set; }
        public IEnumerable<Domain.PeriodEnd> Items { get; set; }
    }
}
