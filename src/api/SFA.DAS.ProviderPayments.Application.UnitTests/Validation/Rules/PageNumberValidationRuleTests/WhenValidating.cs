using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Application.Validation.Failures;
using SFA.DAS.ProviderPayments.Application.Validation.Rules;

namespace SFA.DAS.ProviderPayments.Application.UnitTests.Validation.Rules.PageNumberValidationRuleTests
{
    public class WhenValidating
    {
        private PageNumberValidationRule _rule;

        [SetUp]
        public void Arrange()
        {
            _rule = new PageNumberValidationRule();
        }

        [TestCase(1)]
        [TestCase(100)]
        [TestCase(int.MaxValue)]
        public async Task WithAValidPageNumberThenItShouldReturnAnEmptyEnumerable(int pageNumber)
        {
            // Act
            var actual = (await _rule.ValidateAsync(pageNumber))?.ToArray();

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Length);
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(int.MinValue)]
        public async Task WithAnInvalidPageNumberThenItShouldReturnAnInvalidPageNumberFailure(int pageNumber)
        {
            // Act
            var actual = (await _rule.ValidateAsync(pageNumber))?.ToArray();

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Length);
            Assert.IsInstanceOf<InvalidPageNumberFailure>(actual[0]);
        }
    }
}
