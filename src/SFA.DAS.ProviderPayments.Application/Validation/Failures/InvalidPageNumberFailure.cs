namespace SFA.DAS.ProviderPayments.Application.Validation.Failures
{
    public class InvalidPageNumberFailure : ValidationFailure
    {
        public const string FailureCode = "InvalidPageNumber";
        public const string FailureDescription = "Page number must be 1 or greater";

        public InvalidPageNumberFailure()
        {
            Code = FailureCode;
            Description = FailureDescription;
        }
    }
}
