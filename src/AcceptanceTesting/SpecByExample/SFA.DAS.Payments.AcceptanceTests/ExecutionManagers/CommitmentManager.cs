using System.Data.SqlClient;
using Dapper;
using SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels;

namespace SFA.DAS.Payments.AcceptanceTests.ExecutionManagers
{
    internal static class CommitmentManager
    {
        internal static void AddCommitment(CommitmentReferenceData commitment)
        {
            if(TestEnvironment.ValidateSpecsOnly)
            {
                return;
            }

            using (var connection = new SqlConnection(TestEnvironment.Variables.DedsDatabaseConnectionString))
            {
                connection.Execute("INSERT INTO dbo.DasCommitments " +
                                   "(CommitmentId, VersionId, Uln, Ukprn, AccountId, StartDate, EndDate, AgreedCost, " +
                                   "StandardCode, ProgrammeType, FrameworkCode, PathwayCode, PaymentStatus, PaymentStatusDescription, " +
                                   "Priority, EffectiveFromDate, EffectiveToDate, TransferSendingEmployerAccountId, " +
                                   "TransferApprovalDate, WithdrawnOnDate, PausedOnDate) " +
                                   "VALUES" +
                                   "(@CommitmentId, @VersionId, @Uln, @Ukprn, @AccountId, @StartDate, @EndDate, " +
                                   "@AgreedCost, @StandardCode, @ProgrammeType, @FrameworkCode, @PathwayCode, " +
                                   "@PaymentStatus, @PaymentStatusDescription, @Priority, @EffectiveFromDate, " +
                                   "@EffectiveToDate, @TransferSendingEmployerAccountId, @TransferApprovalDate," +
                                   "@WithdrawnOnDate, @PausedOnDate)",
                                   new
                                   {
                                       commitment.CommitmentId,
                                       commitment.VersionId,
                                       commitment.Uln,
                                       commitment.Ukprn,
                                       AccountId = commitment.EmployerAccountId,
                                       commitment.StartDate,
                                       commitment.EndDate,
                                       AgreedCost = commitment.AgreedPrice,
                                       commitment.StandardCode,
                                       commitment.ProgrammeType,
                                       commitment.FrameworkCode,
                                       commitment.PathwayCode,
                                       PaymentStatus = (int)commitment.Status,
                                       PaymentStatusDescription = commitment.Status.ToString(),
                                       commitment.Priority,
                                       EffectiveFromDate = commitment.EffectiveFrom,
                                       EffectiveToDate = commitment.EffectiveTo,
                                       commitment.TransferSendingEmployerAccountId,
                                       commitment.TransferApprovalDate,
                                       commitment.WithdrawnOnDate,
                                       commitment.PausedOnDate,
                                   });
            }
        }
        internal static void DeleteCommitments()
        {
            if (TestEnvironment.ValidateSpecsOnly)
            {
                return;
            }

            using (var connection = new SqlConnection(TestEnvironment.Variables.DedsDatabaseConnectionString))
            {
                connection.Execute("TRUNCATE TABLE dbo.DasCommitments");
            }
        }
    }
}
