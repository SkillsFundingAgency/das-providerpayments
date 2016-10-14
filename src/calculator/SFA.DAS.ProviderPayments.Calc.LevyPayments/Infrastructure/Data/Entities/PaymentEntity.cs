using System;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Entities
{
    public class PaymentEntity
    {
        public string Id { get; set; }

        public Guid RequiredPaymentId { get; set; }

        public string LearnerRefNumber { get; set; }
        public int AimSequenceNumber { get; set; }
        public long Ukprn { get; set; }

        public long CommitmentId { get; set; }

        public int DeliveryMonth { get; set; }
        public int DeliveryYear { get; set; }
        public int CollectionPeriodMonth { get; set; }
        public int CollectionPeriodYear { get; set; }

        public int FundingSource { get; set; }
        public int TransactionType { get; set; }
        public decimal Amount { get; set; }
    }
}
