namespace SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Entities
{
    public class ApprenticeshipPriceEpisodePeriodEntity
    {
        public string LearnRefNumber { get; set; }
        public string PriceEpisodeIdentifier { get; set; }
        public int Period { get; set; }
        public decimal PriceEpisodeBalancePayment { get; set; }
        public decimal PriceEpisodeCompletionPayment { get; set; }
        public int? PriceEpisodeInstalmentsThisPeriod { get; set; }
        public decimal PriceEpisodeOnProgPayment { get; set; }
    }
}