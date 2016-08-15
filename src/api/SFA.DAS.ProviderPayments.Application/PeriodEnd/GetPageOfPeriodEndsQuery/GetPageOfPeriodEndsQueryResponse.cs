using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Application.Validation;

namespace SFA.DAS.ProviderPayments.Application.PeriodEnd.GetPageOfPeriodEndsQuery
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
