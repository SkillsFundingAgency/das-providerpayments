using SFA.DAS.Payments.Reference.Accounts.Infrastructure.Data.Entities;

namespace SFA.DAS.Payments.Reference.Accounts.IntegrationTests.DataHelpers
{
    internal static class AccountDataHelper
    {
        internal static void AddAccount(long id, string hashId, string name, decimal balance, string versionId, bool isLevyPayer)
        {
            DatabaseHelper.Execute("INSERT INTO DasAccounts VALUES (@id,@hashId,@name,@balance,@versionId,@IsLevyPayer)",
                new { id, hashId, name, balance, versionId , isLevyPayer});
        }

        internal static AccountEntity GetAccountById(long id)
        {
            return DatabaseHelper.QuerySingle<AccountEntity>("SELECT * FROM DasAccounts WHERE AccountId=@id", new { id });
        }
    }
}
