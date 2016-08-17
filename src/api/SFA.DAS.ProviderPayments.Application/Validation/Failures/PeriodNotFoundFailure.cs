namespace SFA.DAS.ProviderPayments.Application.Validation.Failures
{
    public class PeriodNotFoundFailure : ValidationFailure
    {
        public const string FailureCode = "PeriodNotFound";
        public const string FailureDescription = "Specified period does not exist";

        public PeriodNotFoundFailure()
        {
            Code = FailureCode;
            Description = FailureDescription;
        }
    }
}
