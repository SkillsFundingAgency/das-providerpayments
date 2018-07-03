using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Tools;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Utilities
{
    internal static class RequiredPaymentsHistoryDataHelper
    {
        internal static void CreateEntity(RequiredPayment entity)
        {
            const string sql = @"
            INSERT INTO Reference.RequiredPaymentsHistory (
	            Id,
	            CommitmentId,
	            CommitmentVersionId,
	            AccountId,
	            AccountVersionId,
	            LearnRefNumber,
	            Uln,
	            AimSeqNumber,
	            Ukprn,
	            DeliveryMonth,
	            DeliveryYear,
	            CollectionPeriodName,
	            CollectionPeriodMonth,
	            CollectionPeriodYear,
	            TransactionType,
	            AmountDue,
	            StandardCode,
	            ProgrammeType,
	            FrameworkCode,
	            PathwayCode,
	            PriceEpisodeIdentifier,
	            LearnAimRef,
	            LearningStartDate,
	            IlrSubmissionDateTime,
	            ApprenticeshipContractType,
	            SfaContributionPercentage,
	            FundingLineType,
	            UseLevyBalance
            ) VALUES (
	            @Id,
	            @CommitmentId,
	            @CommitmentVersionId,
	            @AccountId,
	            @AccountVersionId,
	            @LearnRefNumber,
	            @Uln,
	            @AimSeqNumber,
	            @Ukprn,
	            @DeliveryMonth,
	            @DeliveryYear,
	            @CollectionPeriodName,
	            @CollectionPeriodMonth,
	            @CollectionPeriodYear,
	            @TransactionType,
	            @AmountDue,
	            @StandardCode,
	            @ProgrammeType,
	            @FrameworkCode,
	            @PathwayCode,
	            @PriceEpisodeIdentifier,
	            @LearnAimRef,
	            @LearningStartDate,
	            @IlrSubmissionDateTime,
	            @ApprenticeshipContractType,
	            @SfaContributionPercentage,
	            @FundingLineType,
	            @UseLevyBalance
            );";

            TestDataHelper.Execute(sql, entity);
        }

        internal static void Truncate()
        {
            const string sql = "TRUNCATE TABLE Reference.RequiredPaymentsHistory";
            TestDataHelper.Execute(sql);
        }
    }
}