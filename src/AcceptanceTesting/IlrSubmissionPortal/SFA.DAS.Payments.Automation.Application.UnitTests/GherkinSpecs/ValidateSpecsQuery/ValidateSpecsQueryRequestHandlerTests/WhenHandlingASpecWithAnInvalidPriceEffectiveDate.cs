using System;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.Payments.Automation.Application.GherkinSpecs;
using SFA.DAS.Payments.Automation.Application.GherkinSpecs.ValidateSpecificationsQuery;

namespace SFA.DAS.Payments.Automation.Application.UnitTests.GherkinSpecs.ValidateSpecificationsQuery.ValidateSpecificationsQueryRequestHandlerTests
{
    public class WhenHandlingASpecificationWithAnInvalidPriceEffectiveDate
    {
        private static readonly DateTime LearningStartDate = new DateTime(2017, 5, 18);

        private ValidateSpecificationsQueryRequestHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _handler = new ValidateSpecificationsQueryRequestHandler();
        }

        [Test]
        public void ThenItShouldReturnInvalidResponseIfTrainingPrice1EffectiveDateIsAfterStartOfLearning()
        {
            // Arrange
            var spec = TestData.GetValidSpecification(false, false);
            foreach (var learner in spec.Arrangement.LearnerRecords)
            {
                learner.StartDate = LearningStartDate;
                learner.TotalTrainingPrice1EffectiveDate = LearningStartDate.AddDays(1);
            }

            // Act
            var actual = _handler.Handle(new ValidateSpecificationsQueryRequest { Specifications = new[] { spec } });

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.IsValid);
            Assert.IsTrue(actual.Errors.Any(e => e.RuleId == ValidationRuleIds.PriceEffectiveTooLate));
        }

        [Test]
        public void ThenItShouldReturnInvalidResponseIfAssessmentPrice1EffectiveDateIsAfterStartOfLearning()
        {
            // Arrange
            var spec = TestData.GetValidSpecification(true, false);
            foreach (var learner in spec.Arrangement.LearnerRecords)
            {
                learner.StartDate = LearningStartDate;
                learner.TotalTrainingPrice1EffectiveDate = LearningStartDate;
                learner.TotalAssessmentPrice1EffectiveDate = LearningStartDate.AddDays(1);
            }

            // Act
            var actual = _handler.Handle(new ValidateSpecificationsQueryRequest { Specifications = new[] { spec } });

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.IsValid);
            Assert.IsTrue(actual.Errors.Any(e => e.RuleId == ValidationRuleIds.PriceEffectiveTooLate));
        }

        [Test]
        public void ThenItShouldReturnInvalidResponseIfTrainingAndAssessmentPrice1EffectiveDateIsAfterStartOfLearning()
        {
            // Arrange
            var spec = TestData.GetValidSpecification(false, false);
            foreach (var learner in spec.Arrangement.LearnerRecords)
            {
                learner.StartDate = LearningStartDate;
                learner.TotalTrainingPrice1EffectiveDate = LearningStartDate.AddDays(1);
                learner.TotalAssessmentPrice1EffectiveDate = LearningStartDate.AddDays(1);
            }

            // Act
            var actual = _handler.Handle(new ValidateSpecificationsQueryRequest { Specifications = new[] { spec } });

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.IsValid);
            Assert.AreEqual(2, actual.Errors.Count(e => e.RuleId == ValidationRuleIds.PriceEffectiveTooLate));
        }






        [Test]
        public void ThenItShouldReturnInvalidResponseIfTrainingPrice1EffectiveDateIsBeforeStartOfLearning()
        {
            // Arrange
            var spec = TestData.GetValidSpecification(false, false);
            foreach (var learner in spec.Arrangement.LearnerRecords)
            {
                learner.StartDate = LearningStartDate;
                learner.TotalTrainingPrice1EffectiveDate = LearningStartDate.AddDays(-1);
            }

            // Act
            var actual = _handler.Handle(new ValidateSpecificationsQueryRequest { Specifications = new[] { spec } });

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.IsValid);
            Assert.IsTrue(actual.Errors.Any(e => e.RuleId == ValidationRuleIds.PriceEffectiveTooEarly));
        }

        [Test]
        public void ThenItShouldReturnInvalidResponseIfAssessmentPrice1EffectiveDateIsBeforeStartOfLearning()
        {
            // Arrange
            var spec = TestData.GetValidSpecification(true, false);
            foreach (var learner in spec.Arrangement.LearnerRecords)
            {
                learner.StartDate = LearningStartDate;
                learner.TotalTrainingPrice1EffectiveDate = LearningStartDate;
                learner.TotalAssessmentPrice1EffectiveDate = LearningStartDate.AddDays(-1);
            }

            // Act
            var actual = _handler.Handle(new ValidateSpecificationsQueryRequest { Specifications = new[] { spec } });

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.IsValid);
            Assert.IsTrue(actual.Errors.Any(e => e.RuleId == ValidationRuleIds.PriceEffectiveTooEarly));
        }

        [Test]
        public void ThenItShouldReturnInvalidResponseIfTrainingAndAssessmentPrice1EffectiveDateIsBeforeStartOfLearning()
        {
            // Arrange
            var spec = TestData.GetValidSpecification(false, false);
            foreach (var learner in spec.Arrangement.LearnerRecords)
            {
                learner.StartDate = LearningStartDate;
                learner.TotalTrainingPrice1EffectiveDate = LearningStartDate.AddDays(-1);
                learner.TotalAssessmentPrice1EffectiveDate = LearningStartDate.AddDays(-1);
            }

            // Act
            var actual = _handler.Handle(new ValidateSpecificationsQueryRequest { Specifications = new[] { spec } });

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.IsValid);
            Assert.AreEqual(2, actual.Errors.Count(e => e.RuleId == ValidationRuleIds.PriceEffectiveTooEarly));
        }

    }
}
