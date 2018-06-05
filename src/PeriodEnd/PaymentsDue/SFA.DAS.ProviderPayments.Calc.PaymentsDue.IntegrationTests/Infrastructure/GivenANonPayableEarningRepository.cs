using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Repositories;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Utilities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Infrastructure
{
    [TestFixture, SetupUkprn]
    public class GivenANonPayableEarningRepository
    {
        private NonPayableEarningRepository _sut;
        
        [OneTimeSetUp]
        public void Setup()
        {
            _sut = new NonPayableEarningRepository(GlobalTestContext.Instance.TransientConnectionString);
        }

        [TestFixture, SetupNoNonPayableEarnings]
        public class AndThereAreNoNonPayableEarnings : GivenANonPayableEarningRepository
        {
            [TestFixture]
            public class WhenCallingAddMany : AndThereAreNoNonPayableEarnings
            {
                private List<NonPayableEarningEntity> _expectedEntities;
                private List<NonPayableEarningEntity> _actualEntities;

                [OneTimeSetUp]
                public new void Setup()
                {
                    base.Setup();

                    _expectedEntities = new Fixture()
                        .Build<NonPayableEarningEntity>()
                        .CreateMany()
                        .ToList();

                    _sut.AddMany(_expectedEntities);

                    _actualEntities = NonPayableEarningsDataHelper.GetAll();
                }

                [Test]
                public void ThenItSavesTheExpectedNumberOfEntities() =>
                    _actualEntities.Count
                        .Should().Be(_expectedEntities.Count);

                [Test]
                public void ThenItSetsCommitmentId() =>
                    _actualEntities[0].CommitmentId
                        .Should().Be(_expectedEntities[0].CommitmentId);
            }
        }
    }
}