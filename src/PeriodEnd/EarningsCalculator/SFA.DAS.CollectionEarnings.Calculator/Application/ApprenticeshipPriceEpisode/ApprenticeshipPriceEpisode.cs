using System;

namespace SFA.DAS.CollectionEarnings.Calculator.Application.ApprenticeshipPriceEpisode
{
    public class ApprenticeshipPriceEpisode
    {
        public string Id { get; set; }
        public string LearnerReferenceNumber { get; set; }
        public int AimSequenceNumber { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? PlannedEndDate { get; set; }
        public DateTime? ActualEndDate { get; set; }

        public decimal NegotiatedPrice { get; set; }
        public int? Tnp1 { get; set; }
        public int? Tnp2 { get; set; }
        public int? Tnp3 { get; set; }
        public int? Tnp4 { get; set; }

        public decimal MonthlyAmount { get; set; }
        public decimal? CompletionAmount { get; set; }
        public decimal? BalancingAmount { get; set; }

        public bool? Completed { get; set; }

        public bool LastEpisode { get; set; }
    }
}