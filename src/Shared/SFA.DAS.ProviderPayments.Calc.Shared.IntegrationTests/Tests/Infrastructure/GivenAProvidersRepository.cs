using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Repositories;
using SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Attributes;

namespace SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Tests.Infrastructure
{
    [TestFixture, SetupProviderRepository(2)]
    public class GivenAProvidersRepository
    {
        private ProviderRepository _sut;

        [OneTimeSetUp]
        public void Setup()
        {
            _sut = new ProviderRepository(GlobalTestContext.Instance.TransientConnectionString);
        }

        [Test]
        public void ThenReturnsAPopulatedList()
        {
            var result = _sut.GetAllProviders();
            result.Count().Should().Be(2);
        }

        [Test]
        public void ThenTheUkprnsMatch()
        {
            var result = _sut.GetAllProviders().OrderBy(x=>x.Ukprn).ToList();
            result.First().Ukprn.Should().Be(1);
            result.Last().Ukprn.Should().Be(2);
        }
    }
}