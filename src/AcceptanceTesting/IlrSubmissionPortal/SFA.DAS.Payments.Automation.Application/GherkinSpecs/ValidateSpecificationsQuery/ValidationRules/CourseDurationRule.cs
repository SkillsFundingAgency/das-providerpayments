using System;
using SFA.DAS.Payments.Automation.Domain.Specifications;

namespace SFA.DAS.Payments.Automation.Application.GherkinSpecs.ValidateSpecificationsQuery.ValidationRules
{
    public class CourseDurationRule : ValidationRule
    {
        public override ValidationViolation[] ValidateLearner(Specification specification, LearnerRecord learner)
        {
            var duration = (int)Math.Ceiling((learner.PlannedEndDate - learner.StartDate).TotalDays);
            var requiredDuration = learner.StandardCode.HasValue ? 372 : 365;
            if (duration < requiredDuration)
            {
                return new[]
                {
                    new ValidationViolation
                    {
                        RuleId = learner.StandardCode.HasValue ? ValidationRuleIds.StandardMinDuration : ValidationRuleIds.FrameworkMinDuration,
                        SpecificationName = specification.Name,
                        Description = $"Learner {learner.LearnerKey} has a learning duration of {duration} days, which is less than the minimum of {requiredDuration}"
                    }
                };
            }
            return null;
        }
    }
}
