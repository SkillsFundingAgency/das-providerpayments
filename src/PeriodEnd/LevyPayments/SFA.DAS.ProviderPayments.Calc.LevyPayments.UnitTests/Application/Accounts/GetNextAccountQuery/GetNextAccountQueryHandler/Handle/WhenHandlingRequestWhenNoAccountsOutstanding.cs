using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Accounts.GetNextAccountQuery;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.UnitTests.Application.Accounts.GetNextAccountQuery.GetNextAccountQueryHandler.Handle
{
    public class WhenHandlingRequestWhenNoAccountsOutstanding
    {
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
                .Returns<AccountEntity>(null);

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
        public void ThenTheResponseShouldNotHaveAnAccount()
        {
            // Act
            var actual = _handler.Handle(_request);

            // Assert
            Assert.IsNull(actual.Account);
        }
    }
}
