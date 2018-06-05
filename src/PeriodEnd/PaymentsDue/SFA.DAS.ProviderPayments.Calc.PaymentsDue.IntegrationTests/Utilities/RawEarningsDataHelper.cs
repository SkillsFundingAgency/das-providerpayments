﻿using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
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
                ApprenticeshipContractType
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
                @ApprenticeshipContractType
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