using System.Linq;
using NUnit.Framework;
using SFA.DAS.Payments.Automation.Application.GherkinSpecs;
using SFA.DAS.Payments.Automation.Application.GherkinSpecs.ValidateSpecificationsQuery;

namespace SFA.DAS.Payments.Automation.Application.UnitTests.GherkinSpecs.ValidateSpecificationsQuery.ValidateSpecificationsQueryRequestHandlerTests
{
    public class WhenHandlingASpecificationWithAnInvalidCourseDuration
    {
        private ValidateSpecificationsQueryRequestHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _handler = new ValidateSpecificationsQueryRequestHandler();
        }

        [Test]
        public void ThenItShouldReturnInvalidResponseIfStandardHasDurationLessThan372Days()
        {
            // Arrange
            var spec = TestData.GetValidSpecification(true, false);
            foreach (var learner in spec.Arrangement.LearnerRecords)
            {
                learner.PlannedEndDate = learner.StartDate.AddDays(371);
            }

            // Act
            var actual = _handler.Handle(new ValidateSpecificationsQueryRequest { Specifications = new[] { spec } });

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.IsValid);
            Assert.IsTrue(actual.Errors.Any(e => e.RuleId == ValidationRuleIds.StandardMinDuration));
        }

        [Test]
        public void ThenItShouldReturnInvalidResponseIfFrameworkHasDurationLessThan365Days()
        {
            // Arrange
            var spec = TestData.GetValidSpecification(false, false);
            foreach (var learner in spec.Arrangement.LearnerRecords)
            {
                learner.PlannedEndDate = learner.StartDate.AddDays(364);
            }

            // Act
            var actual = _handler.Handle(new ValidateSpecificationsQueryRequest { Specifications = new[] { spec } });

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.IsValid);
            Assert.IsTrue(actual.Errors.Any(e => e.RuleId == ValidationRuleIds.FrameworkMinDuration));
        }
    }
}
