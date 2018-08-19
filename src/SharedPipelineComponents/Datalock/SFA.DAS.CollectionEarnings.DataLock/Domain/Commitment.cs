using System;
using System.Diagnostics;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Domain
{
    [DebuggerDisplay("Effective Start: {EffectiveStartDate} -> Effective End Date: {EffectiveEndDate}")]
    public class Commitment : CommitmentEntity
    {
        public Commitment(CommitmentEntity entity)
        {
            CommitmentId = entity.CommitmentId;
            VersionId = entity.VersionId;
            Uln = entity.Uln;
            Ukprn = entity.Ukprn;
            AccountId = entity.AccountId;
            StartDate = entity.StartDate;
            EndDate = entity.EndDate;
            AgreedCost = entity.AgreedCost;
            StandardCode = entity.StandardCode;
            ProgrammeType = entity.ProgrammeType;
            FrameworkCode = entity.FrameworkCode;
            PathwayCode = entity.PathwayCode;
            PaymentStatus = entity.PaymentStatus;
            PaymentStatusDescription = entity.PaymentStatusDescription;
            Priority = entity.Priority;
            EffectiveFrom = entity.EffectiveFrom;
            EffectiveTo = entity.EffectiveTo;
            TransferSendingEmployerAccountId = entity.TransferSendingEmployerAccountId;
            TransferApprovalDate = entity.TransferApprovalDate;
            WithdrawnOnDate = entity.WithdrawnOnDate;
            PausedOnDate = entity.PausedOnDate;
        }

        public DateTime EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public bool IsVersioned { get; set; }
    }
}

