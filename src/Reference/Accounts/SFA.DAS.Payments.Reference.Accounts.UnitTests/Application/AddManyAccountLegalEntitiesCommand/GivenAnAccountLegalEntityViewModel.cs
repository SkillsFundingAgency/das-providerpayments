using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.Payments.Reference.Accounts.Application.AddManyAccountLegalEntitiesCommand;

namespace SFA.DAS.Payments.Reference.Accounts.UnitTests.Application.AddManyAccountLegalEntitiesCommand
{
    [TestFixture]
    public class GivenAnAccountLegalEntityViewModel
    {
        [TestFixture]
        public class WhenCallingToEntity
        {
            [Test, AccountsAutoData]
            public void ThenMapsId(AccountLegalEntityViewModel sut) =>
                sut.ToEntity().Id
                    .Should().Be(sut.AccountLegalEntityId);

            [Test, AccountsAutoData]
            public void ThenMapsPublicHashedId(AccountLegalEntityViewModel sut) =>
                sut.ToEntity().PublicHashedId
                    .Should().Be(sut.AccountLegalEntityPublicHashedId);

            [Test, AccountsAutoData]
            public void ThenMapsAccountId(AccountLegalEntityViewModel sut) =>
                sut.ToEntity().AccountId
                    .Should().Be(sut.AccountId);

            [Test, AccountsAutoData]
            public void ThenMapsLegalEntityId(AccountLegalEntityViewModel sut) =>
                sut.ToEntity().LegalEntityId
                    .Should().Be(sut.LegalEntityId);
        }
    }
}