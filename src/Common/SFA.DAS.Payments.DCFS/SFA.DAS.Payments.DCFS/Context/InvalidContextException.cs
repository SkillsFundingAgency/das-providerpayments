using System;

namespace SFA.DAS.Payments.DCFS.Context
{
    public class InvalidContextException : Exception
    {
        public const string ContextNullMessage = "The context is null.";
        public const string ContextNoPropertiesMessage = "The context contains no properties.";
        public const string ContextPropertiesNoConnectionStringMessage = "The context does not contain the transient database connection string property.";
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
