using System.Collections.Generic;

namespace SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Helpers
{
    internal static class DasAccountDataHelper
    {
        internal static IEnumerable<DasAccountEntity> GetAll( )
        {
            const string sql = @"
            select *
            from Reference.DasAccounts;
            ";

            return TestDataHelper.Query<DasAccountEntity>(sql);
        }

        internal static void Create(DasAccountEntity dasAccount)
        {
            const string sql = @"
            INSERT INTO Reference.DasAccounts (
                [AccountId],
		        [AccountHashId],
                [AccountName],
                [Balance],
		        [VersionId],
		        [IsLevyPayer],
		        [TransferAllowance]
            ) VALUES (
                @AccountId,
		        @AccountHashId,
                @AccountName,
                @Balance,
		        @VersionId,
		        @IsLevyPayer,
		        @TransferAllowance
            );";

            TestDataHelper.Execute(sql, dasAccount);
        }

        internal static void Truncate()
        {
            const string sql = "TRUNCATE TABLE Reference.DasAccounts";
            TestDataHelper.Execute(sql);
        }
    }
}