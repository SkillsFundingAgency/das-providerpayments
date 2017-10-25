namespace SFA.DAS.Payments.Automation.IlrBuilder.Validation
{
    public static class ErrorCodes
    {
        public const string UkprnMissingCode = "UKPRN-MISSING";

        public const string PrepDateNotInYearCode = "PREPDATE-YEAR";
        public const string PrepDateBeforeLearnStart = "PREPDATE-LEARNSTART";
    }
}
