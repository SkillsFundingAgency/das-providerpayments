using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Utilities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Infrastructure
{
    [TestFixture]
    public class GivenARawEarningsRepository
    {
        [TestFixture, SetupRawEarnings]
        public class WhenCallingGetAllForProvider : IExpectedRawEarnings
        {
            public List<RawEarning> RawEarnings { get; set; }

            [SetUp]
            public void Setup()
            {
                //_result = SomeItemsConstraint.Stuff()
            }

            [Test]
            public void ThenItRetrievesExpectedCount()
            {
                RawEarnings.Count.Should().Be(3);
            }

            [Test]
            public void ThenUkprnIsSetCorrectly()
            {

            }

            // etc
        }
    }
}