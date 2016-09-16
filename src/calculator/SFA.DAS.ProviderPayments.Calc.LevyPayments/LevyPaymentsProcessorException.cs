using System;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments
{
    public class LevyPaymentsProcessorException : Exception
    {
        public const string ErrorReadingCollectionPeriodMessage = "Error while reading the current collection period.";
        public const string ErrorNoCollectionPeriodMessage = "Could not find the current collection period.";
        public const string ErrorReadingPaymentsDueForCommitmentMessage = "Error while reading the payments due for the commitment.";

        public LevyPaymentsProcessorException(string message)
            : base(message)
        {
        }

        public LevyPaymentsProcessorException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}