﻿using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Helpers
{
    internal static class RawEarningsMathsEnglishDataHelper
    {
        internal static void CreateRawEarningMathsEnglish(RawEarningForMathsOrEnglish rawEarningForMathsOrEnglish)
        {
            const string sql = @"
            INSERT INTO Staging.RawEarningsMathsEnglish (
                LearnRefNumber,
                Ukprn,
                AimSeqNumber,
                LearningStartDate,
                Period,
                ULN,
                ProgrammeType,
                FrameworkCode,
                PathwayCode,
                StandardCode,
                SfaContributionPercentage,
                FundingLineType,
                LearnAimRef,
                TransactionType13,
                TransactionType14,
                TransactionType15,
                ApprenticeshipContractType
            ) VALUES (
                @LearnRefNumber,
                @Ukprn,
                @AimSeqNumber,
                @LearningStartDate,
                @Period,
                @ULN,
                @ProgrammeType,
                @FrameworkCode,
                @PathwayCode,
                @StandardCode,
                @SfaContributionPercentage,
                @FundingLineType,
                @LearnAimRef,
                @TransactionType13,
                @TransactionType14,
                @TransactionType15,
                @ApprenticeshipContractType
            );";

            TestDataHelper.Execute(sql, rawEarningForMathsOrEnglish);
        }

        internal static void Truncate()
        {
            const string sql = "TRUNCATE TABLE Staging.RawEarningsMathsEnglish";
            TestDataHelper.Execute(sql);
        }
    }
}