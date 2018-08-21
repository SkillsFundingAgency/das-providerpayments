using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Repositories;
using SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Attributes;
using SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Attributes.Datalocks;

namespace SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Tests.Infrastructure
{
    [TestFixture, SetupUkprn]
    public class GivenADatalockRepository
    {
        private DatalockRepository _sut;

        [OneTimeSetUp]
        public void BuildSut()
        {
            _sut = new DatalockRepository(GlobalTestContext.Instance.TransientConnectionString);
        }

        [TestFixture]
        public class WhenCallingGetDatalockOutputForProvider : GivenADatalockRepository
        {
            [TestFixture, SetupNoDatalocks]
            public class AndThereAreNoDatalocksForProvider : WhenCallingGetDatalockOutputForProvider
            {
                [Test]
                public void ThenItReturnsAnEmptyList()
                {
                    var result =
                        _sut.GetDatalockOutputForProvider(SharedTestContext.Ukprn);
                    result.Should().BeEmpty();
                    }
            }

            [TestFixture, SetupDatalocks]
            public class AndThereAreSomeDatalocksForProvider : WhenCallingGetDatalockOutputForProvider
            {
                private List<DatalockOutputEntity> _actualDataLocks;
                private List<DatalockOutputEntity> _expectedDataLocks;

                [SetUp]
                public void Setup()
                {
                    _actualDataLocks = _sut.GetDatalockOutputForProvider(SharedTestContext.Ukprn);

                    _expectedDataLocks = SharedTestContext.DataLockPriceEpisodePeriodMatches
                        .Where(entity => entity.Ukprn == SharedTestContext.Ukprn)
                        .OrderBy(entity => entity.PriceEpisodeIdentifier)
                        .ThenBy(x => x.LearnRefNumber)      // there's a sorted index on the table
                        .ToList();
                }

                [Test]
                public void ThenItRetrievesExpectedCount()
                {
                    if (_expectedDataLocks.Count < 1)
                        Assert.Fail("Not enough earnings to test");

                    _actualDataLocks.Count.Should().Be(_expectedDataLocks.Count);
                }

                [Test]
                public void ThenUkprnIsSetCorrectly() =>
                    _actualDataLocks[0].Ukprn.Should().Be(_expectedDataLocks[0].Ukprn);

                [Test]
                public void ThenPriceEpisodeIdentifierIsSetCorrectly() =>
                    _actualDataLocks[0].PriceEpisodeIdentifier.Should().Be(_expectedDataLocks[0].PriceEpisodeIdentifier);

                [Test]
                public void ThenLearnRefNumberIsSetCorrectly() =>
                    _actualDataLocks[0].LearnRefNumber.Should().Be(_expectedDataLocks[0].LearnRefNumber);

                [Test]
                public void ThenAimSeqNumberIsSetCorrectly() =>
                    _actualDataLocks[0].AimSeqNumber.Should().Be(_expectedDataLocks[0].AimSeqNumber);

                [Test]
                public void ThenCommitmentIdIsSetCorrectly() =>
                    _actualDataLocks[0].CommitmentId.Should().Be(_expectedDataLocks[0].CommitmentId);

                [Test]
                public void ThenVersionIdIsSetCorrectly() =>
                    _actualDataLocks[0].VersionId.Should().Be(_expectedDataLocks[0].VersionId);

                [Test]
                public void ThenPeriodIsSetCorrectly() =>
                    _actualDataLocks[0].Period.Should().Be(_expectedDataLocks[0].Period);

                [Test]
                public void ThenPayableIsSetCorrectly() =>
                    _actualDataLocks[0].Payable.Should().Be(_expectedDataLocks[0].Payable);

                [Test]
                public void ThenTransactionTypeIsSetCorrectly() =>
                    _actualDataLocks[0].TransactionType.Should().Be(_expectedDataLocks[0].TransactionType);

                [Test]
                public void ThenTransactionTypesFlagIsSetCorrectly() =>
                    _actualDataLocks[0].TransactionTypesFlag.Should().Be(_expectedDataLocks[0].TransactionTypesFlag);
            }
        }

        [TestFixture]
        public class WhenCallingGetValidationErrorsForProvider : GivenADatalockRepository
        {
            [TestFixture, SetupNoValidationErrors]
            public class AndThereAreNoValidationErrorsForProvider : WhenCallingGetValidationErrorsForProvider
            {
                [Test]
                public void ThenItReturnsAnEmptyList()
                {
                    var result =
                        _sut.GetValidationErrorsForProvider(SharedTestContext.Ukprn);
                    result.Should().BeEmpty();
                }
            }

            [TestFixture, SetupValidationErrors]
            public class AndThereAreValidationErrorsForProvider : WhenCallingGetValidationErrorsForProvider
            {
                private List<DatalockValidationError> _actualValidationErrors;
                private List<DatalockValidationError> _expectedValidationErrors;

                [SetUp]
                public void Setup()
                {
                    _actualValidationErrors = _sut.GetValidationErrorsForProvider(SharedTestContext.Ukprn);

                    _expectedValidationErrors = SharedTestContext.DatalockValidationErrors
                        .Where(entity => entity.Ukprn == SharedTestContext.Ukprn)
                        .OrderBy(entity => entity.PriceEpisodeIdentifier)
                        .ThenBy(x => x.LearnRefNumber)      // there's a sorted index on the table
                        .ToList();
                }

                [Test]
                public void ThenItRetrievesExpectedCount()
                {
                    if (_expectedValidationErrors.Count < 1)
                        Assert.Fail("Not enough earnings to test");

                    _actualValidationErrors.Count.Should().Be(_expectedValidationErrors.Count);
                }

                [Test]
                public void ThenUkprnIsSetCorrectly() =>
                    _actualValidationErrors[0].Ukprn.Should().Be(_expectedValidationErrors[0].Ukprn);

                [Test]
                public void ThenPriceEpisodeIdentifierIsSetCorrectly() =>
                    _actualValidationErrors[0].PriceEpisodeIdentifier.Should().Be(_expectedValidationErrors[0].PriceEpisodeIdentifier);

                [Test]
                public void ThenLearnRefNumberIsSetCorrectly() =>
                    _actualValidationErrors[0].LearnRefNumber.Should().Be(_expectedValidationErrors[0].LearnRefNumber);

                [Test]
                public void ThenAimSeqNumberIsSetCorrectly() =>
                    _actualValidationErrors[0].AimSeqNumber.Should().Be(_expectedValidationErrors[0].AimSeqNumber);

                [Test]
                public void ThenRuleIdIsSetCorrectly() =>
                    _actualValidationErrors[0].RuleId.Should().Be(_expectedValidationErrors[0].RuleId);
            }
        }
    }
}