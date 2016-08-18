namespace SFA.DAS.ProviderPayments.Application.Validation.Failures
{
    public class InvalidPeriodCodeFailure : ValidationFailure
    {
        public const string FailureCode = "InvalidPeriodCode";
        public const string FailureDescription = "Period code must be in the format YYYYMM";

        public InvalidPeriodCodeFailure()
        {
            Code = FailureCode;
            Description = FailureDescription;
        }
    }
}
