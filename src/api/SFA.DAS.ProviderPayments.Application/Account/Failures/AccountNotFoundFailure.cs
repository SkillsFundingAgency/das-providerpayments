using SFA.DAS.ProviderPayments.Application.Validation;

namespace SFA.DAS.ProviderPayments.Application.Account.Failures
{
    public class AccountNotFoundFailure : ValidationFailure
    {
        public const string FailureCode = "AccountNotFound";
        public const string FailureDescription = "Specified account does not exist";

        public AccountNotFoundFailure()
        {
            Code = FailureCode;
            Description = FailureDescription;
        }
    }
}
