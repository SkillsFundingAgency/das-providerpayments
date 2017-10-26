using System;

namespace SFA.DAS.CollectionEarnings.DataLock.Tools.Extensions
{
    public static class DateTimeDayOfMonthExtensions
    {
        public static DateTime FirstDayOfMonth(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, 1);
        }
    }
}