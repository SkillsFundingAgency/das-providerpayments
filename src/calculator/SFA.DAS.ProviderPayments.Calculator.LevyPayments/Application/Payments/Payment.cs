namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Payments
{
    public class Payment
    {
        public string LearnerRefNumber { get; set; }
        public int AimSequenceNumber { get; set; }
        public long Ukprn { get; set; }

        public string CommitmentId { get; set; }

        public FundingSource Source { get; set; }
        public decimal Amount { get; set; }
    }
}
