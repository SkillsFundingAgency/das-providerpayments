using SFA.DAS.Payments.DCFS.Domain;
using System;


namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Payments
{
    public class PaymentDue
    {
        public Guid Id { get; set; }
        public long CommitmentId { get; set; }
        public string LearnerRefNumber { get; set; }
        public int AimSequenceNumber { get; set; }
        public long Ukprn { get; set; }
        public int DeliveryMonth { get; set; }
        public int DeliveryYear { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal AmountDue { get; set; }
    }
}