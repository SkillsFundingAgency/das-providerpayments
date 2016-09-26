using System;
using SFA.DAS.ProviderPayments.Calc.Common.Interfaces;

namespace SFA.DAS.ProviderPayments.Calc.Common.Tools
{
    public class DateTimeProvider : IDateTimeProvider
    {
        //public DateTimeProvider()
        //{
        //}

        public DateTime Now => DateTime.Now;
    }
}
