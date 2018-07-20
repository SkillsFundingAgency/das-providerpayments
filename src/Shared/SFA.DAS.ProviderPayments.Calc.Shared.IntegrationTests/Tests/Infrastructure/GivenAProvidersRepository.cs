using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Repositories;
using SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Attributes;

namespace SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Tests.Infrastructure
{
    [TestFixture]
    public class GivenAProvidersRepository
    {
        private ProviderRepository _sut;

        [OneTimeSetUp]
        public void Setup()
        {
            _sut = new ProviderRepository(GlobalTestContext.Instance.TransientConnectionString);
        }

        [TestFixture, SetupProviderRepository(2)]
        public class AndProvidersExist : GivenAProvidersRepository
        {
            [Test]
            public void ThenReturnsAPopulatedList()
            {
                var result = _sut.GetAllProviders().ToList();
                result.Count().Should().Be(2);
            }

            [Test]
            public void ThenTheUkprnsMatch()
            {
                var result = _sut.GetAllProviders().OrderBy(x => x.Ukprn).ToList();
                result.First().Ukprn.Should().Be(1);
                result.Last().Ukprn.Should().Be(2);
            }

            [Test]
            public void ThenTheIlrSubmissionDateTimeMatch()
            {
                var result = _sut.GetAllProviders().OrderBy(x => x.Ukprn).ToList();
                result.First().IlrSubmissionDateTime.Should().Be(new DateTime(2018, 01, 01));
            }
        }

        [TestFixture, SetupProviderRepository(0)]
        public class AndNoProvidersExist : GivenAProvidersRepository
        {
            [Test]
            public void ThenReturnsAPopulatedList()
            {
                var result = _sut.GetAllProviders();
                result.Count().Should().Be(0);
            }

        }
    }
}