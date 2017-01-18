namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities
{
    public class EarningEntity
    {
        public long? CommitmentId { get; set; }
        public string CommitmentVersionId { get; set; }
        public string AccountId { get; set; }
        public string AccountVersionId { get; set; }
        public string LearnerRefNumber { get; set; }
        public int AimSequenceNumber { get; set; }
        public long Ukprn { get; set; }
        public long Uln { get; set; }

        public int Period { get; set; }

        public decimal PriceEpisodeOnProgPayment { get; set; }
        public decimal PriceEpisodeCompletionPayment { get; set; }
        public decimal PriceEpisodeBalancePayment { get; set; }
        public decimal PriceEpisodeFirstEmp1618Pay { get; set; }
        public decimal PriceEpisodeFirstProv1618Pay { get; set; }
        public decimal PriceEpisodeSecondEmp1618Pay { get; set; }
        public decimal PriceEpisodeSecondProv1618Pay { get; set; }

        public long? StandardCode { get; set; }
        public int? ProgrammeType { get; set; }
        public int? FrameworkCode { get; set; }
        public int? PathwayCode { get; set; }

        public int ApprenticeshipContractType { get; set; }
        public string PriceEpisodeIdentifier { get; set; }
    }
}