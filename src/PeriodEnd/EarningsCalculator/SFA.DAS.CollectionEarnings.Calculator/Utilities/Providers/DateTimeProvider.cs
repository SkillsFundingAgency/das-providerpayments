using System;

namespace SFA.DAS.CollectionEarnings.Calculator.Tools.Providers
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTimeProvider(string yearOfCollection)
        {
            var year = 2000 + int.Parse(yearOfCollection.Substring(0, 2));

            YearOfCollectionStart = new DateTime(year, 8, 1);
        }

        public DateTime Now
        {
            get { return DateTime.Now; }
        }

        public DateTime Today
        {
            get { return DateTime.Today; }
        }

        public DateTime YearOfCollectionStart { get; }
    }
}