using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Helpers
{
    public static class DasAccountDataHelper
    {
        internal static IEnumerable<DasAccountEntity> GetAll( )
        {
            const string sql = @"
            select *
            from Reference.DasAccounts;
            ";

            return TestDataHelper.Query<DasAccountEntity>(sql);
        }

        internal static void Truncate()
        {
            const string sql = "TRUNCATE TABLE Reference.DasAccounts";
            TestDataHelper.Execute(sql);
        }
    }
}