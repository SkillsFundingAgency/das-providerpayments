using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Helpers
{
    internal static class RawEarningsDataHelper
    {
        internal static void CreateRawEarning(RawEarning rawEarning)
        {
            const string sql = @"
            INSERT INTO Staging.RawEarnings (
                LearnRefNumber,
                Ukprn,
                AimSeqNumber,
                PriceEpisodeIdentifier,
                EpisodeStartDate,
                EpisodeEffectiveTnpStartDate,
                Period,
                ULN,
                ProgrammeType,
                FrameworkCode,
                PathwayCode,
                StandardCode,
                SfaContributionPercentage,
                FundingLineType,
                LearnAimRef,
                LearningStartDate,
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
                ApprenticeshipContractType,
                FirstIncentiveCensusDate,
                SecondIncentiveCensusDate
            ) VALUES (
                @LearnRefNumber,
                @Ukprn,
                @AimSeqNumber,
                @PriceEpisodeIdentifier,
                @EpisodeStartDate,
                @EpisodeEffectiveTnpStartDate,
                @Period,
                @ULN,
                @ProgrammeType,
                @FrameworkCode,
                @PathwayCode,
                @StandardCode,
                @SfaContributionPercentage,
                @FundingLineType,
                @LearnAimRef,
                @LearningStartDate,
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
                @ApprenticeshipContractType,
                @FirstIncentiveCensusDate,
                @SecondIncentiveCensusDate
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