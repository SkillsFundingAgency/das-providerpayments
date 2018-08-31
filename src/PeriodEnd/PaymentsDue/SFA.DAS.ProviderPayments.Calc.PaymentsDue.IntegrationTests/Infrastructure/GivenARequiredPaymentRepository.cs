using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Utilities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Infrastructure
{
    [TestFixture, SetupUkprn]
    public class GivenARequiredPaymentRepository
    {
        private RequiredPaymentRepository _sut;

        [OneTimeSetUp]
        public void Setup()
        {
            _sut = new RequiredPaymentRepository(GlobalTestContext.Instance.TransientConnectionString);
        }

        [TestFixture]
        public class WhenCallingAddMany : GivenARequiredPaymentRepository
        {
            private List<RequiredPayment> _expectedEntities;
            private List<RequiredPaymentEntity> _actualEntities;

            [OneTimeSetUp]
            public new void Setup()
            {
                base.Setup();

                _expectedEntities = new Fixture()
                    .Build<RequiredPayment>()
                    .CreateMany()
                    .ToList();

                _expectedEntities.ForEach(x => x.SfaContributionPercentage = Math.Round(x.SfaContributionPercentage, 4));

                RequiredPaymentsDataHelper.Truncate();

                _sut.AddMany(_expectedEntities);

                _actualEntities = RequiredPaymentsDataHelper.GetAll();
            }

            [Test]
            public void ThenItSavesTheExpectedNumberOfEntities() =>
                _actualEntities.Count
                    .Should().Be(_expectedEntities.Count);

            [Test]
            public void ThenItSetsCommitmentId() =>
                    _actualEntities[0].CommitmentId
                        .Should().Be(_expectedEntities[0].CommitmentId);

            [Test]
            public void ThenItSetsCommitmentVersionId() =>
                _actualEntities[0].CommitmentVersionId
                    .Should().Be(_expectedEntities[0].CommitmentVersionId);

            [Test]
            public void ThenItSetsAccountId() =>
                _actualEntities[0].AccountId
                    .Should().Be(_expectedEntities[0].AccountId);

            [Test]
            public void ThenItSetsAccountVersionId() =>
                _actualEntities[0].AccountVersionId
                    .Should().Be(_expectedEntities[0].AccountVersionId);

            [Test]
            public void ThenItSetsUln() =>
                _actualEntities[0].Uln
                    .Should().Be(_expectedEntities[0].Uln);

            [Test]
            public void ThenItSetsLearnRefNumber() =>
                _actualEntities[0].LearnRefNumber
                    .Should().Be(_expectedEntities[0].LearnRefNumber);

            [Test]
            public void ThenItSetsAimSeqNumber() =>
                _actualEntities[0].AimSeqNumber
                    .Should().Be(_expectedEntities[0].AimSeqNumber);

            [Test]
            public void ThenItSetsUkprn() =>
                _actualEntities[0].Ukprn
                    .Should().Be(_expectedEntities[0].Ukprn);

            [Test]
            public void ThenItSetsIlrSubmissionDateTime() =>
                _actualEntities[0].IlrSubmissionDateTime
                    .Should().BeSameDateAs(_expectedEntities[0].IlrSubmissionDateTime);

            [Test]
            public void ThenItSetsPriceEpisodeIdentifier() =>
                _actualEntities[0].PriceEpisodeIdentifier
                    .Should().Be(_expectedEntities[0].PriceEpisodeIdentifier);

            [Test]
            public void ThenItSetsStandardCode() =>
                _actualEntities[0].StandardCode
                    .Should().Be(_expectedEntities[0].StandardCode);

            [Test]
            public void ThenItSetsProgrammeType() =>
                _actualEntities[0].ProgrammeType
                    .Should().Be(_expectedEntities[0].ProgrammeType);

            [Test]
            public void ThenItSetsFrameworkCode() =>
                _actualEntities[0].FrameworkCode
                    .Should().Be(_expectedEntities[0].FrameworkCode);

            [Test]
            public void ThenItSetsPathwayCode() =>
                _actualEntities[0].PathwayCode
                    .Should().Be(_expectedEntities[0].PathwayCode);

            [Test]
            public void ThenItSetsApprenticeshipContractType() =>
                _actualEntities[0].ApprenticeshipContractType
                    .Should().Be(_expectedEntities[0].ApprenticeshipContractType);

            [Test]
            public void ThenItSetsDeliveryMonth() =>
                _actualEntities[0].DeliveryMonth
                    .Should().Be(_expectedEntities[0].DeliveryMonth);

            [Test]
            public void ThenItSetsDeliveryYear() =>
                _actualEntities[0].DeliveryYear
                    .Should().Be(_expectedEntities[0].DeliveryYear);

            [Test]
            public void ThenItSetsTransactionType() =>
                _actualEntities[0].TransactionType
                    .Should().Be(_expectedEntities[0].TransactionType);

            [Test]
            public void ThenItSetsAmountDue() =>
                _actualEntities[0].AmountDue
                    .Should().Be(_expectedEntities[0].AmountDue);

            [Test]
            public void ThenItSetsSfaContributionPercentage() =>
                _actualEntities[0].SfaContributionPercentage
                    .Should().Be(_expectedEntities[0].SfaContributionPercentage);

            [Test]
            public void ThenItSetsFundingLineType() =>
                _actualEntities[0].FundingLineType
                    .Should().Be(_expectedEntities[0].FundingLineType);

            [Test]
            public void ThenItSetsUseLevyBalance() =>
                _actualEntities[0].UseLevyBalance
                    .Should().Be(_expectedEntities[0].UseLevyBalance);

            [Test]
            public void ThenItSetsLearnAimRef() =>
                _actualEntities[0].LearnAimRef
                    .Should().Be(_expectedEntities[0].LearnAimRef);

            [Test]
            public void ThenItSetsLearningStartDate() =>
                _actualEntities[0].LearningStartDate
                    .Should().BeSameDateAs(_expectedEntities[0].LearningStartDate);

            [Test]
            public void ThenItSetsCollectionPeriodName() =>
                _actualEntities[0].CollectionPeriodName
                    .Should().Be(_expectedEntities[0].CollectionPeriodName);

            [Test]
            public void ThenItSetsCollectionPeriodMonth() =>
                _actualEntities[0].CollectionPeriodMonth
                    .Should().Be(_expectedEntities[0].CollectionPeriodMonth);

            [Test]
            public void ThenItSetsCollectionPeriodYear() =>
                _actualEntities[0].CollectionPeriodYear
                    .Should().Be(_expectedEntities[0].CollectionPeriodYear);
        }

        /// <summary>
        /// Testing that the values are saved to the database are null when 0 in the data for accountId, CommitmentId,
        ///     StandardCode, ProgrammeType, FrameworkCode, PathwayCode
        /// </summary>
        [TestFixture]
        public class WhenCallingAddManyWithEntitiesThatHaveNullValues : GivenARequiredPaymentRepository
        {
            private List<RequiredPayment> _expectedEntities;
            private List<RequiredPaymentEntity> _actualEntities;

            [OneTimeSetUp]
            public new void Setup()
            {
                base.Setup();

                _expectedEntities = new Fixture()
                    .Build<RequiredPayment>()
                    .With(x => x.CommitmentId, 0)
                    .With(x => x.AccountId, 0)
                    .With(x => x.StandardCode, 0)
                    .With(x => x.FrameworkCode, 0)
                    .With(x => x.ProgrammeType, 0)
                    .With(x => x.PathwayCode, 0)
                    .CreateMany()
                    .ToList();

                _expectedEntities.ForEach(x => x.SfaContributionPercentage = Math.Round(x.SfaContributionPercentage, 4));

                RequiredPaymentsDataHelper.Truncate();

                _sut.AddMany(_expectedEntities);

                _actualEntities = RequiredPaymentsDataHelper.GetAll();
            }

            [Test]
            public void ThenItSavesTheExpectedNumberOfEntities() =>
                _actualEntities.Count
                    .Should().Be(_expectedEntities.Count);

            [Test]
            public void ThenItSetsCommitmentId() =>
                    _actualEntities[0].CommitmentId
                        .Should().Be(null);

            [Test]
            public void ThenItSetsCommitmentVersionId() =>
                _actualEntities[0].CommitmentVersionId
                    .Should().Be(_expectedEntities[0].CommitmentVersionId);

            [Test]
            public void ThenItSetsAccountId() =>
                _actualEntities[0].AccountId
                    .Should().Be(null);

            [Test]
            public void ThenItSetsAccountVersionId() =>
                _actualEntities[0].AccountVersionId
                    .Should().Be(_expectedEntities[0].AccountVersionId);

            [Test]
            public void ThenItSetsUln() =>
                _actualEntities[0].Uln
                    .Should().Be(_expectedEntities[0].Uln);

            [Test]
            public void ThenItSetsLearnRefNumber() =>
                _actualEntities[0].LearnRefNumber
                    .Should().Be(_expectedEntities[0].LearnRefNumber);

            [Test]
            public void ThenItSetsAimSeqNumber() =>
                _actualEntities[0].AimSeqNumber
                    .Should().Be(_expectedEntities[0].AimSeqNumber);

            [Test]
            public void ThenItSetsUkprn() =>
                _actualEntities[0].Ukprn
                    .Should().Be(_expectedEntities[0].Ukprn);

            [Test]
            public void ThenItSetsIlrSubmissionDateTime() =>
                _actualEntities[0].IlrSubmissionDateTime
                    .Should().BeSameDateAs(_expectedEntities[0].IlrSubmissionDateTime);

            [Test]
            public void ThenItSetsPriceEpisodeIdentifier() =>
                _actualEntities[0].PriceEpisodeIdentifier
                    .Should().Be(_expectedEntities[0].PriceEpisodeIdentifier);

            [Test]
            public void ThenItSetsStandardCode() =>
                _actualEntities[0].StandardCode
                    .Should().Be(null);

            [Test]
            public void ThenItSetsProgrammeType() =>
                _actualEntities[0].ProgrammeType
                    .Should().Be(null);

            [Test]
            public void ThenItSetsFrameworkCode() =>
                _actualEntities[0].FrameworkCode
                    .Should().Be(null);

            [Test]
            public void ThenItSetsPathwayCode() =>
                _actualEntities[0].PathwayCode
                    .Should().Be(null);

            [Test]
            public void ThenItSetsApprenticeshipContractType() =>
                _actualEntities[0].ApprenticeshipContractType
                    .Should().Be(_expectedEntities[0].ApprenticeshipContractType);

            [Test]
            public void ThenItSetsDeliveryMonth() =>
                _actualEntities[0].DeliveryMonth
                    .Should().Be(_expectedEntities[0].DeliveryMonth);

            [Test]
            public void ThenItSetsDeliveryYear() =>
                _actualEntities[0].DeliveryYear
                    .Should().Be(_expectedEntities[0].DeliveryYear);

            [Test]
            public void ThenItSetsTransactionType() =>
                _actualEntities[0].TransactionType
                    .Should().Be(_expectedEntities[0].TransactionType);

            [Test]
            public void ThenItSetsAmountDue() =>
                _actualEntities[0].AmountDue
                    .Should().Be(_expectedEntities[0].AmountDue);

            [Test]
            public void ThenItSetsSfaContributionPercentage() =>
                _actualEntities[0].SfaContributionPercentage
                    .Should().Be(_expectedEntities[0].SfaContributionPercentage);

            [Test]
            public void ThenItSetsFundingLineType() =>
                _actualEntities[0].FundingLineType
                    .Should().Be(_expectedEntities[0].FundingLineType);

            [Test]
            public void ThenItSetsUseLevyBalance() =>
                _actualEntities[0].UseLevyBalance
                    .Should().Be(_expectedEntities[0].UseLevyBalance);

            [Test]
            public void ThenItSetsLearnAimRef() =>
                _actualEntities[0].LearnAimRef
                    .Should().Be(_expectedEntities[0].LearnAimRef);

            [Test]
            public void ThenItSetsLearningStartDate() =>
                _actualEntities[0].LearningStartDate
                    .Should().BeSameDateAs(_expectedEntities[0].LearningStartDate);

            [Test]
            public void ThenItSetsCollectionPeriodName() =>
                _actualEntities[0].CollectionPeriodName
                    .Should().Be(_expectedEntities[0].CollectionPeriodName);

            [Test]
            public void ThenItSetsCollectionPeriodMonth() =>
                _actualEntities[0].CollectionPeriodMonth
                    .Should().Be(_expectedEntities[0].CollectionPeriodMonth);

            [Test]
            public void ThenItSetsCollectionPeriodYear() =>
                _actualEntities[0].CollectionPeriodYear
                    .Should().Be(_expectedEntities[0].CollectionPeriodYear);
        }
    }
}
