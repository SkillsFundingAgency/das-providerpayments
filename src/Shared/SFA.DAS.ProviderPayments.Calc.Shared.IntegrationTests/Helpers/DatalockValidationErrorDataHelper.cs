using System.Collections.Generic;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Helpers
{
    internal static class DatalockValidationErrorDataHelper
    {
        internal static void Truncate()
        {
            const string sql = "TRUNCATE TABLE [DataLock].[ValidationError]";
            TestDataHelper.Execute(sql);
        }

        public static void CreateEntity(DatalockValidationError validationError)
        {
            const string sql = @"
            INSERT INTO [DataLock].[ValidationError] (
                Ukprn,
                PriceEpisodeIdentifier,
                LearnRefNumber,
                AimSeqNumber,
                RuleId
            ) VALUES (
                @Ukprn,
                @PriceEpisodeIdentifier,
                @LearnRefNumber,
                @AimSeqNumber,
                @RuleId
            );";

            TestDataHelper.Execute(sql, validationError);
        }

        public static IEnumerable<DatalockValidationError> GetAll()
        {
            const string sql = @"
            SELECT *
            FROM [DataLock].[ValidationError];
            ";

            return TestDataHelper.Query<DatalockValidationError>(sql);
        }
    }
}