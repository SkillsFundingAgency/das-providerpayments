namespace ProviderPayments.TestStack.Domain.Data.Entities
{
    public class PaymentReportEntity
    {
        public long? CommitmentId { get; set; }
        public int Priority { get; set; }
        public long Ukprn { get; set; }
        public string ProviderName { get; set; }
        public long Uln { get; set; }
        public string LearnerName { get; set; }
        public string CollectionPeriodName { get; set; }
        public int TransactionType { get; set; }
        public int FundingSource { get; set; }
        public decimal Amount { get; set; }
        public int DeliveryMonth { get; set; }
        public int DeliveryYear { get; set; }
        public long? AccountId { get; set; }
    }
}