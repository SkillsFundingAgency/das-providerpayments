namespace SFA.DAS.CollectionEarnings.Calculator.Application.ApprenticeshipPriceEpisodePeriod
{
    public class LearningDeliveryPeriodEarning
    {
        public string LearnerReferenceNumber { get; set; }
        public int AimSequenceNumber { get; set; }
        public int Period { get; set; }
        public decimal OnProgrammeEarning { get; set; }
        public decimal CompletionEarning { get; set; }
        public decimal BalancingEarning { get; set; }
    }
}