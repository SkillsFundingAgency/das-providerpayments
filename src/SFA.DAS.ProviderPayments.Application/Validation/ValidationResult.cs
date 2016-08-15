using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ProviderPayments.Application.Validation
{
    public class ValidationResult
    {
        public ValidationResult()
        {
            Failures = new ValidationFailure[0];
        }

        public IEnumerable<ValidationFailure> Failures { get; set; }

        public bool IsValid()
        {
            if (Failures == null)
            {
                return true;
            }

            return !Failures.Any();
        }
    }
}