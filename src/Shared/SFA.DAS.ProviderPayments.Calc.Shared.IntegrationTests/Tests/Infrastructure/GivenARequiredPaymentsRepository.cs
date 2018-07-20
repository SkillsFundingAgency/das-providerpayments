using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Repositories;
using SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Attributes;

namespace SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Tests.Infrastructure
{
    [TestFixture, SetupRequiredPaymentsRepository(1002)]
    public class GivenARequiredPaymentsRepository
    {
        private RequiredPaymentRepository _sut;

        [OneTimeSetUp]
        public void Setup()
        {
            _sut = new RequiredPaymentRepository(GlobalTestContext.Instance.TransientConnectionString);
        }

        [Test]
        public void ThenReturnsAllRefundsForProvider()
        {
            var result = _sut.GetRefundsForProvider(1002);
            result.Count().Should().Be(3);
        }

        [Test]
        public void ThenTheAmountDueValuesAreNegative()
        {
            var result = _sut.GetRefundsForProvider(1002).ToList();
            result[0].AmountDue.Should().BeLessThan(0);
            result[1].AmountDue.Should().BeLessThan(0);
            result[2].AmountDue.Should().BeLessThan(0);
        }

        [Test]
        public void ThenReturnsNoRefundsForAnotherProvider()
        {
            var result = _sut.GetRefundsForProvider(999);
            result.Count().Should().Be(0);
        }
    }
}