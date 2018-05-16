using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
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
                private List<RawEarning> _actualRawEarnings;
                private List<RawEarning> _expectedRawEarnings;

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
                public void ThenLearnRefNumberIsSetCorrectly()
                {
                    _actualRawEarnings[0].LearnRefNumber.Should().Be(_expectedRawEarnings[0].LearnRefNumber);
                }

                [Test]
                public void ThenUkprnIsSetCorrectly()
                {
                    _actualRawEarnings[0].Ukprn.Should().Be(_expectedRawEarnings[0].Ukprn);
                }

                // etc
            }
        }
    }

    public class RawEarningsRepository : DcfsRepository
    {
        public RawEarningsRepository(string connectionString) : base(connectionString)
        {
        }

        public List<RawEarning> GetAllForProvider(long ukprn)
        {
            const string sql = @"
            SELECT *
            FROM Staging.RawEarnings
            WHERE Ukprn = @ukprn";

            var result = Query<RawEarning>(sql, new {ukprn})
                .ToList();

            return result;
        }
    }
}