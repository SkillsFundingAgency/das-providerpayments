using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Accounts.GetNextAccountQuery;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Infrastructure.Data;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.UnitTests.Application.Accounts.GetNextAccountQuery.Handle
{
    public class WhenHandlingValidRequest
    {
        private const string AccountId = "ACC001";
        private const string AccountName = "Account 1";
        private const string CommitmentId = "Commtiment123";

        private GetNextAccountQueryRequest _request;
        private Mock<IAccountRepository> _accountRepository;
        private Mock<ICommitmentRepository> _commitmentRepository;
        private GetNextAccountQueryHandler _handler;

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

            _handler = new GetNextAccountQueryHandler(_accountRepository.Object, _commitmentRepository.Object);
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
