using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ProviderPayments.Application.Validation;

namespace SFA.DAS.ProviderPayments.Api.Orchestrators.OrchestratorExceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(ValidationFailure failure)
            : this(new[] { failure })
        {
            Failures = new[] { failure };
        }
        public BadRequestException(IEnumerable<ValidationFailure> failures)
            : base(AggregateFailureMessages(failures))
        {
            Failures = failures;
        }
        public IEnumerable<ValidationFailure> Failures { get; set; }

        private static string AggregateFailureMessages(IEnumerable<ValidationFailure> failures)
        {
            return failures.Select(x => x.Description).Aggregate((x, y) => $"{x}\n{y}");
        }
    }
}