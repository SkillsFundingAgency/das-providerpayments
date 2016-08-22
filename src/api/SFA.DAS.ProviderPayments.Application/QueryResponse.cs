using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Application.Validation;

namespace SFA.DAS.ProviderPayments.Application
{
    public abstract class QueryResponse
    {
        public bool IsValid { get; set; }
        public IEnumerable<ValidationFailure> ValidationFailures { get; set; }
    }
}
