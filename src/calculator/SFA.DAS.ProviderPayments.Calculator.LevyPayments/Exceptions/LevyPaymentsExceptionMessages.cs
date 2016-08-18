namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.Exceptions
{
    public class LevyPaymentsExceptionMessages
    {
        public const string ContextNull = "The received context is null.";
        public const string ContextNoProperties = "The received context contains no properties.";
        public const string ContextPropertiesNoConnectionString = "The context does not contain the transientt database connection string property.";
        public const string ContextPropertiesNoLogLevel = "The context does not contain the logging level property.";
        public const string ContextPropertiesInvalidLogLevel = "The context does not contain a valid logging level.";
    }
}
