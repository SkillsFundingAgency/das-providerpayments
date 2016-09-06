using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Accounts.GetNextAccountQuery;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Infrastructure.Data;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.UnitTests.Application.Accounts.GetNextAccountQuery.Handle
{
    public class WhenHandlingRequestWhenNoAccountsOutstanding
    {
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
                .Returns<AccountEntity>(null);

            _commitmentRepository = new Mock<ICommitmentRepository>();

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
        public void ThenTheResponseShouldNotHaveAnAccount()
        {
            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNull(actual.Account);
        }
    }
}
