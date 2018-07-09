using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;
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

        [TestFixture]
        public class WhenCallingAddMany : GivenADasAccountRepository
        {
            private List<DasAccountEntity> _expectedEntities;
            private List<DasAccountEntity> _actualEntities;

            [OneTimeSetUp]
            public void Setup()
            {
                _expectedEntities = new Fixture()
                    .Build<DasAccountEntity>()
                    .CreateMany()
                    .OrderBy(entity => entity.AccountId)
                    .ToList();

                DasAccountDataHelper.Truncate();

                _sut.AddMany(_expectedEntities);

                _actualEntities = DasAccountDataHelper.GetAll().ToList();
            }

            [Test]
            public void ThenItSavesTheExpectedNumberOfEntities() =>
                _actualEntities.Count
                    .Should().Be(_expectedEntities.Count);

            [Test]
            public void ThenItSetsAccountId() =>
                _actualEntities[0].AccountId
                    .Should().Be(_expectedEntities[0].AccountId);

            [Test]
            public void ThenItSetsAccountHashId() =>
                _actualEntities[0].AccountHashId
                    .Should().Be(_expectedEntities[0].AccountHashId);

            [Test]
            public void ThenItSetsAccountName() =>
                _actualEntities[0].AccountName
                    .Should().Be(_expectedEntities[0].AccountName);

            [Test]
            public void ThenItSetsBalance() =>
                _actualEntities[0].Balance
                    .Should().Be(_expectedEntities[0].Balance);

            [Test]
            public void ThenItSetsVersionId() =>
                _actualEntities[0].VersionId
                    .Should().Be(_expectedEntities[0].VersionId);

            [Test]
            public void ThenItSetsIsLevyPayer() =>
                _actualEntities[0].IsLevyPayer
                    .Should().Be(_expectedEntities[0].IsLevyPayer);

            [Test]
            public void ThenItSetsTransferAllowance() =>
                _actualEntities[0].TransferAllowance
                    .Should().Be(_expectedEntities[0].TransferAllowance);
        }

        [TestFixture, SetupDasAccounts]
        public class WhenCallingUpdateBalance : GivenADasAccountRepository
        {
            [Test, AutoData]
            public void ThenItUpdatesTheBalance(decimal newBalance)
            {
                var entityBeforeUpdate = DasAccountDataHelper
                    .GetAll()
                    .Single(entity => entity.AccountId == SharedTestContext.AccountId);
                
                _sut.UpdateBalance(SharedTestContext.AccountId, newBalance);

                var entityAfterUpdate = DasAccountDataHelper
                    .GetAll()
                    .Single(entity => entity.AccountId == SharedTestContext.AccountId);

                entityBeforeUpdate.Balance.Should().NotBe(newBalance);
                entityAfterUpdate.Balance.Should().Be(newBalance);
            }
        }
    }
}