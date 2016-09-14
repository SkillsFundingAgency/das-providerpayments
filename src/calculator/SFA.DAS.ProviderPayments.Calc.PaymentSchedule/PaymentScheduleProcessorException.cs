using System;

namespace SFA.DAS.ProviderPayments.Calc.PaymentSchedule
{
    public class PaymentScheduleProcessorException : Exception
    {
        public const string ErrorReadingCollectionPeriodMessage = "Error while reading the current collection period.";
        public const string ErrorNoCollectionPeriodMessage = "Could not find the current collection period.";
        public const string ErrorReadingProvidersMessage = "Error while reading the providers.";
        public const string ErrorReadingProviderEarningsMessage = "Error while reading the provider's earnings.";
        public const string ErrorWritingRequiredProviderPaymentsMessage = "Error while writing the provider's required payments.";

        public PaymentScheduleProcessorException(string message)
            : base(message)
        {
        }

        public PaymentScheduleProcessorException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}