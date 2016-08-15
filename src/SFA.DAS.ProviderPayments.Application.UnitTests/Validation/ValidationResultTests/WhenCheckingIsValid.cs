using NUnit.Framework;
using SFA.DAS.ProdiverPayments.Application.Validation;

namespace SFA.DAS.ProviderPayments.Application.UnitTests.Validation.ValidationResultTests
{
    public class WhenCheckingIsValid
    {
        private ValidationResult _result;

        [SetUp]
        public void Arrange()
        {
            _result = new ValidationResult();
        }

        [Test]
        public void WithNoValidationErrorsThenItShouldReturnTrue()
        {
            // Act
            var actual = _result.IsValid();

            // Assert
            Assert.IsTrue(actual);
        }

        [Test]
        public void WithValidationFailuresThenItShouldReturnFalse()
        {
            // Arrange
            _result.Failures = new[]
            {
                new ValidationFailure()
            };

            // Act
            var actual = _result.IsValid();

            // Assert
            Assert.IsFalse(actual);
        }

        [Test]
        public void WithValidationFailuresSetToNullThenItShouldReturnTrue()
        {
            // Arrange
            _result.Failures = null;

            // Act
            var actual = _result.IsValid();

            // Assert
            Assert.IsTrue(actual);
        }
    }
}
