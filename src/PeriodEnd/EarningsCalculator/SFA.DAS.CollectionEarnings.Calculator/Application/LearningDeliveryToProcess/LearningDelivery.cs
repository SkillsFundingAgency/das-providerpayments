using System;

namespace SFA.DAS.CollectionEarnings.Calculator.Application.LearningDeliveryToProcess
{
    public class LearningDelivery
    {
        public int Ukprn { get; set; }
        public string LearnerReferenceNumber { get; set; }
        public long Uln { get; set; }
        public string NiNumber { get; set; }
        public int AimSequenceNumber { get; set; }
        public long? StandardCode { get; set; }
        public int? ProgrammeType { get; set; }
        public int? FrameworkCode { get; set; }
        public int? PathwayCode { get; set; }
        public DateTime LearningStartDate { get; set; }
        public DateTime? OriginalLearningStartDate { get; set; }
        public DateTime LearningPlannedEndDate { get; set; }
        public DateTime? LearningActualEndDate { get; set; }
        public int? CompletionStatus { get; set; }
        public PriceEpisode[] PriceEpisodes { get; set; }
    }
}