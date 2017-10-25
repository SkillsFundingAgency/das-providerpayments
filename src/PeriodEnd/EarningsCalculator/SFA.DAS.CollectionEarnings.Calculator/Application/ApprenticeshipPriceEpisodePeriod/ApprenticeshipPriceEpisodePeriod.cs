using System;

namespace SFA.DAS.CollectionEarnings.Calculator.Application.ApprenticeshipPriceEpisodePeriod
{
    public class ApprenticeshipPriceEpisodePeriod
    {
        public string PriceEpisodeId { get; set; }
        public string LearnerReferenceNumber { get; set; }
        public int AimSequenceNumber { get; set; }
        public DateTime PriceEpisodeStartDate { get; set; }
        public int Period { get; set; }

        public decimal PriceEpisodeOnProgPayment { get; set; }
        public decimal PriceEpisodeCompletionPayment { get; set; }
        public decimal PriceEpisodeBalancePayment { get; set; }
        public decimal PriceEpisodeInstalmentsThisPeriod { get; set; }
    }
}