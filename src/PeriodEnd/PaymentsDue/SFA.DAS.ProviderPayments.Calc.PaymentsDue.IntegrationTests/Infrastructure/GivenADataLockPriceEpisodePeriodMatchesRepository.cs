using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Utilities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Infrastructure
{
    [TestFixture, SetupUkprn]
    public class GivenADataLockPriceEpisodePeriodMatchesRepository
    {
        private DataLockPriceEpisodePeriodMatchesRepository _sut;

        [OneTimeSetUp]
        public void Setup()
        {
            _sut = new DataLockPriceEpisodePeriodMatchesRepository(GlobalTestContext.Instance.TransientConnectionString);
        }

        [TestFixture, SetupNoDataLockPriceEpisodePeriodMatches]
        public class AndThereAreNoDataLocksForProvider : GivenADataLockPriceEpisodePeriodMatchesRepository
        {
            [TestFixture]
            public class WhenCallingGetAllForProvider : AndThereAreNoDataLocksForProvider
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
    }

    public class DataLockPriceEpisodePeriodMatchesRepository: DcfsRepository
    {
        public DataLockPriceEpisodePeriodMatchesRepository(string connectionString) : base(connectionString)
        {
        }

        public List<DataLockPriceEpisodePeriodMatchEntity> GetAllForProvider(long ukprn)
        {
            return new List<DataLockPriceEpisodePeriodMatchEntity>();
        }
    }
}