using System;
using System.Collections.Generic;

namespace SFA.DAS.Payments.Automation.IlrBuilder
{
    public class LearningDelivery
    {
        public const string OnProgrammeAimRef = "ZPROG001";

        public string AimRef { get; set; } = OnProgrammeAimRef;
        public AimType AimType { get; set; } = AimType.OnProgramme;
        public int AimSequenceNumber { get; set; } = 0;
        public DateTime LearningStartDate { get; set; }
        public DateTime PlannedLearningEndDate { get; set; }
        public DateTime? ActualLearningEndDate { get; set; }
        public int FundingModel { get; set; } = 36;
        public long? StandardCode { get; set; }
        public int? ProgrammeType { get; set; }
        public int? FrameworkCode { get; set; }
        public int? PathwayCode { get; set; }
        public CompletionStatus CompletionStatus { get; set; } = CompletionStatus.Continuing;
        public List<FundingAndMonitoringCode> FundingAndMonitoringCodes { get; set; } = new List<FundingAndMonitoringCode>();
        public List<FinancialRecord> FinancialRecords { get; set; } = new List<FinancialRecord>();
    }
}