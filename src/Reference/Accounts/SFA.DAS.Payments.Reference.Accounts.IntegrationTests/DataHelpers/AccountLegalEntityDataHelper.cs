using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Payments.Reference.Accounts.Infrastructure.Data.Entities;

namespace SFA.DAS.Payments.Reference.Accounts.IntegrationTests.DataHelpers
{
    public static class AccountLegalEntityDataHelper
    {
        internal static void Truncate()
        {
            const string sql = "TRUNCATE TABLE dbo.AccountLegalEntity";
            DatabaseHelper.Execute(sql);
        }

        internal static List<AccountLegalEntityEntity> GetAll()
        {
            const string sql = @"
            SELECT *
            FROM dbo.AccountLegalEntity";

            return DatabaseHelper
                .Query<AccountLegalEntityEntity>(sql)
                .ToList();
        }
    }
}