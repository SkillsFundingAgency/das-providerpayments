using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Accounts.GetNextAccountQuery;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.UnitTests.Application.Accounts.GetNextAccountQuery.GetNextAccountQueryHandler.Handle
{
    public class WhenHandlingRequestWhereNextAccountHasNoCommitments
    {
        private const string AccountId = "ACC001";
        private const string AccountName = "Account 1";
        private static readonly CommitmentEntity[][] _emptyCommitmentListExamples = new[]
        {
            (CommitmentEntity[]) null,
            new CommitmentEntity[0]
        };

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

        [TestCaseSource(nameof(_emptyCommitmentListExamples))]
        public void ThenTheAccountShouldHaveAnEmptyCommitmentList(CommitmentEntity[] commitments)
        {
            // Arrange
            _commitmentRepository.Setup(r => r.GetCommitmentsForAccount(AccountId))
                .Returns(commitments);

            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual?.Account?.Commitments);
            Assert.AreEqual(0, actual.Account.Commitments.Length);
        }
    }
}
