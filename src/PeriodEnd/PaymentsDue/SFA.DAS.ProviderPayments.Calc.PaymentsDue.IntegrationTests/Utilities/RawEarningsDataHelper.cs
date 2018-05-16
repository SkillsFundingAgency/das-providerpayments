using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Tools;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Utilities
{
    internal static class RawEarningsDataHelper
    {
        internal static void CreateRawEarning(RawEarningEntity rawEarning)
        {
            const string sql = @"
            INSERT INTO Staging.RawEarnings (
                LearnRefNumber,
                Ukprn,
                PriceEpisodeAimSeqNumber,
                PriceEpisodeIdentifier,
                EpisodeStartDate,
                Period,
                ULN,
                ProgType,
                FworkCode,
                PwayCode,
                StdCode,
                PriceEpisodeSFAContribPct,
                PriceEpisodeFundLineType,
                LearnAimRef,
                LearnStartDate,
                TransactionType01,
                TransactionType02,
                TransactionType03,
                TransactionType04,
                TransactionType05,
                TransactionType06,
                TransactionType07,
                TransactionType08,
                TransactionType09,
                TransactionType10,
                TransactionType11,
                TransactionType12,
                TransactionType15,
                ACT
            ) VALUES (
                @LearnRefNumber,
                @Ukprn,
                @PriceEpisodeAimSeqNumber,
                @PriceEpisodeIdentifier,
                @EpisodeStartDate,
                @Period,
                @ULN,
                @ProgType,
                @FworkCode,
                @PwayCode,
                @StdCode,
                @PriceEpisodeSFAContribPct,
                @PriceEpisodeFundLineType,
                @LearnAimRef,
                @LearnStartDate,
                @TransactionType01,
                @TransactionType02,
                @TransactionType03,
                @TransactionType04,
                @TransactionType05,
                @TransactionType06,
                @TransactionType07,
                @TransactionType08,
                @TransactionType09,
                @TransactionType10,
                @TransactionType11,
                @TransactionType12,
                @TransactionType15,
                @ACT
            );";

            TestDataHelper.Execute(sql, rawEarning);
        }

        internal static void Truncate()
        {
            const string sql = "TRUNCATE TABLE Staging.RawEarnings";
            TestDataHelper.Execute(sql);
        }
    }
}