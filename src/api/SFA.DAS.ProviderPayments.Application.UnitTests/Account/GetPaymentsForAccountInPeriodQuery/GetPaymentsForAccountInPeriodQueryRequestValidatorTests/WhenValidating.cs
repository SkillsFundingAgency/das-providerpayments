using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Application.Account.GetPaymentsForAccountInPeriodQuery;
using SFA.DAS.ProviderPayments.Application.Validation;
using SFA.DAS.ProviderPayments.Application.Validation.Rules;

namespace SFA.DAS.ProviderPayments.Application.UnitTests.Account.GetPaymentsForAccountInPeriodQuery.GetPaymentsForAccountInPeriodQueryRequestValidatorTests
{
    public class WhenValidating
    {
        private const string PeriodCode = "201704";
        private const string AccountId = "DasAccountA";
        private Mock<PageNumberValidationRule> _pageNumberValidationRule;
        private Mock<PeriodCodeValidationRule> _periodCodeValidationRule;
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

            _validator = new GetPaymentsForAccountInPeriodQueryRequestValidator(_pageNumberValidationRule.Object,
                _periodCodeValidationRule.Object);

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
        public async Task WithAnInvalidPageNumber()
        {
            
        }
    }
}
