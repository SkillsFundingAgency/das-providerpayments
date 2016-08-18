namespace SFA.DAS.ProviderPayments.Application.Validation.Failures
{
    public class PageNotFoundFailure : ValidationFailure
    {
        public const string FailureCode = "PageNotFound";
        public const string FailureDescription = "Specified page does not exist";

        public PageNotFoundFailure()
        {
            Code = FailureCode;
            Description = FailureDescription;
        }
    }
}
