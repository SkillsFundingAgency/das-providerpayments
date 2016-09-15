using System;

namespace SFA.DAS.ProviderPayments.Calc.Common.Tools.Extensions
{
    public static class DateTimeDayOfMonthExtensions
    {
        public static int DaysInMonth(this DateTime value)
        {
            return DateTime.DaysInMonth(value.Year, value.Month);
        }

        public static DateTime LastDayOfMonth(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.DaysInMonth());
        }
    }
}