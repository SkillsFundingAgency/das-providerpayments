using System;

namespace SFA.DAS.Payments.Calc.ProviderAdjustments
{
    public class ProviderAdjustmentsProcessorException : Exception
    {
        public const string ErrorReadingCollectionPeriodMessage = "Error while reading the current collection period.";
        public const string ErrorNoCollectionPeriodMessage = "Could not find the current collection period.";
        public const string ErrorReadingProvidersMessage = "Error while reading the providers.";
        public const string ErrorReadingCurrentAdjustmentsMessage = "Error while reading the provider's current adjustments.";
        public const string ErrorReadingPreviousAdjustmentsMessage = "Error while reading the provider's previous adjustments.";
        public const string ErrorWritingAdjustmentsMessage = "Error while writing the provider's new adjustments.";

        public ProviderAdjustmentsProcessorException(string message)
            : base(message)
        {
        }

        public ProviderAdjustmentsProcessorException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}