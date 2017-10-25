using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Accounts;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Accounts.AllocateLevyCommand;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.UnitTests.Application.Accounts.AllocateLevyCommand.AllocateLevyCommandHandler.Handle
{
    public class WhenHandlingRequestForAccountWithEnoughLevy
    {
        private const long AccountId = 1;
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
                    Id = AccountId.ToString()
                },
                AmountRequested = AmountRequested
            };

            _accountRepository = new Mock<IAccountRepository>();
            _accountRepository.Setup(r => r.GetAccountById(AccountId))
                .Returns(new AccountEntity
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
            _accountRepository.Verify(r => r.UpdateLevyBalance(AccountId, AmountRequested), Times.Once);
        }
    }
}
