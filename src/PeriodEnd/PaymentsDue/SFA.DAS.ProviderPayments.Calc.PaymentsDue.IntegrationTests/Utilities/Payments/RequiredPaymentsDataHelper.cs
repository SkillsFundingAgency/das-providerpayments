using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Tools;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Utilities
{
    internal static class RequiredPaymentsDataHelper
    {
        internal static List<RequiredPaymentEntity> GetAll()
        {
            const string sql = @"
            SELECT *
            FROM PaymentsDue.RequiredPayments";

            return TestDataHelper
                .Query<RequiredPaymentEntity>(sql)
                .ToList();
        }

        internal static void Truncate()
        {
            const string sql = "TRUNCATE TABLE PaymentsDue.RequiredPayments";
            TestDataHelper.Execute(sql);
        }

        internal static void AddRequiredPayment(RequiredPayment entity)
        {
            const string sql = @"
                    INSERT INTO PaymentsDue.RequiredPayments
                        (Id, CommitmentId, CommitmentVersionId, AccountId, AccountVersionId, Uln, 
                        LearnRefNumber, AimSeqNumber, Ukprn, IlrSubmissionDateTime, PriceEpisodeIdentifier, 
                        StandardCode, ProgrammeType, FrameworkCode, PathwayCode, ApprenticeshipContractType,
                        DeliveryMonth, DeliveryYear, CollectionPeriodName, CollectionPeriodMonth, 
                        CollectionPeriodYear, TransactionType, AmountDue, SfaContributionPercentage,
                        FundingLineType, UseLevyBalance, LearnAimRef, LearningStartDate)
                        VALUES
                        (@Id, @CommitmentId, @CommitmentVersionId, @AccountId, @AccountVersionId, @Uln, 
                        @LearnRefNumber, @AimSeqNumber, @Ukprn, @IlrSubmissionDateTime, @PriceEpisodeIdentifier, 
                        @StandardCode, @ProgrammeType, @FrameworkCode, @PathwayCode, @ApprenticeshipContractType,
                        @DeliveryMonth, @DeliveryYear, @CollectionPeriodName, @CollectionPeriodMonth, 
                        @CollectionPeriodYear, @TransactionType, @AmountDue, @SfaContributionPercentage,
                        @FundingLineType, @UseLevyBalance, @LearnAimRef, @LearningStartDate)
                ";

            TestDataHelper.Execute(sql, entity);
        }
    }
}