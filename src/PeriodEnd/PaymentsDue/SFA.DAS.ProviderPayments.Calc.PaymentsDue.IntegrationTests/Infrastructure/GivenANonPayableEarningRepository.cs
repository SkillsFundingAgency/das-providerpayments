using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Utilities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Infrastructure
{
    [TestFixture, SetupUkprn]
    public class GivenANonPayableEarningRepository
    {
        private NonPayableEarningRepository _sut;
        
        [OneTimeSetUp]
        public void Setup()
        {
            _sut = new NonPayableEarningRepository(GlobalTestContext.Instance.TransientConnectionString);
        }

        [TestFixture, SetupNoNonPayableEarnings]
        public class AndThereAreNoNonPayableEarnings : GivenANonPayableEarningRepository
        {
            [TestFixture]
            public class WhenCallingAddMany : AndThereAreNoNonPayableEarnings
            {
                private List<NonPayableEarningEntity> _expectedEntities;
                private List<NonPayableEarningEntity> _actualEntities;

                [OneTimeSetUp]
                public new void Setup()
                {
                    base.Setup();

                    _expectedEntities = new Fixture()
                        .Build<NonPayableEarningEntity>()
                        .CreateMany()
                        .ToList();

                    _sut.AddMany(_expectedEntities);

                    _actualEntities = NonPayableEarningsDataHelper.GetAll();
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
                public void ThenItSetsReason() =>
                    _actualEntities[0].Reason
                        .Should().Be(_expectedEntities[0].Reason);
            }
        }
    }
}