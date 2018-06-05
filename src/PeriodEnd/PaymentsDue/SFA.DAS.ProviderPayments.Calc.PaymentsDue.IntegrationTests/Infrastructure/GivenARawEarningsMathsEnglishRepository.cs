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
    public class GivenARawEarningsMathsEnglishRepository
    {
        private RawEarningsMathsEnglishRepository _sut;

        [OneTimeSetUp]
        public void Setup()
        {
            _sut = new RawEarningsMathsEnglishRepository(GlobalTestContext.Instance.TransientConnectionString);
        }

        [TestFixture, SetupNoRawEarningsMathsEnglish]
        public class AndThereAreNoRawEarningsForProvider : GivenARawEarningsMathsEnglishRepository
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

        [TestFixture, SetupRawEarningsMathsEnglish]
        public class AndThereAreSomeRawEarningsForProvider : GivenARawEarningsMathsEnglishRepository
        {
            [TestFixture]
            public class WhenCallingGetAllForProvider : AndThereAreSomeRawEarningsForProvider
            {
                private List<RawEarningMathsEnglishEntity> _actualRawEarningsMathsEnglish;
                private List<RawEarningMathsEnglishEntity> _expectedRawEarningsMathsEnglish;

                [SetUp]
                public new void Setup()
                {
                    base.Setup();
                    _actualRawEarningsMathsEnglish = _sut.GetAllForProvider(PaymentsDueTestContext.Ukprn);

                    _expectedRawEarningsMathsEnglish = PaymentsDueTestContext.RawEarningsMathsEnglish
                        .Where(earning => earning.Ukprn == PaymentsDueTestContext.Ukprn).ToList();
                }

                [Test]
                public void ThenItRetrievesExpectedCount()
                {
                    if (_expectedRawEarningsMathsEnglish.Count < 1)
                        Assert.Fail("Not enough earnings to test");

                    _actualRawEarningsMathsEnglish.Count.Should().Be(_expectedRawEarningsMathsEnglish.Count);
                }

                [Test]
                public void ThenLearnRefNumberIsSetCorrectly() =>
                    _actualRawEarningsMathsEnglish[0].LearnRefNumber.Should().Be(_expectedRawEarningsMathsEnglish[0].LearnRefNumber);

                [Test]
                public void ThenUkprnIsSetCorrectly() => 
                    _actualRawEarningsMathsEnglish[0].Ukprn.Should().Be(_expectedRawEarningsMathsEnglish[0].Ukprn);

                [Test]
                public void ThenAimSeqNumberIsSetCorrectly() =>
                    _actualRawEarningsMathsEnglish[0].AimSeqNumber.Should().Be(_expectedRawEarningsMathsEnglish[0].AimSeqNumber);

                [Test]
                public void ThenLearnStartDateIsSetCorrectly() =>
                    _actualRawEarningsMathsEnglish[0].LearningStartDate.Should().Be(_expectedRawEarningsMathsEnglish[0].LearningStartDate.Date);

                [Test]
                public void ThenPeriodIsSetCorrectly() =>
                    _actualRawEarningsMathsEnglish[0].Period.Should().Be(_expectedRawEarningsMathsEnglish[0].Period);

                [Test]
                public void ThenUlnIsSetCorrectly() =>
                    _actualRawEarningsMathsEnglish[0].Uln.Should().Be(_expectedRawEarningsMathsEnglish[0].Uln);

                [Test]
                public void ThenProgTypeIsSetCorrectly() =>
                    _actualRawEarningsMathsEnglish[0].ProgrammeType.Should().Be(_expectedRawEarningsMathsEnglish[0].ProgrammeType);

                [Test]
                public void ThenFworkCodeIsSetCorrectly() =>
                    _actualRawEarningsMathsEnglish[0].FrameworkCode.Should().Be(_expectedRawEarningsMathsEnglish[0].FrameworkCode);

                [Test]
                public void ThenPwayCodeIsSetCorrectly() =>
                    _actualRawEarningsMathsEnglish[0].PathwayCode.Should().Be(_expectedRawEarningsMathsEnglish[0].PathwayCode);

                [Test]
                public void ThenStdCodeIsSetCorrectly() =>
                    _actualRawEarningsMathsEnglish[0].StandardCode.Should().Be(_expectedRawEarningsMathsEnglish[0].StandardCode);

                [Test]
                public void ThenLearnDelSfaContribPctIsSetCorrectly() =>
                    _actualRawEarningsMathsEnglish[0].SfaContributionPct.Should().Be(_expectedRawEarningsMathsEnglish[0].SfaContributionPct);

                [Test]
                public void ThenFundLineTypeIsSetCorrectly() =>
                    _actualRawEarningsMathsEnglish[0].FundingLineType.Should().Be(_expectedRawEarningsMathsEnglish[0].FundingLineType);

                [Test]
                public void ThenLearnAimRefIsSetCorrectly() =>
                    _actualRawEarningsMathsEnglish[0].LearnAimRef.Should().Be(_expectedRawEarningsMathsEnglish[0].LearnAimRef);

                [Test]
                public void ThenTransactionType13IsSetCorrectly() => 
                    _actualRawEarningsMathsEnglish[0].TransactionType13.Should().Be(_expectedRawEarningsMathsEnglish[0].TransactionType13);

                [Test]
                public void ThenTransactionType14IsSetCorrectly() => 
                    _actualRawEarningsMathsEnglish[0].TransactionType14.Should().Be(_expectedRawEarningsMathsEnglish[0].TransactionType14);

                [Test]
                public void ThenTransactionType15IsSetCorrectly() => 
                    _actualRawEarningsMathsEnglish[0].TransactionType15.Should().Be(_expectedRawEarningsMathsEnglish[0].TransactionType15);

                [Test]
                public void ThenActIsSetCorrectly() =>
                    _actualRawEarningsMathsEnglish[0].Act.Should().Be(_expectedRawEarningsMathsEnglish[0].Act);
            }
        }
    }
}