using System.Linq;
using NUnit.Framework;
using SFA.DAS.Payments.Automation.IlrBuilder.Validation;

namespace SFA.DAS.Payments.Automation.IlrBuilder.UnitTests.ValidationTests
{
    public class WhenValidatingAnIlrWithHeaderIssues
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
        public void ThenItShouldReturnInvalidResultIfUkprnIsMissing()
        {
            // Arrange
            _ilr.Ukprn = 0;

            // Act
            var actual = _validator.Validate(_ilr);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.IsValid);
            Assert.IsNotNull(actual.Errors);
            Assert.IsTrue(actual.Errors.Any(x => x.Code == ErrorCodes.UkprnMissingCode));
        }

        [Test]
        public void ThenItShouldReturnInvalidResultIfPreparationDateIsNotInAcademicYear()
        {
            // Arrange
            _ilr.PreparationDate = new System.DateTime(2015, 6, 1);

            // Act
            var actual = _validator.Validate(_ilr);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.IsValid);
            Assert.IsNotNull(actual.Errors);
            Assert.IsTrue(actual.Errors.Any(x => x.Code == ErrorCodes.PrepDateNotInYearCode));
        }

        [Test]
        public void ThenItShouldReturnInvalidResultIfPreparationDateIsBeforeAnyLearningStartDates()
        {
            // Arrange
            _ilr.PreparationDate = new System.DateTime(2015, 6, 1);

            // Act
            var actual = _validator.Validate(_ilr);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.IsValid);
            Assert.IsNotNull(actual.Errors);
            Assert.IsTrue(actual.Errors.Any(x => x.Code == ErrorCodes.PrepDateBeforeLearnStart));
        }
    }
}
