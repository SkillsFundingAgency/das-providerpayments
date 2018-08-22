using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Helpers
{
    internal static class PriceEpisodeMatchDataHelper
    {
        internal static void Truncate()
        {
            const string sql = "TRUNCATE TABLE [DataLock].[PriceEpisodeMatch]";
            TestDataHelper.Execute(sql);
        }

        public static IEnumerable<PriceEpisodeMatchEntity> GetAll()
        {
            const string sql = @"
            SELECT *
            FROM [DataLock].[PriceEpisodeMatch];
            ";

            return TestDataHelper.Query<PriceEpisodeMatchEntity>(sql);
        }
    }
}