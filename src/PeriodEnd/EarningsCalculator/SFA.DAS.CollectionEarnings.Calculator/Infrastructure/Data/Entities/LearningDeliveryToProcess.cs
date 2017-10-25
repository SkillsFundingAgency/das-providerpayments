using System;

namespace SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Entities
{
    public class LearningDeliveryToProcess
    {
        public int Ukprn { get; set; }
        public string LearnRefNumber { get; set; }
        public long Uln { get; set; }
        public string NiNumber { get; set; }
        public int AimSeqNumber { get; set; }
        public long? StandardCode { get; set; }
        public int? ProgrammeType { get; set; }
        public int? FrameworkCode { get; set; }
        public int? PathwayCode { get; set; }
        public DateTime LearnStartDate { get; set; }
        public DateTime? OrigLearnStartDate { get; set; }
        public DateTime LearnPlanEndDate { get; set; }
        public DateTime? LearnActEndDate { get; set; }
        public int? CompletionStatus { get; set; }
    }
}