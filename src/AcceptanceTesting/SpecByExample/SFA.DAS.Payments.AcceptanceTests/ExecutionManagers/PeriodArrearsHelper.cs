﻿namespace SFA.DAS.Payments.AcceptanceTests.ExecutionManagers
{
    //The only reason this class exists is because it is more readable from a business perspective
    //to have the period for payments and balances to be 1 month later than the actual period
    //because this is when the payments are actually made. Ideally we should come to some sort of
    //agreement about displaying periods as R01, R02 etc. and nuke this class and everywhere it is
    //called.
    public static class PeriodArrearsHelper
    {
        public static string GetEarningsMonthForPaymentsMadeIn(string periodDate)
        {
            if (string.IsNullOrEmpty(periodDate))
                return null;

            switch (periodDate.ToUpper())
            {
                case "09/17": return "08/17";
                case "10/17": return "09/17";
                case "11/17": return "10/17";
                case "12/17": return "11/17";
                case "01/18": return "12/17";
                case "02/18": return "01/18";
                case "03/18": return "02/18";
                case "04/18": return "03/18";
                case "05/18": return "04/18";
                case "06/18": return "05/18";
                case "07/18": return "06/18";
                case "08/18": return "07/18";
                case "09/18": return "08/18";
                case "10/18": return "09/18";
                case "11/18": return "10/18";
                default: return null;
            }
        }
    }
}