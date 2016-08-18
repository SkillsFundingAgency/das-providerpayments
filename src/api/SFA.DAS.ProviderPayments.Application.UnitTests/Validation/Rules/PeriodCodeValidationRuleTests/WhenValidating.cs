using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Application.Validation.Failures;
using SFA.DAS.ProviderPayments.Application.Validation.Rules;
using SFA.DAS.ProviderPayments.Domain.Data;

namespace SFA.DAS.ProviderPayments.Application.UnitTests.Validation.Rules.PeriodCodeValidationRuleTests
{
    public class WhenValidating
    {
        private const string ValidPeriodCode = "201704";

        private Mock<IPeriodEndRepository> _periodEndRepository;
        private PeriodCodeValidationRule _rule;

        [SetUp]
        public void Arrange()
        {
            _periodEndRepository = new Mock<IPeriodEndRepository>();
            _periodEndRepository.Setup(r => r.GetPeriodEndAsync(ValidPeriodCode))
                .Returns(Task.FromResult(new Domain.Data.Entities.PeriodEndEntity()));

            _rule = new PeriodCodeValidationRule(_periodEndRepository.Object);
        }

        [Test]
        public async Task WithAPeriodCodeTheIsValidAndFoundThenItShouldReturnEmptyEnumerable()
        {
            // Act
            var actual = (await _rule.ValidateAsync(ValidPeriodCode))?.ToArray();

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Length);
        }

        [TestCase("2017")]
        [TestCase("20174")]
        [TestCase("1704")]
        [TestCase("sfgfdgdf")]
        [TestCase("190004")]
        [TestCase("201799")]
        [TestCase("201700")]
        public async Task WithAPeriodCodeInTheWrongFormatThenItShouldReturnAValidationFailure(string periodCode)
        {
            // Act
            var actual = (await _rule.ValidateAsync(periodCode))?.ToArray();

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Length);
            Assert.IsInstanceOf<InvalidPeriodCodeFailure>(actual.First());
        }

        [Test]
        public async Task WithAPeriodCodeThatIsValidButNotFoundThenItShouldReturnAValidationFailure()
        {
            // Act
            var actual = (await _rule.ValidateAsync("201705"))?.ToArray();

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Length);
            Assert.IsInstanceOf<PeriodNotFoundFailure>(actual.First());
        }
    }
}
