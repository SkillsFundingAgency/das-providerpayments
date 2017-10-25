namespace ProviderPayments.TestStack.Domain
{
    public class PaymentReport
    {
        public long? CommitmentId { get; set; }
        public int Priority { get; set; }
        public long Ukprn { get; set; }
        public string ProviderName { get; set; }
        public long Uln { get; set; }
        public string LearnerName { get; set; }
        public string CollectionPeriodName { get; set; }
        public TransactionType TransactionType { get; set; }
        public FundingSource FundingSource { get; set; }
        public decimal Amount { get; set; }
        public int DeliveryMonth { get; set; }
        public int DeliveryYear { get; set; }
        public long? AccountId { get; set; }
    }
}