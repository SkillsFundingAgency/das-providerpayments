using System;
using SFA.DAS.Payments.Reference.Commitments.IntegrationTests.DataHelpers.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Payments.Reference.Commitments.IntegrationTests.DataHelpers
{
    internal static class CommitmentDataHelper
    {
        internal static CommitmentEntity[] GetCommitments()
        {
            return DatabaseHelper.Query<CommitmentEntity>("SELECT * FROM dbo.DasCommitments");
        }

        internal static CommitmentHistoryEntity[] GetCommitmentHistory()
        {
            return DatabaseHelper.Query<CommitmentHistoryEntity>("SELECT * FROM dbo.DasCommitmentsHistory");
        }

        internal static void Clean()
        {
            DatabaseHelper.Execute("DELETE FROM dbo.DasCommitments");
            DatabaseHelper.Execute("DELETE FROM dbo.DasCommitmentsHistory");

            DatabaseHelper.Execute("DELETE FROM dbo.EventStreamPointer");

        }

        internal static void AddCommitment(long id,
                                           long accountId,
                                           long uln,
                                           long ukprn,
                                           DateTime startDate,
                                           DateTime endDate,
                                           decimal agreedCost,
                                           long? standardCode,
                                           int? programmeType,
                                           int? frameworkCode,
                                           int? pathwayCode,
                                           int priority,
                                           int paymentStatus,
                                           string paymentStatusDescription,
                                           string versionId,
                                           DateTime? effectiveFromDate = null,
                                           DateTime? effectiveToDate = null,
                                           string legalEntityName = "ACME Ltd.",
                                           string tableName= "dbo.DasCommitments")
        {
            effectiveFromDate = effectiveFromDate ?? startDate;

            DatabaseHelper.Execute("INSERT INTO " + tableName + 
                    "(CommitmentId,versionId,AccountId,Uln,Ukprn,StartDate,EndDate,AgreedCost,StandardCode,ProgrammeType,FrameworkCode,PathwayCode,PaymentStatus,PaymentStatusDescription,Priority,EffectiveFromDate,EffectiveToDate,LegalEntityName) " +
                    "VALUES " +
                    "(@id,@versionId, @accountId, @uln, @ukprn, @startDate, @endDate, @agreedCost, @standardCode, @programmeType, @frameworkCode, @pathwayCode, @paymentStatus, @paymentStatusDescription, @priority,@EffectiveFromDate,@EffectiveToDate,@LegalEntityName)",
                    new { id, versionId, accountId, uln, ukprn, startDate, endDate, agreedCost, standardCode, programmeType, frameworkCode, pathwayCode, priority, paymentStatus, paymentStatusDescription, effectiveFromDate,effectiveToDate,legalEntityName});
        }

        public static long GetHistoryCommitment(long commitmentId)
        {
            return DatabaseHelper.Query<long>("Select CommitmentId From dbo.DasCommitmentsHistory Where CommitmentId =@CommitmentId  ", new { commitmentId } ).FirstOrDefault();
        }
    }
}
