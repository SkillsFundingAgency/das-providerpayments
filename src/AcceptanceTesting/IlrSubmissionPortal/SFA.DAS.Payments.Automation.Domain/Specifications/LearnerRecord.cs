using System;

namespace SFA.DAS.Payments.Automation.Domain.Specifications
{
    public class LearnerRecord
    {
        public virtual string LearnerKey { get; set; }
        public virtual LearnerType LearnerType { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime PlannedEndDate { get; set; }
        public virtual DateTime? ActualEndDate { get; set; }
        public virtual CompletionStatus CompletionStatus { get; set; }
        public virtual string ProviderKey { get; set; }
        public virtual int TotalTrainingPrice1 { get; set; }
        public virtual DateTime TotalTrainingPrice1EffectiveDate { get; set; }
        public virtual int? TotalAssessmentPrice1 { get; set; }
        public virtual DateTime? TotalAssessmentPrice1EffectiveDate { get; set; }
        public virtual int? ResidualTrainingPrice1 { get; set; }
        public virtual DateTime? ResidualTrainingPrice1EffectiveDate { get; set; }
        public virtual int? ResidualAssessmentPrice1 { get; set; }
        public virtual DateTime? ResidualAssessmentPrice1EffectiveDate { get; set; }
        public virtual int? TotalTrainingPrice2 { get; set; }
        public virtual DateTime? TotalTrainingPrice2EffectiveDate { get; set; }
        public virtual int? TotalAssessmentPrice2 { get; set; }
        public virtual DateTime? TotalAssessmentPrice2EffectiveDate { get; set; }
        public virtual AimType AimType { get; set; }
        public virtual string AimRate { get; set; }
        public virtual long? StandardCode { get; set; }
        public virtual int? FrameworkCode { get; set; }
        public virtual int? ProgrammeType { get; set; }
        public virtual int? PathwayCode { get; set; }
        public virtual string HomePostcodeDeprivation { get; set; }
        public virtual string EmploymentStatus { get; set; }
        public virtual string EmploymentStatusApplies { get; set; }
        public virtual string EmployerKey { get; set; }
        public virtual string SmallEmployer { get; set; }
        public virtual string LearnDelFam { get; set; }
        public virtual int? ResidualTrainingPrice2 { get; set; }
        public virtual DateTime? ResidualTrainingPrice2EffectiveDate { get; set; }
        public virtual int? ResidualAssessmentPrice2 { get; set; }
        public virtual DateTime? ResidualAssessmentPrice2EffectiveDate { get; set; }

        public virtual int AimSequenceNumber { get; set; }
        public virtual string LearnerReferenceNumber { get; set; }
    }
}
