using System;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments
{
    public class CoInvestedPaymentsProcessorException : Exception
    {
        public const string ErrorReadingCollectionPeriodMessage = "Error while reading the current collection period.";
        public const string ErrorNoCollectionPeriodMessage = "Could not find the current collection period.";
        public const string ErrorReadingProviders = "Error while reading the providers";
        public const string ErrorReadingProviderEarnings = "Error while reading the provider earnings";
        public const string ErrorReadingPaymentsDueForUkprn = "Error while reading payments due for UKPRN.";
        public const string ErrorWritingPaymentsForUkprn = "Error while writing co-invested payments for UKPRN.";
        public const string ErrorReadingPaymentsHistoryMessage = "Error while writing co-invested payments history for UKPRN.";
        public CoInvestedPaymentsProcessorException(string message)
            : base(message)
        {
        }

        public CoInvestedPaymentsProcessorException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}