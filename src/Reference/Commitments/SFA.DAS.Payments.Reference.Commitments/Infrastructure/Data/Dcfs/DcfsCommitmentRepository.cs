using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.Payments.Reference.Commitments.Infrastructure.Data.Entities;
using System;

namespace SFA.DAS.Payments.Reference.Commitments.Infrastructure.Data.Dcfs
{
    public class DcfsCommitmentRepository : DcfsRepository, ICommitmentRepository
    {
        private const string Source = "Reference.DasCommitments ";
        private const string SourceHistory = "dbo.DasCommitmentsHistory ";
        private const string Columns = "CommitmentId, "
                                     + "VersionId,"
                                     + "Uln, "
                                     + "Ukprn, "
                                     + "AccountId, "
                                     + "StartDate, "
                                     + "EndDate, "
                                     + "AgreedCost, "
                                     + "StandardCode, "
                                     + "ProgrammeType, "
                                     + "FrameworkCode, "
                                     + "PathwayCode, "
                                     + "Priority, "
                                     + "PaymentStatus, "
                                     + "PaymentStatusDescription, "
                                     + "EffectiveFromDate, "
                                     + "EffectiveToDate, "
                                     + "LegalEntityName";

        private const string SingleCommitmentClause = " WHERE CommitmentId = @commitmentId ";
        private const string VerionWhereClause = " VersionId = @VersionId ";
        private const string OrderByClause = " ORDER BY VersionId DESC ";
        private const string SelectByIdCommand = "SELECT TOP 1 " + Columns + " FROM " + Source + " " + SingleCommitmentClause + " " + OrderByClause;
        private const string InsertCommand = "INSERT INTO " + Source + " (" + Columns + ") VALUES ("
                                           + "@CommitmentId,@VersionId,@Uln,@Ukprn,@AccountId,@StartDate,@EndDate,"
                                           + "@AgreedCost,@StandardCode,@ProgrammeType,@FrameworkCode,@PathwayCode,"
                                           + "@Priority,@PaymentStatus,@PaymentStatusDescription,@EffectiveFromDate,@EffectiveToDate,@LegalEntityName)";
        private const string UpdateCommand = "UPDATE " + Source + " SET "
                                          + "CommitmentId = @CommitmentId, "
                                          + "Uln = @Uln, "
                                          + "Ukprn = @Ukprn, "
                                          + "AccountId = @AccountId, "
                                          + "StartDate = @StartDate, "
                                          + "EndDate = @EndDate, "
                                          + "AgreedCost = @AgreedCost, "
                                          + "StandardCode = @StandardCode, "
                                          + "ProgrammeType = @ProgrammeType, "
                                          + "FrameworkCode = @FrameworkCode, "
                                          + "PathwayCode = @PathwayCode, "
                                          + "Priority = @Priority, "
                                          + "VersionId = @VersionId, "
                                          + "PaymentStatus = @PaymentStatus, "
                                          + "PaymentStatusDescription = @PaymentStatusDescription, "
                                          + "EffectiveFromDate = @EffectiveFromDate, "
                                          + "EffectiveToDate = @EffectiveToDate, "
                                          + "LegalEntityName = @LegalEntityName "
                                          + SingleCommitmentClause + " AND "
                                          + VerionWhereClause;

        private const string GetByAttributesCommand = "SELECT CommitmentId from " + Source + " WHERE "
                                                    +"  CommitmentId = @CommitmentId AND "
                                                    + "Uln = @Uln AND "
                                                    + "Ukprn = @Ukprn AND "
                                                    + "AccountId = @AccountId AND "
                                                    + "StartDate = @StartDate AND "
                                                    + "EndDate = @EndDate AND "
                                                    + "AgreedCost = @AgreedCost AND "
                                                    + "IsNull(StandardCode,0) = @StandardCode AND "
                                                    + "IsNull(ProgrammeType,0) = @ProgrammeType AND "
                                                    + "IsNull(FrameworkCode,0) = @FrameworkCode AND "
                                                    + "IsNull(PathwayCode,0) = @PathwayCode AND "
                                                    + "Priority = @Priority AND "
                                                    + "PaymentStatus = @PaymentStatus AND "
                                                    + "EffectiveFromDate = @EffectiveFromDate AND "
                                                    + "LegalEntityName = @LegalEntityName AND ";

        private const string DeleteCommand = "DELETE FROM " + Source + SingleCommitmentClause;

        public DcfsCommitmentRepository(string connectionString)
           : base(connectionString)
        {
        }

        public DcfsCommitmentRepository(ContextWrapper context)
            : base(context.GetPropertyValue(ContextPropertyKeys.TransientDatabaseConnectionString))
        {
        }

        public bool CommitmentExists(CommitmentEntity commitment)
        {
            var effetiveToClause = commitment.EffectiveToDate == null ? " EffectiveToDate Is Null " : $" EffectiveToDate = '{commitment.EffectiveToDate.Value.ToString("yyyy-MM-dd")}' ";
            var selectCommand = GetByAttributesCommand + effetiveToClause;

            var result = QuerySingle<int>(selectCommand, commitment);
            return result == 0 ? false : true;
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
    }
}
