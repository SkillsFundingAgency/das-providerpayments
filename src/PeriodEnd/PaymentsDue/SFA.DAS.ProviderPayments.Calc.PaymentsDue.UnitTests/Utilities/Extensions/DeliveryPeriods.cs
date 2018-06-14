namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.Extensions
{
    public static class DeliveryPeriods
    {
        public static int  DeliveryMonthFromPeriod(this int period)
        {
            if (period < 6)
            {
                return period + 7;
            }

            return period - 5;
        }

        public static int DeliveryYearFromPeriod(this int period)
        {
            if (period < 6)
            {
                return 2017;
            }

            return 2018;
        }
    }
}
