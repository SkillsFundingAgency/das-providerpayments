using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Accounts.MarkAccountAsProcessedCommand;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.UnitTests.Application.Accounts.MarkAccountAsProcessedCommand.MarkAccountAsProcessedCommandHandler.Handle
{
    public class WhenHandlingAValidRequest
    {
        private const string AccountId = "ACC001";

        private MarkAccountAsProcessedCommandRequest _request;
        private Mock<IAccountRepository> _accountRepository;
        private LevyPayments.Application.Accounts.MarkAccountAsProcessedCommand.MarkAccountAsProcessedCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _request = new MarkAccountAsProcessedCommandRequest { AccountId = AccountId };

            _accountRepository = new Mock<IAccountRepository>();

            _handler = new LevyPayments.Application.Accounts.MarkAccountAsProcessedCommand.MarkAccountAsProcessedCommandHandler(_accountRepository.Object);
        }

        [Test]
        public void ThenItShouldMarkAccountAsProcessedInRepository()
        {
            // Act
            _handler.Handle(_request);

            // Assert
            _accountRepository.Verify(r => r.MarkAccountAsProcessed(AccountId), Times.Once);
        }
    }
}
