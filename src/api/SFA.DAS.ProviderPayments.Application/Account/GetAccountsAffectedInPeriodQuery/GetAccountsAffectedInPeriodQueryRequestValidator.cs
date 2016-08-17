﻿using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SFA.DAS.ProviderPayments.Application.Validation;
using SFA.DAS.ProviderPayments.Application.Validation.Failures;

namespace SFA.DAS.ProviderPayments.Application.Account.GetAccountsAffectedInPeriodQuery
{
    public class GetAccountsAffectedInPeriodQueryRequestValidator : IValidator<GetAccountsAffectedInPeriodQueryRequest>
    {
        public Task<ValidationResult> ValidateAsync(GetAccountsAffectedInPeriodQueryRequest item)
        {
            var failures = new List<ValidationFailure>();

            if (!ValidatePeriodCode(item.PeriodCode))
            {
                failures.Add(new InvalidPeriodCodeFailure());
            }
            if (!ValidatePageNumber(item.PageNumber))
            {
                failures.Add(new InvalidPageNumberFailure());
            }

            return Task.FromResult(new ValidationResult
            {
                Failures = failures
            });
        }

        private bool ValidatePeriodCode(string periodCode)
        {
            var match = Regex.Match(periodCode, @"(\d{4})(\d{2})");
            if (!match.Success)
            {
                return false;
            }

            var year = int.Parse(match.Groups[1].Value);
            if (year < 2016)
            {
                return false;
            }

            var month = int.Parse(match.Groups[2].Value);
            if (month < 1 || month > 12)
            {
                return false;
            }

            return true;
        }
        private bool ValidatePageNumber(int pageNumber)
        {
            return pageNumber > 0;
        }
    }
}
