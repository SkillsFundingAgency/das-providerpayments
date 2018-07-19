using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Payments.Reference.Accounts.Application.AddAuditCommand;

namespace SFA.DAS.Payments.Reference.Accounts.UnitTests.Application.AddAuditCommand
{
    [TestFixture]
    public class GivenAnAddAuditCommandRequest
    {
        [TestFixture]
        public class AndCallingToAuditEntity
        {
            [Test, AccountsAutoData]
            public void ThenMapsReadDateTime(AddAuditCommandRequest sut) =>
                sut.ToAuditEntity().ReadDateTime
                    .Should().Be(sut.CorrelationDate);

            [Test, AccountsAutoData]
            public void ThenMapsAccountsRead(AddAuditCommandRequest sut) =>
                sut.ToAuditEntity().AccountsRead
                    .Should().Be(sut.AccountsRead);

            [Test, AccountsAutoData]
            public void ThenMapsCompletedSuccessfully(AddAuditCommandRequest sut) =>
                sut.ToAuditEntity().CompletedSuccessfully
                    .Should().Be(sut.CompletedSuccessfully);

            [Test, AccountsAutoData]
            public void ThenMapsAuditType(AddAuditCommandRequest sut) =>
                sut.ToAuditEntity().AuditType
                    .Should().Be((short)sut.AuditType);
        }
    }
}