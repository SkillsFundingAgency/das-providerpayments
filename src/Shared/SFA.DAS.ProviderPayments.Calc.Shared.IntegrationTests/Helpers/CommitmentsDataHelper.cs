using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Helpers
{
    public static class CommitmentsDataHelper
    {
        internal static IEnumerable<CommitmentEntity> GetAll( )
        {
            const string sql = @"
            select *
            from Reference.DasCommitments;
            ";

            return TestDataHelper.Query<CommitmentEntity>(sql);
        }

        internal static void Create(CommitmentEntity commitment)
        {
            const string sql = @"
            INSERT INTO Reference.DasCommitments (
                [CommitmentId],
		        [VersionId],
                [Uln],
                [Ukprn],
		        [AccountId],
		        [StartDate],
                [EndDate],
                [AgreedCost],
                [StandardCode],
                [ProgrammeType],
                [FrameworkCode],
                [PathwayCode],
                [PaymentStatus],
                [PaymentStatusDescription],
                [Priority],
	            [EffectiveFrom],
	            [EffectiveTo],
	            [TransferSendingEmployerAccountId],
	            [TransferApprovalDate] 
            ) VALUES (
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
                @PaymentStatus,
                @PaymentStatusDescription,
                @Priority,
	            @EffectiveFrom,
	            @EffectiveTo,
	            @TransferSendingEmployerAccountId,
	            @TransferApprovalDate 
            );";

            TestDataHelper.Execute(sql, commitment);
        }

        internal static void Truncate()
        {
            const string sql = "TRUNCATE TABLE Reference.DasCommitments";
            TestDataHelper.Execute(sql);
        }
    }
}