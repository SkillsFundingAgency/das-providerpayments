using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Accounts.GetNextAccountQuery;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.UnitTests.Application.Accounts.GetNextAccountQuery.GetNextAccountQueryHandler.Handle
{
    public class WhenHandlingValidRequest
    {
        private const string AccountId = "ACC001";
        private const string AccountName = "Account 1";
        private const long CommitmentId = 123;

        private GetNextAccountQueryRequest _request;
        private Mock<IAccountRepository> _accountRepository;
        private Mock<ICommitmentRepository> _commitmentRepository;
        private LevyPayments.Application.Accounts.GetNextAccountQuery.GetNextAccountQueryHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _request = new GetNextAccountQueryRequest();

            _accountRepository = new Mock<IAccountRepository>();
            _accountRepository.Setup(r => r.GetNextAccountRequiringProcessing())
                .Returns(new AccountEntity
                {
                    Id = AccountId,
                    Name = AccountName
                });

            _commitmentRepository = new Mock<ICommitmentRepository>();
            _commitmentRepository.Setup(r => r.GetCommitmentsForAccount(AccountId))
                .Returns(new[]
                {
                    new CommitmentEntity { Id = CommitmentId }
                });

            _handler = new LevyPayments.Application.Accounts.GetNextAccountQuery.GetNextAccountQueryHandler(_accountRepository.Object, _commitmentRepository.Object);
        }

        [Test]
        public void ThenItShouldReturnAnInstanceOfGetNextAccountQueryResponse()
        {
            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual);
        }

        [Test]
        public void ThenTheResponseShouldTheAccount()
        {
            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual.Account);
            Assert.AreEqual(AccountId, actual.Account.Id);
            Assert.AreEqual(AccountName, actual.Account.Name);
        }

        [Test]
        public void ThenTheAccountShouldHaveItsCommitments()
        {
            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual?.Account?.Commitments);
            Assert.AreEqual(1, actual.Account.Commitments.Length);
            Assert.AreEqual(CommitmentId, actual.Account.Commitments[0].Id);
        }

    }
}
