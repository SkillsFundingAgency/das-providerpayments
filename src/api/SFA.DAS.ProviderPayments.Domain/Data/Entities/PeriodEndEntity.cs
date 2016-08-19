using System;

namespace SFA.DAS.ProviderPayments.Domain.Data.Entities
{
    public class PeriodEndEntity
    {
        public string PeriodCode { get; set; }
        public int PeriodType { get; set; }
        public decimal TotalValue { get; set; }
        public int NumberOfProviders { get; set; }
        public int NumberOfEmployers { get; set; }
        public DateTime PaymentRunDate { get; set; }
    }
}
