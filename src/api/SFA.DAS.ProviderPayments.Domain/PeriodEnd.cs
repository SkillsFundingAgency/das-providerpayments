using System;

namespace SFA.DAS.ProviderPayments.Domain
{
    public class PeriodEnd
    {
        public Period Period { get; set; }
        public decimal TotalValue { get; set; }
        public int NumberOfProviders { get; set; }
        public int NumberOfEmployers { get; set; }
        public DateTime PaymentRunDate { get; set; }
    }
}
