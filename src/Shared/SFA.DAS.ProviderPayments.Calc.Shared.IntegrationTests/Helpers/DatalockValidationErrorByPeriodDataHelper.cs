using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Helpers
{
    internal static class DatalockValidationErrorByPeriodDataHelper
    {
        internal static void Truncate()
        {
            const string sql = "TRUNCATE TABLE [DataLock].[ValidationErrorByPeriod]";
            TestDataHelper.Execute(sql);
        }

        public static IEnumerable<DatalockValidationErrorByPeriod> GetAll()
        {
            const string sql = @"
            SELECT *
            FROM [DataLock].[ValidationErrorByPeriod];
            ";

            return TestDataHelper.Query<DatalockValidationErrorByPeriod>(sql);
        }
    }
}