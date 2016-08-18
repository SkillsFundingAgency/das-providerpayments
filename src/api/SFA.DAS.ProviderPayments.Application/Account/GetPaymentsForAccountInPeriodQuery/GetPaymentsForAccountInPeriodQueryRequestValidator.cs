using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ProviderPayments.Application.Validation;
using SFA.DAS.ProviderPayments.Application.Validation.Rules;

namespace SFA.DAS.ProviderPayments.Application.Account.GetPaymentsForAccountInPeriodQuery
{
    public class GetPaymentsForAccountInPeriodQueryRequestValidator : IValidator<GetPaymentsForAccountInPeriodQueryRequest>
    {
        private readonly PageNumberValidationRule _pageNumberValidationRule;
        private readonly PeriodCodeValidationRule _periodCodeValidationRule;

        public GetPaymentsForAccountInPeriodQueryRequestValidator(PageNumberValidationRule pageNumberValidationRule, PeriodCodeValidationRule periodCodeValidationRule)
        {
            _pageNumberValidationRule = pageNumberValidationRule;
            _periodCodeValidationRule = periodCodeValidationRule;
        }

        public async Task<ValidationResult> ValidateAsync(GetPaymentsForAccountInPeriodQueryRequest item)
        {
            var failures = new List<ValidationFailure>();

            //failures.AddRange(await _pageNumberValidationRule.ValidateAsync(item.PageNumber));
            //failures.AddRange(await _periodCodeValidationRule.ValidateAsync(item.PeriodCode));

            return new ValidationResult { Failures = failures };
        }
    }
}
