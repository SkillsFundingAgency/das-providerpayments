using System;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.Payments.Reference.Commitments.Infrastructure.Data.Entities;

namespace SFA.DAS.Payments.Reference.Commitments.Infrastructure.Data.Dcfs
{
    public class DcfsCommitmentRepository : DcfsRepository, ICommitmentRepository
    {
        private const string Source = "dbo.DasCommitments ";
        private const string SourceHistory = "dbo.DasCommitmentsHistory ";

        private const string SelectByIdCommand = @" SELECT TOP 1 
                                                        CommitmentId, 
                                                        VersionId,
                                                        Uln,
                                                        Ukprn,
                                                        AccountId,
                                                        StartDate,
                                                        EndDate,
                                                        AgreedCost,
                                                        StandardCode,
                                                        ProgrammeType,
                                                        FrameworkCode,
                                                        PathwayCode,
                                                        Priority,
                                                        PaymentStatus,
                                                        PaymentStatusDescription,
                                                        PausedOnDate,
                                                        WithdrawnOnDate,
                                                        EffectiveFromDate,
                                                        EffectiveToDate,
                                                        LegalEntityName,
                                                        TransferSendingEmployerAccountId,
                                                        TransferApprovalDate,
                                                        AccountLegalEntityPublicHashedId
                                                    FROM dbo.DasCommitments 
                                                    WHERE CommitmentId = @commitmentId 
                                                    ORDER BY VersionId DESC ";
        private const string InsertCommand = @"INSERT INTO dbo.DasCommitments 
                                                (
                                                    CommitmentId, 
                                                    VersionId,
                                                    Uln,
                                                    Ukprn,
                                                    AccountId,
                                                    StartDate,
                                                    EndDate,
                                                    AgreedCost,
                                                    StandardCode,
                                                    ProgrammeType,
                                                    FrameworkCode,
                                                    PathwayCode,
                                                    Priority,
                                                    PaymentStatus,
                                                    PaymentStatusDescription,
                                                    PausedOnDate,
                                                    WithdrawnOnDate,
                                                    EffectiveFromDate,
                                                    EffectiveToDate,
                                                    LegalEntityName,
                                                    TransferSendingEmployerAccountId,
                                                    TransferApprovalDate,
                                                    AccountLegalEntityPublicHashedId
                                                )
                                                VALUES
                                                (
                                                    @CommitmentId,
                                                    @VersionId,
                                                    @Uln,
                                                    @Ukprn,
                                                    @AccountId,
                                                    @StartDate,
                                                    @EndDate,
                                                    @AgreedCost,
                                                    @StandardCode,
                                                    @ProgrammeType,
                                                    @FrameworkCode,
                                                    @PathwayCode,
                                                    @Priority,
                                                    @PaymentStatus,
                                                    @PaymentStatusDescription,
                                                    @PausedOnDate,
                                                    @WithdrawnOnDate,
                                                    @EffectiveFromDate,
                                                    @EffectiveToDate,
                                                    @LegalEntityName,
                                                    @TransferSendingEmployerAccountId,
                                                    @TransferApprovalDate,
                                                    @AccountLegalEntityPublicHashedId
                                                )";

        private const string UpdateCommand = @" UPDATE dbo.DasCommitments
                                                SET CommitmentId = @CommitmentId,
                                                    Uln = @Uln,
                                                    Ukprn = @Ukprn,
                                                    AccountId = @AccountId,
                                                    StartDate = @StartDate,
                                                    EndDate = @EndDate,
                                                    AgreedCost = @AgreedCost,
                                                    StandardCode = @StandardCode,
                                                    ProgrammeType = @ProgrammeType,
                                                    FrameworkCode = @FrameworkCode,
                                                    PathwayCode = @PathwayCode,
                                                    Priority = @Priority,
                                                    VersionId = @VersionId,
                                                    PaymentStatus = @PaymentStatus,
                                                    PaymentStatusDescription = @PaymentStatusDescription,
                                                    PausedOnDate = @PausedOnDate,
                                                    WithdrawnOnDate = @WithdrawnOnDate,
                                                    EffectiveFromDate = @EffectiveFromDate,
                                                    EffectiveToDate = @EffectiveToDate,
                                                    LegalEntityName = @LegalEntityName,
                                                    TransferSendingEmployerAccountId = @TransferSendingEmployerAccountId,
                                                    TransferApprovalDate = @TransferApprovalDate,
,                                                   AccountLegalEntityPublicHashedId = @AccountLegalEntityPublicHashedId
                                                WHERE CommitmentId = @commitmentId  
                                                AND VersionId = @VersionId ";

        private const string DeleteCommand = "DELETE FROM dbo.DasCommitments WHERE CommitmentId = @commitmentId";

        public DcfsCommitmentRepository(string connectionString)
           : base(connectionString)
        {
        }

        public DcfsCommitmentRepository(ContextWrapper context)
            : base(context.GetPropertyValue(ContextPropertyKeys.TransientDatabaseConnectionString))
        {
        }

        public bool CommitmentExistsAndDetailsAreIdentical(CommitmentEntity commitment)
        {
            var selectCommand = @"SELECT CommitmentId 
                                                        FROM dbo.DasCommitments 
                                                        WHERE  CommitmentId = @CommitmentId 
                                                        AND Uln = @Uln 
                                                        AND Ukprn = @Ukprn
                                                        AND AccountId = @AccountId
                                                        AND StartDate = @StartDate
                                                        AND EndDate = @EndDate 
                                                        AND AgreedCost = @AgreedCost
                                                        AND IsNull(StandardCode,0) = @StandardCode 
                                                        AND IsNull(ProgrammeType,0) = @ProgrammeType
                                                        AND IsNull(FrameworkCode,0) = @FrameworkCode
                                                        AND IsNull(PathwayCode,0) = @PathwayCode
                                                        AND Priority = @Priority
                                                        AND PaymentStatus = @PaymentStatus
                                                        AND EffectiveFromDate = @EffectiveFromDate
                                                        AND LegalEntityName = @LegalEntityName 
                                                        AND ISNULL(TransferSendingEmployerAccountId, 0) = ISNULL(@TransferSendingEmployerAccountId, 0)
                                                        AND " + CreateCompareDateSqlClause("TransferApprovalDate", commitment.TransferApprovalDate) + 
                                                        "AND " + CreateCompareDateSqlClause("EffectiveToDate", commitment.EffectiveToDate) +
                                                        "AND " + CreateCompareDateSqlClause("PausedOnDate", commitment.PausedOnDate) +
                                                        "AND " + CreateCompareDateSqlClause("WithdrawnOnDate", commitment.WithdrawnOnDate) +
                                                        "AND AccountLegalEntityPublicHashedId = @AccountLegalEntityPublicHashedId";

            var result = QuerySingle<int>(selectCommand, commitment);
            return result != 0;
        }

        public CommitmentEntity GetById(long commitmentId)
        {
            return QuerySingle<CommitmentEntity>(SelectByIdCommand, new { commitmentId });
        }

        public void Insert(CommitmentEntity commitment)
        {
            Execute(InsertCommand, commitment);
        }

        public void InsertHistory(CommitmentEntity commitment)
        {
            Execute(InsertCommand.Replace(Source,SourceHistory), commitment);
        }
        
        public void Update(CommitmentEntity commitment)
        {
            Execute(UpdateCommand, commitment);
        }

        public void Delete(long commitmentId)
        {
            Execute(DeleteCommand, new { commitmentId });
        }

        private static string CreateCompareDateSqlClause(string fieldname, DateTime? date) =>
            date == null ? $" {fieldname} Is Null " : $" {fieldname} = '{date.Value.ToString("yyyy-MM-dd")}' ";

    }
}
