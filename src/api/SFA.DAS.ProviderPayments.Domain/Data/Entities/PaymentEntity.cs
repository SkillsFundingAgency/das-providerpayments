using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderPayments.Domain.Data.Entities
{
    public class PaymentEntity
    {
        public string AccountId { get; set; }
        public string Ukprn { get; set; }
        public long Uln { get; set; }
        public string ReportedPeriodCode { get; set; }
        public string DeliveryPeriod { get; set; }
        public decimal Amount { get; set; }
        public int TransactionType { get; set; }
        public int FundingType { get; set; }
    }
}
