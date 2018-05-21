using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Tools;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Utilities
{
    internal static class RawEarningsMathsEnglishDataHelper
    {
        internal static void CreateRawEarningMathsEnglish(RawEarningMathsEnglishEntity rawEarningMathsEnglish)
        {
            const string sql = @"
            INSERT INTO Staging.RawEarningsMathsEnglish (
                LearnRefNumber,
                Ukprn,
                AimSeqNumber,
                LearnStartDate,
                Period,
                ULN,
                ProgType,
                FworkCode,
                PwayCode,
                StdCode,
                LearnDelSfaContribPct,
                FundLineType,
                LearnAimRef,
                TransactionType13,
                TransactionType14,
                TransactionType15,
                ACT
            ) VALUES (
                @LearnRefNumber,
                @Ukprn,
                @AimSeqNumber,
                @LearnStartDate,
                @Period,
                @ULN,
                @ProgType,
                @FworkCode,
                @PwayCode,
                @StdCode,
                @LearnDelSfaContribPct,
                @FundLineType,
                @LearnAimRef,
                @TransactionType13,
                @TransactionType14,
                @TransactionType15,
                @ACT
            );";

            TestDataHelper.Execute(sql, rawEarningMathsEnglish);
        }

        internal static void Truncate()
        {
            const string sql = "TRUNCATE TABLE Staging.RawEarningsMathsEnglish";
            TestDataHelper.Execute(sql);
        }
    }
}