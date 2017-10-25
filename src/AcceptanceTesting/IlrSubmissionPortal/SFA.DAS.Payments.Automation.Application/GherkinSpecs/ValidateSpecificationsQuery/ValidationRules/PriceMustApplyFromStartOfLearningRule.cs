using System;
using System.Collections.Generic;
using SFA.DAS.Payments.Automation.Domain.Specifications;

namespace SFA.DAS.Payments.Automation.Application.GherkinSpecs.ValidateSpecificationsQuery.ValidationRules
{
    public class PriceMustApplyFromStartOfLearningRule : ValidationRule
    {
        public override ValidationViolation[] ValidateLearner(Specification specification, LearnerRecord learner)
        {
            var violations = new List<ValidationViolation>();

            EnsureNotTooLate(specification, learner, violations);
            EnsureNotTooEarly(specification, learner, violations);

            return violations.ToArray();
        }

        private void EnsureNotTooLate(Specification specification, LearnerRecord learner, List<ValidationViolation> violations)
        {
            if (learner.TotalTrainingPrice1EffectiveDate > learner.StartDate)
            {
                violations.Add(new ValidationViolation
                {
                    RuleId = ValidationRuleIds.PriceEffectiveTooLate,
                    SpecificationName = specification.Name,
                    Description = $"Learner {learner.LearnerKey} has training price 1 effective from {learner.TotalTrainingPrice1EffectiveDate:dd/MM/yyyy}, which is after start date of {learner.StartDate:dd/MM/yyyy}"
                });
            }

            if (learner.TotalAssessmentPrice1EffectiveDate.HasValue && learner.TotalAssessmentPrice1EffectiveDate > learner.StartDate)
            {
                violations.Add(new ValidationViolation
                {
                    RuleId = ValidationRuleIds.PriceEffectiveTooLate,
                    SpecificationName = specification.Name,
                    Description = $"Learner {learner.LearnerKey} has assessment price 1 effective from {learner.TotalAssessmentPrice1EffectiveDate:dd/MM/yyyy}, which is after start date of {learner.StartDate:dd/MM/yyyy}"
                });
            }
        }
        private void EnsureNotTooEarly(Specification specification, LearnerRecord learner, List<ValidationViolation> violations)
        {
            if (learner.TotalTrainingPrice1EffectiveDate != DateTime.MinValue && learner.TotalTrainingPrice1EffectiveDate < learner.StartDate)
            {
                violations.Add(new ValidationViolation
                {
                    RuleId = ValidationRuleIds.PriceEffectiveTooEarly,
                    SpecificationName = specification.Name,
                    Description = $"Learner {learner.LearnerKey} has training price 1 effective from {learner.TotalTrainingPrice1EffectiveDate:dd/MM/yyyy}, which is after start date of {learner.StartDate:dd/MM/yyyy}"
                });
            }

            if (learner.TotalAssessmentPrice1EffectiveDate.HasValue && learner.TotalAssessmentPrice1EffectiveDate < learner.StartDate)
            {
                violations.Add(new ValidationViolation
                {
                    RuleId = ValidationRuleIds.PriceEffectiveTooEarly,
                    SpecificationName = specification.Name,
                    Description = $"Learner {learner.LearnerKey} has assessment price 1 effective from {learner.TotalAssessmentPrice1EffectiveDate:dd/MM/yyyy}, which is after start date of {learner.StartDate:dd/MM/yyyy}"
                });
            }
        }
    }
}
