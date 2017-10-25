using System.Linq;

namespace SFA.DAS.Payments.Automation.IlrBuilder.Validation
{
    public class ValidationResult
    {
        public ValidationResult(ValidationError[] errors)
        {
            Errors = errors ?? new ValidationError[0];
        }
        public bool IsValid => !Errors.Any();
        public ValidationError[] Errors { get; }
    }
}