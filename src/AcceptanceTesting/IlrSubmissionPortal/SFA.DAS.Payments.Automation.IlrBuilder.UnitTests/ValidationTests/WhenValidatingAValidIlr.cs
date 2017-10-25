using NUnit.Framework;
using SFA.DAS.Payments.Automation.IlrBuilder.Validation;

namespace SFA.DAS.Payments.Automation.IlrBuilder.UnitTests.ValidationTests
{
    public class WhenValidatingAValidIlr
    {
        private IndividualLearningRecord _ilr;
        private IndividualLearningRecordValidator _validator;

        [SetUp]
        public void Arrange()
        {
            _ilr = CommonIndividualLearningRecords.SingleDasLearnerOnAStandard();

            _validator = new IndividualLearningRecordValidator();
        }

        [Test]
        public void ThenItShouldReturnAValidResult()
        {
            // Act
            var actual = _validator.Validate(_ilr);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsValid);
        }

        [Test]
        public void ThenItShouldHaveAnEmptyErrorArray()
        {
            // Act
            var actual = _validator.Validate(_ilr);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Errors);
            Assert.IsEmpty(actual.Errors);
        }
    }
}
