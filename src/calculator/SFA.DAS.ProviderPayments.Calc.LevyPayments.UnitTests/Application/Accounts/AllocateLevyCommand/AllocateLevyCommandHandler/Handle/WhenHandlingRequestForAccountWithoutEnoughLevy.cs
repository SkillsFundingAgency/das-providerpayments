using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Accounts;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Accounts.AllocateLevyCommand;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.UnitTests.Application.Accounts.AllocateLevyCommand.AllocateLevyCommandHandler.Handle
{
    public class WhenHandlingRequestForAccountWithoutEnoughLevy
    {
        private const string AccountId = "ACC01";
        private const int AmountRequested = 12345;

        private AllocateLevyCommandRequest _request;
        private Mock<IAccountRepository> _accountRepository;
        private LevyPayments.Application.Accounts.AllocateLevyCommand.AllocateLevyCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _request = new AllocateLevyCommandRequest
            {
                Account = new Account
                {
                    Id = AccountId
                },
                AmountRequested = AmountRequested
            };

            _accountRepository = new Mock<IAccountRepository>();
            _accountRepository.Setup(r => r.GetAccountById(AccountId))
                .Returns(new AccountEntity
                {
                    Id = AccountId,
                    Balance = 12345
                });

            _handler = new LevyPayments.Application.Accounts.AllocateLevyCommand.AllocateLevyCommandHandler(_accountRepository.Object);
        }

        [Test]
        public void ThenItShouldReturnAnInstanceOfAllocateLevyCommandResponse()
        {
            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(actual);
        }

        [TestCase(123, 100, 100)]
        [TestCase(123, 0, 0)]
        [TestCase(123, -100, 0)]
        public void ThenItShouldReturnTheSpendAsAmountOfLevy(decimal spend, decimal balance, decimal expectedSpend)
        {
            // Arrange
            _request.AmountRequested = spend;
            _accountRepository.Setup(r => r.GetAccountById(AccountId))
                .Returns(new AccountEntity
                {
                    Id = AccountId,
                    Balance = balance
                });

            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.AreEqual(expectedSpend, actual.AmountAllocated);
        }

        [TestCase(123, 100, 100)]
        [TestCase(123, 0, 0)]
        [TestCase(123, -100, 0)]
        public void ThenItShouldUpdateTheRepositoryWithTheSpend(decimal spend, decimal balance, decimal expectedSpend)
        {
            // Arrange
            _request.AmountRequested = spend;
            _accountRepository.Setup(r => r.GetAccountById(AccountId))
                .Returns(new AccountEntity
                {
                    Id = AccountId,
                    Balance = balance
                });

            // Act
            _handler.Handle(_request);

            // Assert
            _accountRepository.Verify(r => r.SpendLevy(AccountId, expectedSpend), Times.Once);
        }

    }
}
