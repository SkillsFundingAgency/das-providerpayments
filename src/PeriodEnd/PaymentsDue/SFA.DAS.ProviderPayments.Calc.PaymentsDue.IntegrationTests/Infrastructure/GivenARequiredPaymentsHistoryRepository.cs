using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Utilities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Infrastructure
{
    [TestFixture, SetupUkprn]
    public class GivenARequiredPaymentsHistoryRepository
    {
        private RequiredPaymentsHistoryRepository _sut;

        [OneTimeSetUp]
        public void Setup()
        {
            _sut = new RequiredPaymentsHistoryRepository(GlobalTestContext.Instance.TransientConnectionString);
        }

        [TestFixture, SetupNoRequiredPaymentsHistory]
        public class AndThereAreNoRequiredPaymentsHistoryForProvider : GivenARequiredPaymentsHistoryRepository
        {
            [TestFixture]
            public class WhenCallingGetAllForProvider : AndThereAreNoRequiredPaymentsHistoryForProvider
            {
                [Test]
                public void ThenItReturnsAnEmptyList()
                {
                    Setup();
                    var result = _sut.GetAllForProvider(PaymentsDueTestContext.Ukprn);
                    result.Should().BeEmpty();
                }
            }
        }

        [TestFixture, SetupRequiredPaymentsHistory]
        public class AndThereAreSomeRequiredPaymentsHistoryForProvider : GivenARequiredPaymentsHistoryRepository
        {
            [TestFixture]
            public class WhenCallingGetAllForProvider : AndThereAreSomeRequiredPaymentsHistoryForProvider
            {
                private List<RequiredPaymentEntity> _actualHistoricalPayments;
                private List<RequiredPaymentEntity> _expectedHistoricalPayments;

                [SetUp]
                public new void Setup()
                {
                    base.Setup();
                    _actualHistoricalPayments = _sut.GetAllForProvider(PaymentsDueTestContext.Ukprn);

                    _expectedHistoricalPayments = PaymentsDueTestContext.RequiredPaymentsHistory
                        .Where(entity => entity.Ukprn == PaymentsDueTestContext.Ukprn).ToList();
                }

                [Test]
                public void ThenItRetrievesExpectedCount()
                {
                    if (_expectedHistoricalPayments.Count < 1)
                        Assert.Fail("Not enough earnings to test");

                    _actualHistoricalPayments.Count.Should().Be(_expectedHistoricalPayments.Count);
                }

                [Test]
                public void ThenIdIsSetCorrectly() =>
                    _actualHistoricalPayments[0].Id.Should().Be(_expectedHistoricalPayments[0].Id);

                [Test]
                public void ThenCommitmentIdIsSetCorrectly() =>
                    _actualHistoricalPayments[0].CommitmentId.Should().Be(_expectedHistoricalPayments[0].CommitmentId);

                [Test]
                public void ThenCommitmentVersionIdIsSetCorrectly() =>
                    _actualHistoricalPayments[0].CommitmentVersionId.Should().Be(_expectedHistoricalPayments[0].CommitmentVersionId);

                [Test]
                public void ThenAccountIdIsSetCorrectly() =>
                    _actualHistoricalPayments[0].AccountId.Should().Be(_expectedHistoricalPayments[0].AccountId);

                [Test]
                public void ThenAccountVersionIdIsSetCorrectly() =>
                    _actualHistoricalPayments[0].AccountVersionId.Should().Be(_expectedHistoricalPayments[0].AccountVersionId);

                [Test]
                public void ThenLearnRefNumberIsSetCorrectly() =>
                    _actualHistoricalPayments[0].LearnRefNumber.Should().Be(_expectedHistoricalPayments[0].LearnRefNumber);

                [Test]
                public void ThenUlnIsSetCorrectly() =>
                    _actualHistoricalPayments[0].Uln.Should().Be(_expectedHistoricalPayments[0].Uln);

                [Test]
                public void ThenAimSeqNumberIsSetCorrectly() =>
                    _actualHistoricalPayments[0].AimSeqNumber.Should().Be(_expectedHistoricalPayments[0].AimSeqNumber);

                [Test]
                public void ThenUkprnIsSetCorrectly() =>
                    _actualHistoricalPayments[0].Ukprn.Should().Be(_expectedHistoricalPayments[0].Ukprn);

                [Test]
                public void ThenDeliveryMonthIsSetCorrectly() =>
                    _actualHistoricalPayments[0].DeliveryMonth.Should().Be(_expectedHistoricalPayments[0].DeliveryMonth);

                [Test]
                public void ThenDeliveryYearIsSetCorrectly() =>
                    _actualHistoricalPayments[0].DeliveryYear.Should().Be(_expectedHistoricalPayments[0].DeliveryYear);

                [Test]
                public void ThenCollectionPeriodNameIsSetCorrectly() =>
                    _actualHistoricalPayments[0].CollectionPeriodName.Should().Be(_expectedHistoricalPayments[0].CollectionPeriodName);

                [Test]
                public void ThenCollectionPeriodMonthIsSetCorrectly() =>
                    _actualHistoricalPayments[0].CollectionPeriodMonth.Should().Be(_expectedHistoricalPayments[0].CollectionPeriodMonth);

                [Test]
                public void ThenCollectionPeriodYearIsSetCorrectly() =>
                    _actualHistoricalPayments[0].CollectionPeriodYear.Should().Be(_expectedHistoricalPayments[0].CollectionPeriodYear);

                [Test]
                public void ThenTransactionTypeIsSetCorrectly() =>
                    _actualHistoricalPayments[0].TransactionType.Should().Be(_expectedHistoricalPayments[0].TransactionType);

                [Test]
                public void ThenAmountDueIsSetCorrectly() =>
                    _actualHistoricalPayments[0].AmountDue.Should().Be(_expectedHistoricalPayments[0].AmountDue);

                [Test]
                public void ThenStandardCodeIsSetCorrectly() =>
                    _actualHistoricalPayments[0].StandardCode.Should().Be(_expectedHistoricalPayments[0].StandardCode);

                [Test]
                public void ThenProgrammeTypeIsSetCorrectly() =>
                    _actualHistoricalPayments[0].ProgrammeType.Should().Be(_expectedHistoricalPayments[0].ProgrammeType);

                [Test]
                public void ThenFrameworkCodeIsSetCorrectly() =>
                    _actualHistoricalPayments[0].FrameworkCode.Should().Be(_expectedHistoricalPayments[0].FrameworkCode);

                [Test]
                public void ThenPathwayCodeIsSetCorrectly() =>
                    _actualHistoricalPayments[0].PathwayCode.Should().Be(_expectedHistoricalPayments[0].PathwayCode);

                [Test]
                public void ThenPriceEpisodeIdentifierIsSetCorrectly() =>
                    _actualHistoricalPayments[0].PriceEpisodeIdentifier.Should().Be(_expectedHistoricalPayments[0].PriceEpisodeIdentifier);

                [Test]
                public void ThenLearnAimRefIsSetCorrectly() =>
                    _actualHistoricalPayments[0].LearnAimRef.Should().Be(_expectedHistoricalPayments[0].LearnAimRef);

                [Test]
                public void ThenLearningStartDateIsSetCorrectly() =>
                    _actualHistoricalPayments[0].LearningStartDate.Should().BeCloseTo(_expectedHistoricalPayments[0].LearningStartDate);

                [Test]
                public void ThenIlrSubmissionDateTimeIsSetCorrectly() =>
                    _actualHistoricalPayments[0].IlrSubmissionDateTime.Should().BeCloseTo(_expectedHistoricalPayments[0].IlrSubmissionDateTime);

                [Test]
                public void ThenApprenticeshipContractTypeIsSetCorrectly() =>
                    _actualHistoricalPayments[0].ApprenticeshipContractType.Should().Be(_expectedHistoricalPayments[0].ApprenticeshipContractType);

                [Test]
                public void ThenSfaContributionPercentageIsSetCorrectly() =>
                    _actualHistoricalPayments[0].SfaContributionPercentage.Should().Be(_expectedHistoricalPayments[0].SfaContributionPercentage);

                [Test]
                public void ThenFundingLineTypeIsSetCorrectly() =>
                    _actualHistoricalPayments[0].FundingLineType.Should().Be(_expectedHistoricalPayments[0].FundingLineType);

                [Test]
                public void ThenUseLevyBalanceIsSetCorrectly() =>
                    _actualHistoricalPayments[0].UseLevyBalance.Should().Be(_expectedHistoricalPayments[0].UseLevyBalance);
            }
        }
    }
}