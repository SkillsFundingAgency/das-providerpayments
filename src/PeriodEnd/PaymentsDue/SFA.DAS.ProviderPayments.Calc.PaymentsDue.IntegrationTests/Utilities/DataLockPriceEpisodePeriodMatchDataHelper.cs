using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Tools;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Utilities
{
    internal static class DataLockPriceEpisodePeriodMatchDataHelper
    {
        internal static void CreateEntity(DatalockOutput entity)
        {
            const string sql = @"
            INSERT INTO [DataLock].[PriceEpisodePeriodMatch] (
                Ukprn,
                PriceEpisodeIdentifier,
                LearnRefNumber,
                AimSeqNumber,
                CommitmentId,
                VersionId,
                Period,
                Payable,
                TransactionType,
                TransactionTypesFlag
            ) VALUES (
                @Ukprn,
                @PriceEpisodeIdentifier,
                @LearnRefNumber,
                @AimSeqNumber,
                @CommitmentId,
                @VersionId,
                @Period,
                @Payable,
                @TransactionType,
                @TransactionTypesFlag
            );";

            TestDataHelper.Execute(sql, entity);
        }

        internal static void Truncate()
        {
            const string sql = "TRUNCATE TABLE [DataLock].[PriceEpisodePeriodMatch]";
            TestDataHelper.Execute(sql);
        }
    }
}