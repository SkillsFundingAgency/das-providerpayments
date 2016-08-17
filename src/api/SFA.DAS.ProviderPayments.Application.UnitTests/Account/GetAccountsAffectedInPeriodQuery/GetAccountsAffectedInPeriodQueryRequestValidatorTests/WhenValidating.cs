using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Application.Account.GetAccountsAffectedInPeriodQuery;
using SFA.DAS.ProviderPayments.Application.Validation.Failures;
using SFA.DAS.ProviderPayments.Domain.Data;

namespace SFA.DAS.ProviderPayments.Application.UnitTests.Account.GetAccountsAffectedInPeriodQuery.GetAccountsAffectedInPeriodQueryRequestValidatorTests
{
    public class WhenValidating
    {
        private Mock<IPeriodEndRepository> _periodEndRepository;
        private GetAccountsAffectedInPeriodQueryRequestValidator _validator;

        [SetUp]
        public void Arrange()
        {
            _periodEndRepository = new Mock<IPeriodEndRepository>();
            _periodEndRepository.Setup(r => r.GetPeriodEndAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new Domain.Data.Entities.PeriodEndEntity()));

            _validator = new GetAccountsAffectedInPeriodQueryRequestValidator(_periodEndRepository.Object);
        }

        [Test]
        public async Task WithValidRequestThenItShouldReturnValidResponse()
        {
            // Act
            var actual = await _validator.ValidateAsync(new GetAccountsAffectedInPeriodQueryRequest
            {
                PeriodCode = "201704",
                PageNumber = 1
            });

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsValid());
        }

        [TestCase("2017")]
        [TestCase("04")]
        [TestCase("1704")]
        [TestCase("20174")]
        [TestCase("abcdef")]
        [TestCase("201504")]
        [TestCase("201600")]
        [TestCase("201613")]
        public async Task WithAnInvalidPeriodCodeThenItShouldReturnAnInvalidResponse(string periodCode)
        {
            // Act
            var actual = await _validator.ValidateAsync(new GetAccountsAffectedInPeriodQueryRequest
            {
                PeriodCode = periodCode,
                PageNumber = 1
            });

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.IsValid());
            Assert.IsTrue(actual.Failures.Any(f => f.Code == InvalidPeriodCodeFailure.FailureCode && f.Description == InvalidPeriodCodeFailure.FailureDescription));
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(int.MinValue)]
        public async Task WithAnInvalidPageNumberThenItShouldReturnAnInvalidResponse(int pageNumber)
        {
            // Act
            var actual = await _validator.ValidateAsync(new GetAccountsAffectedInPeriodQueryRequest
            {
                PeriodCode = "201704",
                PageNumber = pageNumber
            });

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.IsValid());
            Assert.IsTrue(actual.Failures.Any(f => f.Code == InvalidPageNumberFailure.FailureCode && f.Description == InvalidPageNumberFailure.FailureDescription));
        }

        [Test]
        public async Task WithAPeriodCodeThatDoesNotExistThenItShouldReturnAnInvalidResponse()
        {
            // Arrange
            _periodEndRepository.Setup(r => r.GetPeriodEndAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<Domain.Data.Entities.PeriodEndEntity>(null));

            // Act
            var actual = await _validator.ValidateAsync(new GetAccountsAffectedInPeriodQueryRequest
            {
                PeriodCode = "201809",
                PageNumber = 1
            });

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.IsValid());
            Assert.IsTrue(actual.Failures.Any(f => f.Code == PeriodNotFoundFailure.FailureCode && f.Description == PeriodNotFoundFailure.FailureDescription));
        }
    }
}
