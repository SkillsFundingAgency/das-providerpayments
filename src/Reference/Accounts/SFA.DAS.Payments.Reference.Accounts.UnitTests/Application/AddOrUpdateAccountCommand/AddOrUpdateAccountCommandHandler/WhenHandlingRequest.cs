using Moq;
using NUnit.Framework;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.Payments.Reference.Accounts.Application.AddOrUpdateAccountCommand;
using SFA.DAS.Payments.Reference.Accounts.Infrastructure.Data;
using SFA.DAS.Payments.Reference.Accounts.Infrastructure.Data.Entities;

namespace SFA.DAS.Payments.Reference.Accounts.UnitTests.Application.AddOrUpdateAccountCommand.AddOrUpdateAccountCommandHandler
{
    public class WhenHandlingRequest
    {
        private AddOrUpdateAccountCommandRequest _request;
        private Mock<IAccountRepository> _accountRepository;
        private Accounts.Application.AddOrUpdateAccountCommand.AddOrUpdateAccountCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _request = new AddOrUpdateAccountCommandRequest
            {
                Account = new AccountWithBalanceViewModel
                {
                    AccountId = 1,
                    AccountHashId = "ACC01",
                    AccountName = "Account1",
                    Balance = 3738
                },
                CorrelationDate = new System.DateTime(2017, 4, 1)
            };

            _accountRepository = new Mock<IAccountRepository>();

            _handler = new Accounts.Application.AddOrUpdateAccountCommand.AddOrUpdateAccountCommandHandler(_accountRepository.Object);
        }

        [Test]
        public void ThenItShouldInsertAccountIfOneDoesNotAlreadyExist()
        {
            // Act
            _handler.Handle(_request);

            // Assert
            _accountRepository.Verify(r => r.Insert(It.Is<AccountEntity>(e => IsEntityForAccount(e, _request.Account))), Times.Once);
        }

        [Test]
        public void ThenItShouldUpdateAccountIfOneDoesExist()
        {
            // Arrange
            _accountRepository.Setup(r => r.GetById(_request.Account.AccountId))
                .Returns(new AccountEntity());
            // Act
            _handler.Handle(_request);

            // Assert
            _accountRepository.Verify(r => r.Update(It.Is<AccountEntity>(e => IsEntityForAccount(e, _request.Account))), Times.Once);
        }

        private bool IsEntityForAccount(AccountEntity entity, AccountWithBalanceViewModel account)
        {
            return entity.AccountId == account.AccountId
                   && entity.AccountHashId == account.AccountHashId
                   && entity.AccountName == account.AccountName
                   && entity.Balance == account.Balance
                   && entity.VersionId == "20170401"
                   && entity.IsLevyPayer == account.IsLevyPayer;
        }
    }
}
