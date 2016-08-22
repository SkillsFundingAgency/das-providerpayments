using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderPayments.Application.Validation.Rules
{
    public interface IValidationRule<T>
    {
        Task<IEnumerable<ValidationFailure>> ValidateAsync(T value);
    }
}
