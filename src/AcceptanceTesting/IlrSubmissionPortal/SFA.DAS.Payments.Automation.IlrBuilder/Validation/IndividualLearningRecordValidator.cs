using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Payments.Automation.IlrBuilder.Validation.PreparationDateRules;
using SFA.DAS.Payments.Automation.IlrBuilder.Validation.UkprnRules;

namespace SFA.DAS.Payments.Automation.IlrBuilder.Validation
{
    public interface IIndividualLearningRecordValidator
    {
        ValidationResult Validate(IndividualLearningRecord ilr);
    }

    public class IndividualLearningRecordValidator : IIndividualLearningRecordValidator
    {
        private readonly ValidationRule[] _rules = {
            new MustHaveUkprnRule(),
            new PrepDateMustBeInAcademicYearRule(),
            new PrepDateMustNotBeforeBeforeLearnStartDatesRule()
        };

        public ValidationResult Validate(IndividualLearningRecord ilr)
        {
            var errors = new List<ValidationError>();
            foreach (var rule in _rules)
            {
                var ruleErrors = rule.Validate(ilr);
                if (ruleErrors != null && ruleErrors.Any())
                {
                    errors.AddRange(ruleErrors);
                }
            }
            return new ValidationResult(errors.ToArray());
        }
    }
}
