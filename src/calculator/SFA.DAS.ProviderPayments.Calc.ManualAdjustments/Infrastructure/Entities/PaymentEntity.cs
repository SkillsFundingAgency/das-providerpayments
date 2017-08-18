using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Infrastructure.Entities
{
    public class PaymentEntity
    {
        public string PaymentId { get; set; }
        public Guid RequiredPaymentId { get; set; }
        public int DeliveryMonth { get; set; }
        public int DeliveryYear { get; set; }
        public string CollectionPeriodName { get; set; }
        public int CollectionPeriodMonth { get; set; }
        public int CollectionPeriodYear { get; set; }
        public int FundingSource { get; set; }
        public int TransactionType { get; set; }
        public decimal Amount { get; set; }
        public long? CommitmentId { get; set; }
    }
}
