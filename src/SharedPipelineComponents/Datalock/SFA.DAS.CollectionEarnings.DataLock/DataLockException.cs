using System;

namespace SFA.DAS.CollectionEarnings.DataLock
{
    public class DataLockException : Exception
    {
        public const string ContextPropertiesNoYearOfCollectionMessage = "The context does not contain the year of collection property.";
        public const string ContextPropertiesInvalidYearOfCollectionMessage = "The context does not contain a valid year of collection property.";

        public const string ErrorReadingProvidersMessage = "Error while reading providers.";
        public const string ErrorReadingCommitmentsMessage = "Error while reading commitments.";
        public const string ErrorReadingPriceEpisodesMessage = "Error while reading data lock specific price episodes.";
        public const string ErrorPerformingDataLockMessage = "Error while performing data lock.";
        public const string ErrorWritingDataLockValidationErrorsMessage = "Error while writing data lock validation errors.";
        public const string ErrorWritingPriceEpisodeMatchesMessage = "Error while writing price episode matches.";
        public const string ErrorWritingPriceEpisodePeriodMatchesMessage = "Error while writing price episode period matches.";
        public const string ErrorReadingAccountsMessage = "Error while reading das accounts.";

        public const string ErrorDeletingPriceEpisodePeriodMatchesMessage = "Error while deleting price episode period matches.";
        public const string ErrorReadingIncentiveEarningsMessage = "Error while reading data for incentive earnings";

        public DataLockException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}