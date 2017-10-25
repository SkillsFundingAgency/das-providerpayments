using System;

namespace IlrGenerator
{
    public class LearningDelivery
    {
        public LearningDelivery()
        {
            CompletionStatus = CompletionStatus.Continuing;
        }
        public short ActFamCodeValue { get; set; }
        public DateTime ActualStartDate { get; set; }
        public DateTime PlannedEndDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public decimal TrainingCost { get; set; }
        public decimal EndpointAssesmentCost { get; set; }
        public long StandardCode { get; set; }
        public int PathwayCode { get; set; }
        public int FrameworkCode { get; set; }
        public int ProgrammeType { get; set; }
     
        public CompletionStatus CompletionStatus { get; set; }

        public AimType Type { get; set; }

        public FinancialRecord[] FinancialRecords { get; set; }
        public LearningDeliveryFamRecord[] FamRecords { get; set; }

        public int AimSequenceNumber { get; set; }
        public string LearnAimRef { get; set; }

        public int LearningAdjustmentForPriorLearning { get; set; } = -1;
        public int OtherFundingAdjustments { get; set; } = -1;
    }
}