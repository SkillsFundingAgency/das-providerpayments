using System.Collections.Generic;
using SFA.DAS.ProdiverPayments.Application.Validation;

namespace SFA.DAS.ProdiverPayments.Application.PeriodEnd.GetPageOfPeriodEndsQuery
{
    public class GetPageOfPeriodEndsQueryResponse
    {
        public long TotalNumberOfItems { get; set; }
        public int TotalNumberOfPages { get; set; }
        public IEnumerable<Domain.PeriodEnd> Items { get; set; }

        public bool IsValid { get; set; }
        public IEnumerable<ValidationFailure> ValidationFailures { get; set; }
    }
}
