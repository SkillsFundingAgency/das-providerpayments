using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Application.Validation;

namespace SFA.DAS.ProviderPayments.Application.Account.GetPaymentsForAccountInPeriodQuery
{
    public class GetPaymentsForAccountInPeriodQueryResponse
    {
        public int TotalNumberOfPages { get; set; }
        public long TotalNumberOfItems { get; set; }
        public IEnumerable<Domain.Payment> Items { get; set; }

        public bool IsValid { get; set; }
        public IEnumerable<ValidationFailure> ValidationFailures { get; set; }
    }
}
