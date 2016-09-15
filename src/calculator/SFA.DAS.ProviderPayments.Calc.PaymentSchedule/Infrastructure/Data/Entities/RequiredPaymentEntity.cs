namespace SFA.DAS.ProviderPayments.Calc.PaymentSchedule.Infrastructure.Data.Entities
{
    public class RequiredPaymentEntity
    {
        public string CommitmentId { get; set; }
        public string LearnRefNumber { get; set; }
        public int AimSeqNumber { get; set; }
        public long Ukprn { get; set; }
        public int DeliveryMonth { get; set; }
        public int DeliveryYear { get; set; }
        public int TransactionType { get; set; }
        public decimal AmountDue { get; set; }
    }
}