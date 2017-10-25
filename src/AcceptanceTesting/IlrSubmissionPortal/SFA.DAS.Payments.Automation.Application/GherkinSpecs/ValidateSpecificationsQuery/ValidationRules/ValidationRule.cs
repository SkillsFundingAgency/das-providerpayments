using System.Collections.Generic;
using SFA.DAS.Payments.Automation.Domain.Specifications;

namespace SFA.DAS.Payments.Automation.Application.GherkinSpecs.ValidateSpecificationsQuery.ValidationRules
{
    public abstract class ValidationRule
    {
        public virtual ValidationViolation[] Validate(Specification specification)
        {
            if (specification == null || specification.Arrangement == null || specification.Arrangement.LearnerRecords == null)
            {
                return null;
            }

            var violations = new List<ValidationViolation>();
            foreach (var learner in specification.Arrangement.LearnerRecords)
            {
                var learnerViolations = ValidateLearner(specification, learner);
                if (learnerViolations != null)
                {
                    violations.AddRange(learnerViolations);
                }
            }
            return violations.ToArray();
        }
        public virtual ValidationViolation[] ValidateLearner(Specification specification, LearnerRecord learner)
        {
            return null;
        }
    }
}
