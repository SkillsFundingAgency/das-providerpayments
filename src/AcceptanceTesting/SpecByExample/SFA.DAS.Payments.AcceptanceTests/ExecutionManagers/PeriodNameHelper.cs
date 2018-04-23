using System;
using System.Linq;

namespace SFA.DAS.Payments.AcceptanceTests.ExecutionManagers
{
    public static class PeriodNameHelper
    {
        public static DateTime? GetDateFromPeriodName(string periodInRNotation, DateTime yearStartDate)
        {
            if (string.IsNullOrEmpty(periodInRNotation))
                return null;

            switch (periodInRNotation.ToUpper())
            {
                case "R01": return new DateTime(yearStartDate.Year, 8, 1);
                case "R02": return new DateTime(yearStartDate.Year, 9, 1);
                case "R03": return new DateTime(yearStartDate.Year, 10, 1);
                case "R04": return new DateTime(yearStartDate.Year, 11, 1);
                case "R05": return new DateTime(yearStartDate.Year, 12, 1);
                case "R06": return new DateTime(yearStartDate.Year + 1, 1, 1);
                case "R07": return new DateTime(yearStartDate.Year + 1, 2, 1);
                case "R08": return new DateTime(yearStartDate.Year + 1, 3, 1);
                case "R09": return new DateTime(yearStartDate.Year + 1, 4, 1);
                case "R10": return new DateTime(yearStartDate.Year + 1, 5, 1);
                case "R11": return new DateTime(yearStartDate.Year + 1, 6, 1);
                case "R12": return new DateTime(yearStartDate.Year + 1, 7, 1);
                case "R13": return new DateTime(yearStartDate.Year + 1, 8, 1);
                case "R14": return new DateTime(yearStartDate.Year + 1, 9, 1);
                default: return new DateTime(yearStartDate.Year, 8, 1);
            }
        }

        public static string GetPeriodFromStringDate(string periodDate)
        {
            if (string.IsNullOrEmpty(periodDate))
                return null;

            switch (periodDate.ToUpper())
            {
                case "08/17": return "R01";
                case "09/17": return "R02";
                case "10/17": return "R03";
                case "11/17": return "R04";
                case "12/17": return "R05";
                case "01/18": return "R06";
                case "02/18": return "R07";
                case "03/18": return "R08";
                case "04/18": return "R09";
                case "05/18": return "R10";
                case "06/18": return "R11";
                case "07/18": return "R12";
                case "09/18": return "R13";
                case "10/18": return "R14";
                default: return null;
            }
        }

        public static DateTime? GetDateFromStringDate(string periodDate)
        {
            if (string.IsNullOrEmpty(periodDate))
                return null;

            var dateParts = periodDate.Split('/');
            if (dateParts.Length != 2)
                return null;
            int year;
            if (!int.TryParse($"20{dateParts[1]}", out year))
                return null;
            int month;
            if (!int.TryParse(dateParts[0], out month))
                return null;
            const int day = 1;

            return new DateTime(year, month, day);
        }

        public static string GetStringDateFromPeriod(string period)
        {
            if (string.IsNullOrEmpty(period))
                return null;

            switch (period.ToUpper())
            {
                case "R01": return "08/17";
                case "R02": return "09/17";
                case "R03": return "10/17";
                case "R04": return "11/17";
                case "R05": return "12/17";
                case "R06": return "01/18";
                case "R07": return "02/18";
                case "R08": return "03/18";
                case "R09": return "04/18";
                case "R10": return "05/18";
                case "R11": return "06/18";
                case "R12": return "07/18";
                case "R13": return "09/18";
                case "R14": return "10/18";
                default: return null;
            }
        }

        public static int GetNumericalPeriodFromPeriod(string period)
        {
            if (string.IsNullOrEmpty(period))
                return 0;

            return int.Parse(period.ToCharArray().Last().ToString());
        }
    }
}