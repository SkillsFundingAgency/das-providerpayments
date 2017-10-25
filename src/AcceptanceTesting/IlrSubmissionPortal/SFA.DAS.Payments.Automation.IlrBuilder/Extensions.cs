using System;

namespace SFA.DAS.Payments.Automation.IlrBuilder
{
    internal static class Extensions
    {
        internal static string ToAcademicYear(this DateTime date)
        {
            var startYear = (date.Month >= 8 ? date.Year : date.Year - 1) - 2000;
            return $"{startYear}{startYear + 1}";
        }
    }
}
