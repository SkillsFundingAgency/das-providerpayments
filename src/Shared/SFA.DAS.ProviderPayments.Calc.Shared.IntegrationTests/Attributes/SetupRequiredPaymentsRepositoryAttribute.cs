using System.Linq;
using AutoFixture;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Helpers;

namespace SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Attributes
{
    public class SetupRequiredPaymentsRepositoryAttribute : TestActionAttribute
    {
        private readonly long _idToSearchFor;

        public SetupRequiredPaymentsRepositoryAttribute(long idToSearchFor)
        {
            _idToSearchFor = idToSearchFor;
        }

        public override ActionTargets Targets { get; } = ActionTargets.Default;

        public override void BeforeTest(ITest test)
        {
            TestDataHelper.Execute("TRUNCATE TABLE PaymentsDue.RequiredPayments");

            var fixture = new Fixture();

            var requiredPayments = fixture.Build<RequiredPaymentEntity>()
                .With(x => x.Ukprn,
                    fixture.Create<Generator<long>>()
                        .First(ukprn => ukprn != _idToSearchFor))
                .CreateMany(3)
                .ToList();

            var matchingRequiredPayments = fixture.Build<RequiredPaymentEntity>()
                .With(x => x.Ukprn, _idToSearchFor)
                .With(x => x.AmountDue, -100)
                .CreateMany(3)
                .ToList();

            var positiveMatchingRequiredPayments = fixture.Build<RequiredPaymentEntity>()
                .With(x => x.Ukprn, _idToSearchFor)
                .With(x => x.AmountDue, 100)
                .CreateMany(1)
                .ToList();

            requiredPayments.AddRange(matchingRequiredPayments);
            requiredPayments.AddRange(positiveMatchingRequiredPayments);

            foreach (var requiredPayment in requiredPayments)
            {
                AddRequiredPayment(requiredPayment);
            }

            base.BeforeTest(test);
        }

        static void AddRequiredPayment(RequiredPaymentEntity entity)
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