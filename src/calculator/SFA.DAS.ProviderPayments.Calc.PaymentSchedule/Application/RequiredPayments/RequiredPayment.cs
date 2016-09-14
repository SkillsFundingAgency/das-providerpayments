namespace SFA.DAS.ProviderPayments.Calc.PaymentSchedule.Application.RequiredPayments
{
    public class RequiredPayment
    {
        public string LearnerRefNumber { get; set; }
        public int AimSequenceNumber { get; set; }
        public long Ukprn { get; set; }
        public int DeliveryMonth { get; set; }
        public int DeliveryYear { get; set; }
        public int TransactionType { get; set; }
        public decimal AmountDue { get; set; }
    }
}