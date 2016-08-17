using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Application.Validation;

namespace SFA.DAS.ProviderPayments.Application.Account.GetAccountsAffectedInPeriodQuery
{
    public class GetAccountsAffectedInPeriodQueryResponse
    {
        public int TotalNumberOfPages { get; set; }
        public int TotalNumberOfItems { get; set; }
        public IEnumerable<Domain.Account> Items { get; set; }

        public bool IsValid { get; set; }
        public IEnumerable<ValidationFailure> ValidationFailures { get; set; }
    }
}
