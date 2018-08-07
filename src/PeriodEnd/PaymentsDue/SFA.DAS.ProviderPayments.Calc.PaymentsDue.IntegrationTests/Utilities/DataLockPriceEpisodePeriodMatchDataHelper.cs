using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Tools;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Utilities
{
    internal static class DataLockPriceEpisodePeriodMatchDataHelper
    {
        internal static void CreateEntity(DatalockOutputEntity entity)
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

            const string nonPeriodSql = @"
                    IF NOT EXISTS (
                        SELECT NULL 
                        FROM Datalock.PriceEpisodeMatch
                        WHERE PriceEpisodeIdentifier = @PriceEpisodeIdentifier
                        AND Ukprn = @Ukprn
                        AND LearnRefNumber = @LearnRefNumber
                        AND CommitmentId = @CommitmentId
                    )
                    INSERT INTO Datalock.PriceEpisodeMatch
                    (UKPRN, PriceEpisodeIdentifier, LearnRefNumber, AimSeqNumber, CommitmentId, IsSuccess)
                    VALUES
                    (@Ukprn, @priceEpisodeIdentifier, @LearnRefNumber, @AimSeqNumber, @CommitmentId, 1)
                ";

            TestDataHelper.Execute(sql, entity);
            TestDataHelper.Execute(nonPeriodSql, entity);
        }

        internal static void Truncate()
        {
            const string sql = "TRUNCATE TABLE [DataLock].[PriceEpisodePeriodMatch]";
            TestDataHelper.Execute(sql);
        }
    }
}