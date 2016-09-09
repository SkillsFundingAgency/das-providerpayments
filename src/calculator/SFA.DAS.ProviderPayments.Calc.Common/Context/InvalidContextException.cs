using System;

namespace SFA.DAS.ProviderPayments.Calc.Common.Context
{
    public class InvalidContextException : Exception
    {
        public const string ContextNullMessage = "The received context is null.";
        public const string ContextNoPropertiesMessage = "The received context contains no properties.";
        public const string ContextPropertiesNoConnectionStringMessage = "The context does not contain the transientt database connection string property.";
        public const string ContextPropertiesNoLogLevelMessage = "The context does not contain the logging level property.";
        public const string ContextPropertiesInvalidLogLevelMessage = "The context does not contain a valid logging level.";

        public InvalidContextException(string message)
            : base(message)
        {
        }

        public InvalidContextException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}
