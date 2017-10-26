using System;

namespace SFA.DAS.CollectionEarnings.DataLock.Tools.Providers
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTimeProvider(string yearOfCollection)
        {
            var year = 2000 + int.Parse(yearOfCollection.Substring(0, 2));

            YearOfCollectionStart = new DateTime(year, 8, 1);
        }

        public DateTime YearOfCollectionStart { get; }
    }
}