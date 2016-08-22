using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ProviderPayments.Application.Account.Failures;
using SFA.DAS.ProviderPayments.Application.Validation;
using SFA.DAS.ProviderPayments.Application.Validation.Rules;
using SFA.DAS.ProviderPayments.Domain.Data;

namespace SFA.DAS.ProviderPayments.Application.Account.Rules
{
    public class AccountIdValidationRule : IValidationRule<string>
    {
        private readonly IAccountRepository _accountRepository;

        public AccountIdValidationRule(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        protected AccountIdValidationRule()
        {
        }

        public virtual async Task<IEnumerable<ValidationFailure>> ValidateAsync(string value)
        {
            var account = await _accountRepository.GetAccountAsync(value);
            if (account == null)
            {
                return new[] { new AccountNotFoundFailure() };
            }

            return new ValidationFailure[0];
        }
    }
}
