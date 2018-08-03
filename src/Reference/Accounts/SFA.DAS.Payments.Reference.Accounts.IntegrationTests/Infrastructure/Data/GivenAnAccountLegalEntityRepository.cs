using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.Payments.Reference.Accounts.Infrastructure.Data.Dcfs;
using SFA.DAS.Payments.Reference.Accounts.Infrastructure.Data.Entities;
using SFA.DAS.Payments.Reference.Accounts.IntegrationTests.DataHelpers;

namespace SFA.DAS.Payments.Reference.Accounts.IntegrationTests.Infrastructure.Data
{
    [TestFixture]
    public class GivenAnAccountLegalEntityRepository
    {
        private DcfsAccountLegalEntityRepository _sut;

        [OneTimeSetUp]
        public void CreateSut()
        {
            var contextWrapper = new ContextWrapper(new IntegrationTaskContext());
            _sut = new DcfsAccountLegalEntityRepository(contextWrapper);
        }

        [TestFixture]
        public class WhenCallingAddMany : GivenAnAccountLegalEntityRepository
        {
            private List<AccountLegalEntityEntity> _expectedEntities;
            private List<AccountLegalEntityEntity> _actualEntities;

            [OneTimeSetUp]
            public void MakeTheCall()
            {
                _expectedEntities = new Fixture()
                    .Build<AccountLegalEntityEntity>()
                    .CreateMany()
                    .OrderBy(entity => entity.Id)
                    .ToList();

                AccountLegalEntityDataHelper.Truncate();

                _sut.AddMany(_expectedEntities);

                _actualEntities = AccountLegalEntityDataHelper.GetAll();
            }

            [Test]
            public void ThenItSavesTheExpectedNumberOfEntities() =>
                _actualEntities.Count
                    .Should().Be(_expectedEntities.Count);

            [Test]
            public void ThenItSetsId() =>
                _actualEntities[0].Id
                    .Should().Be(_expectedEntities[0].Id);

            [Test]
            public void ThenItSetsPublicHashedId() =>
                _actualEntities[0].PublicHashedId
                    .Should().Be(_expectedEntities[0].PublicHashedId);

            [Test]
            public void ThenItSetsAccountId() =>
                _actualEntities[0].AccountId
                    .Should().Be(_expectedEntities[0].AccountId);

            [Test]
            public void ThenItSetsLegalEntityId() =>
                _actualEntities[0].LegalEntityId
                    .Should().Be(_expectedEntities[0].LegalEntityId);
        }
    }
}