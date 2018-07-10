using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Repositories;
using SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Attributes;
using SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Helpers;

namespace SFA.DAS.ProviderPayments.Calc.Shared.IntegrationTests.Tests.Infrastructure
{
    [TestFixture, SetupAccountId]
    public class GivenADasAccountRepository
    {
        private DasAccountRepository _sut;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _sut = new DasAccountRepository(GlobalTestContext.Instance.TransientConnectionString);
        }

        [TestFixture, SetupDasAccounts]
        public class WhenCallingAdjustBalance : GivenADasAccountRepository
        {
            private List<DasAccountEntity> _entitiesBeforeUpdate = new List<DasAccountEntity>();
            private List<DasAccountEntity> _entitiesAfterUpdate = new List<DasAccountEntity>();
            private decimal _newBalance;

            [SetUp]
            public void Setup()
            {
                var fixture = new Fixture();
                _newBalance = fixture.Create<decimal>();


                _entitiesBeforeUpdate = DasAccountDataHelper.GetAll().ToList();

                _sut.AdjustBalance(SharedTestContext.AccountId, _newBalance);

                _entitiesAfterUpdate = DasAccountDataHelper.GetAll().ToList();
            }

            [Test]
            public void ThenItUpdatesTheBalanceOfTheCorrectAccount()
            {
                var entityBeforeUpdate = _entitiesBeforeUpdate
                    .Single(entity => entity.AccountId == SharedTestContext.AccountId);
                var entityAfterUpdate = _entitiesAfterUpdate
                    .Single(entity => entity.AccountId == SharedTestContext.AccountId);

                entityBeforeUpdate.Balance.Should().NotBe(_newBalance);
                entityAfterUpdate.Balance.Should().Be(_newBalance);
            }

            [Test]
            public void ThenItDoesNotUpdateOtherAccounts()
            {
                _entitiesAfterUpdate.Count(entity => entity.Balance == _newBalance)
                    .Should().Be(1);
            }
        }
    }
}