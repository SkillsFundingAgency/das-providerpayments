namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Entities
{
    public class PaymentEntity
    {
        public string Id { get; set; }

        public string LearnerRefNumber { get; set; }
        public int AimSequenceNumber { get; set; }
        public long Ukprn { get; set; }

        public string CommitmentId { get; set; }

        public int Source { get; set; }
        public int TransactionType { get; set; }
        public decimal Amount { get; set; }
    }
}
