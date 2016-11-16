using System;
using SFA.DAS.Payments.DCFS.Context;

namespace SFA.DAS.ProviderPayments.Calc.Common.Context
{
    public class PaymentsInvalidContextException : InvalidContextException
    {
        public const string ContextPropertiesNoYearOfCollectionMessage = "The context does not contain the year of collection property.";
        public const string ContextPropertiesInvalidYearOfCollectionMessage = "The context does not contain a valid year of collection property.";

        public PaymentsInvalidContextException(string message)
            : base(message)
        {
        }

        public PaymentsInvalidContextException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}