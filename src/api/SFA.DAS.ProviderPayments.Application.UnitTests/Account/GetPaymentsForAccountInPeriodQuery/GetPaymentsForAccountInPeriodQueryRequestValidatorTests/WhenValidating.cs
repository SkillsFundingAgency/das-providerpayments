using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Application.Account.Failures;
using SFA.DAS.ProviderPayments.Application.Account.GetPaymentsForAccountInPeriodQuery;
using SFA.DAS.ProviderPayments.Application.Account.Rules;
using SFA.DAS.ProviderPayments.Application.Validation;
using SFA.DAS.ProviderPayments.Application.Validation.Failures;
using SFA.DAS.ProviderPayments.Application.Validation.Rules;

namespace SFA.DAS.ProviderPayments.Application.UnitTests.Account.GetPaymentsForAccountInPeriodQuery.GetPaymentsForAccountInPeriodQueryRequestValidatorTests
{
    public class WhenValidating
    {
        private const string PeriodCode = "201704";
        private const string AccountId = "DasAccountA";
        private Mock<PageNumberValidationRule> _pageNumberValidationRule;
        private Mock<PeriodCodeValidationRule> _periodCodeValidationRule;
        private Mock<AccountIdValidationRule> _accountIdValidationRule;
        private GetPaymentsForAccountInPeriodQueryRequestValidator _validator;
        private GetPaymentsForAccountInPeriodQueryRequest _request;

        [SetUp]
        public void Arrange()
        {
            _pageNumberValidationRule = new Mock<PageNumberValidationRule>();
            _pageNumberValidationRule.Setup(r => r.ValidateAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<IEnumerable<ValidationFailure>>(new ValidationFailure[0]));

            _periodCodeValidationRule = new Mock<PeriodCodeValidationRule>();
            _periodCodeValidationRule.Setup(r => r.ValidateAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<IEnumerable<ValidationFailure>>(new ValidationFailure[0]));

            _accountIdValidationRule = new Mock<AccountIdValidationRule>();
            _accountIdValidationRule.Setup(r => r.ValidateAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<IEnumerable<ValidationFailure>>(new ValidationFailure[0]));

            _validator = new GetPaymentsForAccountInPeriodQueryRequestValidator(_pageNumberValidationRule.Object,
                _periodCodeValidationRule.Object, _accountIdValidationRule.Object);

            _request = new GetPaymentsForAccountInPeriodQueryRequest
            {
                PageNumber = 1,
                PeriodCode = PeriodCode,
                AccountId = AccountId
            };
        }

        [Test]
        public async Task WithAValidRequestThenItShouldReturnAValidResult()
        {
            // Act
            var actual = await _validator.ValidateAsync(_request);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsValid());
        }

        [Test]
        public async Task WithAnInvalidPageNumberThenItShouldReturnInvalidResult()
        {
            // Arrange
            _pageNumberValidationRule.Setup(r => r.ValidateAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<IEnumerable<ValidationFailure>>(new[]
                {
                    new PageNotFoundFailure()
                }));

            // Act
            var actual = await _validator.ValidateAsync(_request);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.IsValid());
            Assert.AreEqual(1, actual.Failures.Count());
            Assert.IsTrue(actual.Failures.Any(f => f.Code == PageNotFoundFailure.FailureCode && f.Description == PageNotFoundFailure.FailureDescription));
        }

        [Test]
        public async Task WithAnInvalidPeriodCodeThenItShouldReturnInvalidResult()
        {
            // Arrange
            _periodCodeValidationRule.Setup(r => r.ValidateAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<IEnumerable<ValidationFailure>>(new[]
                {
                    new PeriodNotFoundFailure()
                }));

            // Act
            var actual = await _validator.ValidateAsync(_request);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.IsValid());
            Assert.AreEqual(1, actual.Failures.Count());
            Assert.IsTrue(actual.Failures.Any(f => f.Code == PeriodNotFoundFailure.FailureCode && f.Description == PeriodNotFoundFailure.FailureDescription));
        }

        [Test]
        public async Task WithAnInvalidAccountIdThenItShouldReturnInvalidResult()
        {
            // Arrange
            _accountIdValidationRule.Setup(r => r.ValidateAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<IEnumerable<ValidationFailure>>(new[]
                {
                    new AccountNotFoundFailure()
                }));

            // Act
            var actual = await _validator.ValidateAsync(_request);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.IsValid());
            Assert.AreEqual(1, actual.Failures.Count());
            Assert.IsTrue(actual.Failures.Any(f => f.Code == AccountNotFoundFailure.FailureCode && f.Description == AccountNotFoundFailure.FailureDescription));
        }
    }
}
