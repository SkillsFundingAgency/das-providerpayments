using System;
using System.Linq;

namespace SFA.DAS.Payments.Automation.Application.GherkinSpecs.ValidateSpecificationsQuery
{
    public class ValidateSpecificationsQueryResponse
    {
        public ValidationViolation[] Errors { get; set; }
        public bool IsValid => Errors == null || !Errors.Any();
    }
}