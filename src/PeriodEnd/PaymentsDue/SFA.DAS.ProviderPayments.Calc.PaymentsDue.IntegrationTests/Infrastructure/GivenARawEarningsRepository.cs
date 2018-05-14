using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Tools;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Infrastructure
{
    [TestFixture]
    public class GivenARawEarningsRepository
    {
        [TestFixture]
        public class WhenCallingGetAllForProvider
        {
            [SetUp]
            public void Setup()
            {
                RawEarningsDataHelper.CreateRawEarning(null);
                RawEarningsDataHelper.CreateRawEarning(null);
                RawEarningsDataHelper.CreateRawEarning(null);

                //_result = SomeItemsConstraint.Stuff()
            }

            [Test, AutoData]
            public void ThenItRetrievesExpectedCount(List<RawEarning> earnings)
            {
                earnings.Count.Should().Be(3);
            }

            [Test]
            public void ThenUkprnIsSetCorrectly()
            {

            }

            // etc
        }
    }
}