using System;
using System.Globalization;

namespace SFA.DAS.CollectionEarnings.DataLock.Domain
{
    class PriceEpisode
    {
        public PriceEpisode(string priceEpisodeIdentifier)
        {
            var datePart = priceEpisodeIdentifier.Substring(priceEpisodeIdentifier.Length - 10);
            var date = DateTime.ParseExact(datePart, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var firstDayOfAcademicYear = FirstDayOfAcademicYear(date);
            var firstYearForAcademicYear = firstDayOfAcademicYear.Year % 100;
            AcademicYear = $"{firstYearForAcademicYear}{firstYearForAcademicYear + 1}";
        }

        public string AcademicYear { get; set; }

        DateTime FirstDayOfAcademicYear(DateTime source)
        {
            if (source.Month < 8)
            {
                return new DateTime(source.Year - 1, 8, 1);
            }
            return new DateTime(source.Year, 8, 1);
        }
    }
}
