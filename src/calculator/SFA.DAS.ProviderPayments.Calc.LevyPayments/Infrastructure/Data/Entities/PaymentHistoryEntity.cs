using System;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Entities
{
    public class PaymentHistoryEntity
    {
    

        public Guid RequiredPaymentId { get; set; }

        public int DeliveryMonth { get; set; }
        public int DeliveryYear { get; set; }
        public long CommitmentId { get; set; }
        public int TransactionType { get; set; }
        public decimal Amount { get; set; }
    }
}
