using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ProviderPayments.Application.Account.Rules;
using SFA.DAS.ProviderPayments.Application.Validation;
using SFA.DAS.ProviderPayments.Application.Validation.Rules;

namespace SFA.DAS.ProviderPayments.Application.Account.GetPaymentsForAccountInPeriodQuery
{
    public class GetPaymentsForAccountInPeriodQueryRequestValidator : IValidator<GetPaymentsForAccountInPeriodQueryRequest>
    {
        private readonly PageNumberValidationRule _pageNumberValidationRule;
        private readonly PeriodCodeValidationRule _periodCodeValidationRule;
        private readonly AccountIdValidationRule _accountIdValidationRule;

        public GetPaymentsForAccountInPeriodQueryRequestValidator(PageNumberValidationRule pageNumberValidationRule, 
                                                                  PeriodCodeValidationRule periodCodeValidationRule,
                                                                  AccountIdValidationRule accountIdValidationRule)
        {
            _pageNumberValidationRule = pageNumberValidationRule;
            _periodCodeValidationRule = periodCodeValidationRule;
            _accountIdValidationRule = accountIdValidationRule;
        }

        public async Task<ValidationResult> ValidateAsync(GetPaymentsForAccountInPeriodQueryRequest item)
        {
            var failures = new List<ValidationFailure>();

            failures.AddRange(await _pageNumberValidationRule.ValidateAsync(item.PageNumber));
            failures.AddRange(await _periodCodeValidationRule.ValidateAsync(item.PeriodCode));
            failures.AddRange(await _accountIdValidationRule.ValidateAsync(item.AccountId));

            return new ValidationResult { Failures = failures };
        }
    }
}
