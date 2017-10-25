using System;

namespace SFA.DAS.CollectionEarnings.Calculator
{
    [Serializable]
    public class EarningsCalculatorException : Exception
    {
        public const string ContextPropertiesNoYearOfCollectionMessage = "The context does not contain the year of collection property.";
        public const string ContextPropertiesInvalidYearOfCollectionMessage = "The context does not contain a valid year of collection property.";

        public const string ErrorReadingLearningDeliveriesToProcessMessage = "Error while reading learning deliveries to process.";
        public const string ErrorCalculatingEarningsForTheLearningDeliveriesMessage = "Error while processing the learning deliveries to calculate the earnings.";
        public const string ErrorWritingProcessedLearningDeliveriesMessage = "Error while writing processed learning deliveries.";
        public const string ErrorWritingProcessedLearningDeliveryPeriodisedValuesMessage = "Error while writing processed learning deliveries periodised values.";
        public const string ErrorWritingLearningDeliveriesPeriodEarningsMessage = "Error while writing learning deliveries period earnings.";

        public EarningsCalculatorException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}