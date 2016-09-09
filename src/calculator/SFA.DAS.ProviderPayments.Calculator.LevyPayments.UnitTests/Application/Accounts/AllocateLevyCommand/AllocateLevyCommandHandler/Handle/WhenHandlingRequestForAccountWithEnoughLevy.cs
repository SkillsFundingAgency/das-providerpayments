using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Accounts.AllocateLevyCommand;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Infrastructure.Data;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.UnitTests.Application.Accounts.AllocateLevyCommand.AllocateLevyCommandHandler.Handle
{
    public class WhenHandlingRequestForAccountWithEnoughLevy
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
                Account = new LevyPayments.Application.Accounts.Account
                {
                    Id = AccountId
                },
                AmountRequested = AmountRequested
            };

            _accountRepository = new Mock<IAccountRepository>();
            _accountRepository.Setup(r => r.GetAccountById(AccountId))
                .Returns(new Infrastructure.Data.Entities.AccountEntity
                {
                    Id = AccountId,
                    Balance = 1234567
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

        [Test]
        public void ThenItShouldReturnTheSpendAsTheAmountRequested()
        {
            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.AreEqual(AmountRequested, actual.AmountAllocated);
        }

        [Test]
        public void ThenItShouldUpdateTheRepositoryWithTheSpend()
        {
            // Act
            _handler.Handle(_request);

            // Assert
            _accountRepository.Verify(r => r.SpendLevy(AccountId, AmountRequested), Times.Once);
        }
    }
}
