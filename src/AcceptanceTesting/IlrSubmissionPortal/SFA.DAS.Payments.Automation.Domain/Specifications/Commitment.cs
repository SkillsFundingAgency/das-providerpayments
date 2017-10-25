using System;

namespace SFA.DAS.Payments.Automation.Domain.Specifications
{
    public class Commitment
    {
        public virtual long CommitmentId { get; set; }
        public virtual int VersionId { get; set; }
        public virtual string EmployerKey { get; set; }
        public virtual string LearnerKey { get; set; }
        public virtual int Priority { get; set; }
        public virtual string ProviderKey { get; set; }
        public virtual int AgreedPrice { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }
        public virtual DateTime? EffectiveFrom { get; set; }
        public virtual DateTime? EffectiveTo { get; set; }
        public virtual CommitmentPaymentStatus Status { get; set; }
        public virtual long? StandardCode { get; set; }
        public virtual int? ProgrammeType { get; set; }
        public virtual int? FrameworkCode { get; set; }
        public virtual int? PathwayCode { get; set; }


    }

    public enum CommitmentPaymentStatus
    {
        PendingApproval = 0,
        Active = 1,
        Paused = 2,
        Cancelled = 3,
        Completed = 4,
        Deleted = 5
    }
}