using SFA.DAS.Payments.DCFS.Domain;
using System;


namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Payments
{
    public class PaymentHistory
    {
        public Guid RequiredPaymentId { get; set; }
        public long CommitmentId { get; set; }
        public int DeliveryMonth { get; set; }
        public int DeliveryYear { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }
    }
}
