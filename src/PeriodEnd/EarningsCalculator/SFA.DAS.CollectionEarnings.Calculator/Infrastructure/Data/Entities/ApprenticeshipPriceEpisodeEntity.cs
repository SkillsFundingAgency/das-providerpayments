// ReSharper disable InconsistentNaming
using System;

namespace SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Entities
{
    public class ApprenticeshipPriceEpisodeEntity
    {
        public string LearnRefNumber { get; set; }
        public string PriceEpisodeIdentifier { get; set; }
        public DateTime EpisodeEffectiveTNPStartDate { get; set; }
        public DateTime EpisodeStartDate { get; set; }
        public DateTime? PriceEpisodeActualEndDate { get; set; }
        public int? PriceEpisodeActualInstalments { get; set; }
        public int PriceEpisodeAimSeqNumber { get; set; }
        public decimal? PriceEpisodeCappedRemainingTNPAmount { get; set; }
        public bool? PriceEpisodeCompleted { get; set; }
        public decimal? PriceEpisodeCompletionElement { get; set; }
        public decimal? PriceEpisodeExpectedTotalMonthlyValue { get; set; }
        public decimal? PriceEpisodeInstalmentValue { get; set; }
        public DateTime? PriceEpisodePlannedEndDate { get; set; }
        public int? PriceEpisodePlannedInstalments { get; set; }
        public decimal? PriceEpisodePreviousEarnings { get; set; }
        public decimal? PriceEpisodeRemainingAmountWithinUpperLimit { get; set; }
        public decimal? PriceEpisodeRemainingTNPAmount { get; set; }
        public decimal? PriceEpisodeTotalEarnings { get; set; }
        public decimal? PriceEpisodeTotalTNPPrice { get; set; }
        public decimal? PriceEpisodeUpperBandLimit { get; set; }
        public decimal? PriceEpisodeUpperLimitAdjustment { get; set; }
        public decimal? TNP1 { get; set; }
        public decimal? TNP2 { get; set; }
        public decimal? TNP3 { get; set; }
        public decimal? TNP4 { get; set; }
    }
}