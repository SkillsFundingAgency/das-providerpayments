using System;

namespace SFA.DAS.ProviderPayments.Api.Dto
{
    public class PeriodEndDto
    {
        public PeriodDto Period { get; set; }
        public decimal TotalValue { get; set; }
        public int NumberOfProviders { get; set; }
        public int NumberOfEmployers { get; set; }
        public DateTime PaymentRunDate { get; set; }
    }
}
