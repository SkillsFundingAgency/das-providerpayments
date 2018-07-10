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
            private decimal _adjustment;

            [SetUp]
            public void Setup()
            {
                var fixture = new Fixture();
                _adjustment = fixture.Create<decimal>();

                _entitiesBeforeUpdate = DasAccountDataHelper.GetAll().ToList();

                _sut.AdjustBalance(SharedTestContext.AccountId, _adjustment);

                _entitiesAfterUpdate = DasAccountDataHelper.GetAll().ToList();
            }

            [Test]
            public void ThenItAdjustsTheBalanceOfTheCorrectAccount()
            {
                var entityBeforeUpdate = _entitiesBeforeUpdate
                    .Single(entity => entity.AccountId == SharedTestContext.AccountId);
                var entityAfterUpdate = _entitiesAfterUpdate
                    .Single(entity => entity.AccountId == SharedTestContext.AccountId);
                var expectedAdjustedBalance = entityBeforeUpdate.Balance + _adjustment;

                entityBeforeUpdate.Balance.Should().NotBe(expectedAdjustedBalance);
                entityAfterUpdate.Balance.Should().Be(expectedAdjustedBalance);
            }

            [Test]
            public void ThenItDoesNotAdjustOtherAccounts()
            {
                _entitiesAfterUpdate
                   .Count(entity => entity.Balance.HasValue 
                    && entity.Balance == GetPriorBalance(entity.AccountId) + _adjustment)
                    .Should().Be(1);
            }

            private decimal? GetPriorBalance(long accountId)
            {
                return _entitiesBeforeUpdate.Single(entity => entity.AccountId == accountId).Balance;
            }
        }
    }
}