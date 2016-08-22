using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Application.Account.Failures;
using SFA.DAS.ProviderPayments.Application.Account.Rules;
using SFA.DAS.ProviderPayments.Domain.Data;
using SFA.DAS.ProviderPayments.Domain.Data.Entities;

namespace SFA.DAS.ProviderPayments.Application.UnitTests.Account.Rules.AccountIdValidationRuleTests
{
    public class WhenValidating
    {
        private const string AccountId = "TheAccount";

        private Mock<IAccountRepository> _accountRepository;
        private AccountIdValidationRule _rule;

        [SetUp]
        public void Arrange()
        {
            _accountRepository = new Mock<IAccountRepository>();
            _accountRepository.Setup(r => r.GetAccountAsync(AccountId))
                .Returns(Task.FromResult(new AccountEntity()));

            _rule = new AccountIdValidationRule(_accountRepository.Object);
        }

        [Test]
        public async Task WithAValidAndExistingAccountIdThenItShouldReturnAnEmptyEnumerable()
        {
            // Act
            var actual = await _rule.ValidateAsync(AccountId);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsEmpty(actual);
        }

        [Test]
        public async Task WithAnAccountIdThatDoesNotExistThenItShouldReturnAnAccountNotFoundFailure()
        {
            // Arrange
            _accountRepository.Setup(r => r.GetAccountAsync(AccountId))
                .Returns(Task.FromResult<AccountEntity>(null));

            // Act
            var actual = (await _rule.ValidateAsync(AccountId))?.ToArray();

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Length);
            Assert.IsInstanceOf<AccountNotFoundFailure>(actual[0]);
        }

    }
}
