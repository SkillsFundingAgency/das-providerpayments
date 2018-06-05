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
    public class GivenARawEarningsRepository
    {
        private RawEarningsRepository _sut;

        [OneTimeSetUp]
        public void Setup()
        {
            _sut = new RawEarningsRepository(GlobalTestContext.Instance.TransientConnectionString);
        }

        [TestFixture, SetupNoRawEarnings]
        public class AndThereAreNoRawEarningsForProvider : GivenARawEarningsRepository
        {
            [TestFixture]
            public class WhenCallingGetAllForProvider : AndThereAreNoRawEarningsForProvider
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

        [TestFixture, SetupRawEarnings]
        public class AndThereAreSomeRawEarningsForProvider : GivenARawEarningsRepository
        {
            [TestFixture]
            public class WhenCallingGetAllForProvider : AndThereAreSomeRawEarningsForProvider
            {
                private List<RawEarningEntity> _actualRawEarnings;
                private List<RawEarningEntity> _expectedRawEarnings;

                [SetUp]
                public new void Setup()
                {
                    base.Setup();
                    _actualRawEarnings = _sut.GetAllForProvider(PaymentsDueTestContext.Ukprn);

                    _expectedRawEarnings = PaymentsDueTestContext.RawEarnings
                        .Where(earning => earning.Ukprn == PaymentsDueTestContext.Ukprn).ToList();
                }

                [Test]
                public void ThenItRetrievesExpectedCount()
                {
                    if (_expectedRawEarnings.Count < 1)
                        Assert.Fail("Not enough earnings to test");

                    _actualRawEarnings.Count.Should().Be(_expectedRawEarnings.Count);
                }

                [Test]
                public void ThenLearnRefNumberIsSetCorrectly() =>
                    _actualRawEarnings[0].LearnRefNumber.Should().Be(_expectedRawEarnings[0].LearnRefNumber);
                
                [Test]
                public void ThenUkprnIsSetCorrectly() => 
                    _actualRawEarnings[0].Ukprn.Should().Be(_expectedRawEarnings[0].Ukprn);

                [Test]
                public void ThenPriceEpisodeAimSeqNumberIsSetCorrectly() =>
                    _actualRawEarnings[0].AimSeqNumber.Should().Be(_expectedRawEarnings[0].AimSeqNumber);

                [Test]
                public void ThenPriceEpisodeIdentifierIsSetCorrectly() =>
                    _actualRawEarnings[0].PriceEpisodeIdentifier.Should().Be(_expectedRawEarnings[0].PriceEpisodeIdentifier);

                [Test]
                public void ThenEpisodeStartDateIsSetCorrectly() =>
                    _actualRawEarnings[0].EpisodeStartDate.Should().Be(_expectedRawEarnings[0].EpisodeStartDate?.Date);

                [Test]
                public void ThenEpisodeEffectiveTnpStartDateIsSetCorrectly() =>
                    _actualRawEarnings[0].EpisodeEffectiveTnpStartDate.Should().Be(_expectedRawEarnings[0].EpisodeEffectiveTnpStartDate?.Date);

                [Test]
                public void ThenPeriodIsSetCorrectly() =>
                    _actualRawEarnings[0].Period.Should().Be(_expectedRawEarnings[0].Period);

                [Test]
                public void ThenUlnIsSetCorrectly() =>
                    _actualRawEarnings[0].Uln.Should().Be(_expectedRawEarnings[0].Uln);

                [Test]
                public void ThenProgTypeIsSetCorrectly() =>
                    _actualRawEarnings[0].ProgrammeType.Should().Be(_expectedRawEarnings[0].ProgrammeType);

                [Test]
                public void ThenFworkCodeIsSetCorrectly() =>
                    _actualRawEarnings[0].FrameworkCode.Should().Be(_expectedRawEarnings[0].FrameworkCode);

                [Test]
                public void ThenPwayCodeIsSetCorrectly() =>
                    _actualRawEarnings[0].PathwayCode.Should().Be(_expectedRawEarnings[0].PathwayCode);

                [Test]
                public void ThenStdCodeIsSetCorrectly() =>
                    _actualRawEarnings[0].StandardCode.Should().Be(_expectedRawEarnings[0].StandardCode);

                [Test]
                public void ThenPriceEpisodeSfaContribPctIsSetCorrectly() =>
                    _actualRawEarnings[0].SfaContributionPct.Should().Be(_expectedRawEarnings[0].SfaContributionPct);

                [Test]
                public void ThenPriceEpisodeFundLineTypeIsSetCorrectly() =>
                    _actualRawEarnings[0].FundingLineType.Should().Be(_expectedRawEarnings[0].FundingLineType);

                [Test]
                public void ThenLearnAimRefIsSetCorrectly() =>
                    _actualRawEarnings[0].LearnAimRef.Should().Be(_expectedRawEarnings[0].LearnAimRef);

                [Test]
                public void ThenLearnStartDateIsSetCorrectly() =>
                    _actualRawEarnings[0].LearningStartDate.Should().Be(_expectedRawEarnings[0].LearningStartDate.Date);

                [Test]
                public void ThenTransactionType01IsSetCorrectly() => 
                    _actualRawEarnings[0].TransactionType01.Should().Be(_expectedRawEarnings[0].TransactionType01);

                [Test]
                public void ThenTransactionType02IsSetCorrectly() => 
                    _actualRawEarnings[0].TransactionType02.Should().Be(_expectedRawEarnings[0].TransactionType02);

                [Test]
                public void ThenTransactionType03IsSetCorrectly() => 
                    _actualRawEarnings[0].TransactionType03.Should().Be(_expectedRawEarnings[0].TransactionType03);

                [Test]
                public void ThenTransactionType04IsSetCorrectly() => 
                    _actualRawEarnings[0].TransactionType04.Should().Be(_expectedRawEarnings[0].TransactionType04);

                [Test]
                public void ThenTransactionType05IsSetCorrectly() => 
                    _actualRawEarnings[0].TransactionType05.Should().Be(_expectedRawEarnings[0].TransactionType05);

                [Test]
                public void ThenTransactionType06IsSetCorrectly() => 
                    _actualRawEarnings[0].TransactionType06.Should().Be(_expectedRawEarnings[0].TransactionType06);

                [Test]
                public void ThenTransactionType07IsSetCorrectly() => 
                    _actualRawEarnings[0].TransactionType07.Should().Be(_expectedRawEarnings[0].TransactionType07);

                [Test]
                public void ThenTransactionType08IsSetCorrectly() => 
                    _actualRawEarnings[0].TransactionType08.Should().Be(_expectedRawEarnings[0].TransactionType08);

                [Test]
                public void ThenTransactionType09IsSetCorrectly() => 
                    _actualRawEarnings[0].TransactionType09.Should().Be(_expectedRawEarnings[0].TransactionType09);

                [Test]
                public void ThenTransactionType10IsSetCorrectly() => 
                    _actualRawEarnings[0].TransactionType10.Should().Be(_expectedRawEarnings[0].TransactionType10);

                [Test]
                public void ThenTransactionType11IsSetCorrectly() => 
                    _actualRawEarnings[0].TransactionType11.Should().Be(_expectedRawEarnings[0].TransactionType11);

                [Test]
                public void ThenTransactionType12IsSetCorrectly() => 
                    _actualRawEarnings[0].TransactionType12.Should().Be(_expectedRawEarnings[0].TransactionType12);

                [Test]
                public void ThenTransactionType15IsSetCorrectly() => 
                    _actualRawEarnings[0].TransactionType15.Should().Be(_expectedRawEarnings[0].TransactionType15);

                [Test]
                public void ThenActIsSetCorrectly() =>
                    _actualRawEarnings[0].Act.Should().Be(_expectedRawEarnings[0].Act);
            }
        }
    }
}