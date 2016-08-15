using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ProdiverPayments.Application.Validation;
using SFA.DAS.ProdiverPayments.Application.Validation.Failures;

namespace SFA.DAS.ProdiverPayments.Application.PeriodEnd.GetPageOfPeriodEndsQuery
{
    public class GetPageOfPeriodEndsQueryRequestValidator : IValidator<GetPageOfPeriodEndsQueryRequest>
    {
        public Task<ValidationResult> ValidateAsync(GetPageOfPeriodEndsQueryRequest item)
        {
            var failures = new List<ValidationFailure>();

            if (item.PageNumber < 1)
            {
                failures.Add(new InvalidPageNumberFailure());
            }

            return Task.FromResult(new ValidationResult { Failures = failures });
        }
    }
}
