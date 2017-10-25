using System;
using System.Linq;

namespace SFA.DAS.Payments.Automation.Application.GherkinSpecs
{
    public class InvalidSpecificationsException : Exception
    {
        public InvalidSpecificationsException(ValidationViolation[] exceptions)
            : base(BuildExceptionMessage(exceptions))
        {
            InnerExceptions = exceptions;
        }

        public ValidationViolation[] InnerExceptions { get; }

        private static string BuildExceptionMessage(ValidationViolation[] exceptions)
        {
            return "Specifications are not valid.\n" + exceptions.Select(e => e.Description).Aggregate((x, y) => $"{x}\n{y}");
        }
    }
}
