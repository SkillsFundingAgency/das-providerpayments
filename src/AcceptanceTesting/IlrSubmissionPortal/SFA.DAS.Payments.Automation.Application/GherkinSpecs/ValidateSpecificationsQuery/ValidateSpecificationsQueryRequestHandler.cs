using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.Payments.Automation.Application.GherkinSpecs.ValidateSpecificationsQuery.ValidationRules;

namespace SFA.DAS.Payments.Automation.Application.GherkinSpecs.ValidateSpecificationsQuery
{
    public class ValidateSpecificationsQueryRequestHandler : IRequestHandler<ValidateSpecificationsQueryRequest, ValidateSpecificationsQueryResponse>
    {
        private static readonly ValidationRule[] Rules =
        {
            new CourseDurationRule(),
            new PriceMustApplyFromStartOfLearningRule()
        };

        public ValidateSpecificationsQueryResponse Handle(ValidateSpecificationsQueryRequest message)
        {
            var violations = new List<ValidationViolation>();
            foreach (var specification in message.Specifications)
            {
                foreach (var rule in Rules)
                {
                    var result = rule.Validate(specification);
                    if (result != null && result.Length > 0)
                    {
                        violations.AddRange(result);
                    }
                }
            }
            return new ValidateSpecificationsQueryResponse { Errors = violations.ToArray() };
        }
    }
}