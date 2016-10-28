namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities
{
    public class EarningEntity
    {
        public long CommitmentId { get; set; }
        public string CommitmentVersionId { get; set; }
        public string AccountId { get; set; }
        public string AccountVersionId { get; set; }
        public string LearnerRefNumber { get; set; }
        public int AimSequenceNumber { get; set; }
        public long Ukprn { get; set; }
        public long Uln { get; set; }


        public decimal MonthlyInstallment { get; set; }
        public decimal CompletionPayment { get; set; }


        public decimal Period1 { get; set; }
        public decimal Period2 { get; set; }
        public decimal Period3 { get; set; }
        public decimal Period4 { get; set; }
        public decimal Period5 { get; set; }
        public decimal Period6 { get; set; }
        public decimal Period7 { get; set; }
        public decimal Period8 { get; set; }
        public decimal Period9 { get; set; }
        public decimal Period10 { get; set; }
        public decimal Period11 { get; set; }
        public decimal Period12 { get; set; }
    }
}