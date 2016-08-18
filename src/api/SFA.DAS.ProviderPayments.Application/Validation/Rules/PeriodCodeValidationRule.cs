using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SFA.DAS.ProviderPayments.Application.Validation.Failures;
using SFA.DAS.ProviderPayments.Domain.Data;

namespace SFA.DAS.ProviderPayments.Application.Validation.Rules
{
    public class PeriodCodeValidationRule : IValidationRule<string>
    {
        private readonly IPeriodEndRepository _periodEndRepository;

        public PeriodCodeValidationRule(IPeriodEndRepository periodEndRepository)
        {
            _periodEndRepository = periodEndRepository;
        }
        protected PeriodCodeValidationRule()
        {
        }

        public virtual async Task<IEnumerable<ValidationFailure>> ValidateAsync(string value)
        {
            var failures = new List<ValidationFailure>();

            if (!IsPreiodCodeInCorrectFormat(value))
            {
                failures.Add(new InvalidPeriodCodeFailure());
            }
            else
            {
                var period = await _periodEndRepository.GetPeriodEndAsync(value);
                if (period == null)
                {
                    failures.Add(new PeriodNotFoundFailure());
                }
            }

            return failures;
        }
        private bool IsPreiodCodeInCorrectFormat(string periodCode)
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
    }
}
