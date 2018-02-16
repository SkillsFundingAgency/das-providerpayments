using System;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue
{
    [Serializable]
    public class PaymentsDueProcessorException : Exception
    {
        public const string ErrorReadingCollectionPeriodMessage = "Error while reading the current collection period.";
        public const string ErrorNoCollectionPeriodMessage = "Could not find the current collection period.";
        public const string ErrorReadingProvidersMessage = "Error while reading the providers.";
        public const string ErrorReadingProviderEarningsMessage = "Error while reading the provider's earnings.";
        public const string ErrorReadingPaymentHistoryMessage = "Error while reading the provider's payment history.";
        public const string ErrorReadingPaymentHistoryWithoutEarningsMessage = "Error while reading the provider's payment history that has no earnings.";
        public const string ErrorWritingRequiredProviderPaymentsMessage = "Error while writing the provider's required payments.";

        public PaymentsDueProcessorException(string message)
            : base(message)
        {
        }

        public PaymentsDueProcessorException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}