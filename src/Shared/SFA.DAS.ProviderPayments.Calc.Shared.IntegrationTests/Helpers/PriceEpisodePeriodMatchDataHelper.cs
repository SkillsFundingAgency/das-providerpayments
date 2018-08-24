using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Helpers
{
    internal static class PriceEpisodePeriodMatchDataHelper
    {
        internal static void Truncate()
        {
            const string sql = "TRUNCATE TABLE [DataLock].[PriceEpisodePeriodMatch]";
            TestDataHelper.Execute(sql);
        }

        internal static IEnumerable<PriceEpisodePeriodMatchEntity> GetAll()
        {
            const string sql = @"
            SELECT *
            FROM [DataLock].[PriceEpisodePeriodMatch];
            ";

            return TestDataHelper.Query<PriceEpisodePeriodMatchEntity>(sql);
        }
    }
}