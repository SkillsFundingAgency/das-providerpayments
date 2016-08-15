using System.Threading.Tasks;

namespace SFA.DAS.ProdiverPayments.Application.Validation
{
    public interface IValidator<T>
    {
        Task<ValidationResult> ValidateAsync(T item);
    }
}
