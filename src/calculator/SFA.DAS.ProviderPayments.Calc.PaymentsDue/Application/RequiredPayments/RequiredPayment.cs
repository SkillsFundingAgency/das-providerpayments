namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments
{
    public class RequiredPayment
    {
        public string CommitmentId { get; set; }
        public string LearnerRefNumber { get; set; }
        public int AimSequenceNumber { get; set; }
        public long Ukprn { get; set; }
        public int DeliveryMonth { get; set; }
        public int DeliveryYear { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal AmountDue { get; set; }
    }
}