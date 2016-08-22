using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ProviderPayments.Application.Validation.Failures;

namespace SFA.DAS.ProviderPayments.Application.Validation.Rules
{
    public class PageNumberValidationRule : IValidationRule<int>
    {
        public virtual Task<IEnumerable<ValidationFailure>> ValidateAsync(int value)
        {
            if (value < 1)
            {
                return Task.FromResult<IEnumerable<ValidationFailure>>(new[]
                {
                    new InvalidPageNumberFailure()
                });
            }
            return Task.FromResult<IEnumerable<ValidationFailure>>(new ValidationFailure[0]);
        }
    }
}
