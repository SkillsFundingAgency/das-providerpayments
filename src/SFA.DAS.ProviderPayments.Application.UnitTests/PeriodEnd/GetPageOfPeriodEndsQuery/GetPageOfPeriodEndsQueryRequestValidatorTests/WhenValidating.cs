using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Application.PeriodEnd.GetPageOfPeriodEndsQuery;
using SFA.DAS.ProviderPayments.Application.Validation.Failures;

namespace SFA.DAS.ProviderPayments.Application.UnitTests.PeriodEnd.GetPageOfPeriodEndsQuery.GetPageOfPeriodEndsQueryRequestValidatorTests
{
    public class WhenValidating
    {
        private GetPageOfPeriodEndsQueryRequestValidator _validator;

        [SetUp]
        public void Arrange()
        {
            _validator = new GetPageOfPeriodEndsQueryRequestValidator();
        }

        [Test]
        public async Task ThenItShouldReturnAValidationResult()
        {
            // Act
            var actual = await _validator.ValidateAsync(new GetPageOfPeriodEndsQueryRequest { PageNumber = 1 });

            // Assert
            Assert.IsNotNull(actual);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(10)]
        [TestCase(9999999)]
        public async Task WithAPageNumberGreaterThanZeroThenItShouldReturnAValidResult(int pageNumber)
        {
            // Act
            var actual = await _validator.ValidateAsync(new GetPageOfPeriodEndsQueryRequest { PageNumber = pageNumber });

            // Assert
            Assert.IsTrue(actual.IsValid());
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-999999)]
        public async Task WithAPageNumberOfZeroOrLessThenItShouldReturnAnInvalidResult(int pageNumber)
        {
            // Act
            var actual = await _validator.ValidateAsync(new GetPageOfPeriodEndsQueryRequest { PageNumber = pageNumber });

            // Assert
            Assert.IsFalse(actual.IsValid());
            Assert.IsTrue(actual.Failures.Any(f => f.Code == InvalidPageNumberFailure.FailureCode && f.Description == InvalidPageNumberFailure.FailureDescription));
        }
    }
}
