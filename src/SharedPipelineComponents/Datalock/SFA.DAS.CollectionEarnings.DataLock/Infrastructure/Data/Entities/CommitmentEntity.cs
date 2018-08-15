using System;

namespace SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities
{
    public class CommitmentEntity
    {
        public long CommitmentId { get; set; }
        public string VersionId { get; set; }

        public long Uln { get; set; }
        public long Ukprn { get; set; }
        public long AccountId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal AgreedCost { get; set; }
        public long? StandardCode { get; set; }
        public int? ProgrammeType { get; set; }
        public int? FrameworkCode { get; set; }
        public int? PathwayCode { get; set; }

        public int PaymentStatus { get; set; }
        public string PaymentStatusDescription { get; set; }

        public int Priority { get; set; }

        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }

        public long? TransferSendingEmployerAccountId { get; set; }

        public DateTime? TransferApprovalDate { get; set; }

        public DateTime? WithdrawnOnDate { get; set; }
        public DateTime? PausedOnDate { get; set; }

        public CommitmentEntity Clone()
        {
            return (CommitmentEntity)MemberwiseClone();
        }
    }
}