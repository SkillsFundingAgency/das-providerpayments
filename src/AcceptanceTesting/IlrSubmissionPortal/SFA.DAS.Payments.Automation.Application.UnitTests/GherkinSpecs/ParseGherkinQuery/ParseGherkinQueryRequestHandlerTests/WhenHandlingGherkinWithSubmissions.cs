using System;
using NUnit.Framework;
using SFA.DAS.Payments.Automation.Application.GherkinSpecs.ParseGherkinQuery;
using SFA.DAS.Payments.Automation.Domain.Specifications;

namespace SFA.DAS.Payments.Automation.Application.UnitTests.GherkinSpecs.ParseGherkinQuery.ParseGherkinQueryRequestHandlerTests
{
    public class WhenHandlingGherkinWithSubmissions
    {
        private ParseGherkinQueryRequest _request;
        private ParseGherkinQueryRequestHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _request = new ParseGherkinQueryRequest
            {
                GherkinSpecs = Properties.Resources.WhenHandlingGherkinWithSubmissions
            };

            _handler = new ParseGherkinQueryRequestHandler();
        }

        [Test]
        public void ThenItShouldReturnSuccessfulResponse()
        {
            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsSuccess);
        }

        [Test]
        public void ThenItShouldReturnEachScenario()
        {
            // Act
            var response = _handler.Handle(_request);
            if (!response.IsSuccess)
            {
                throw response.Error;
            }

            // Assert
            Assert.IsNotNull(response.Results);
            Assert.AreEqual(2, response.Results.Length);
        }

        [Test]
        public void ThenItShouldParseIlrWithoutResidual()
        {
            // Act
            var response = _handler.Handle(_request);
            if (!response.IsSuccess)
            {
                throw response.Error;
            }

            // Assert
            var spec1 = response.Results[0];
            Assert.IsNotNull(spec1);
            Assert.IsNotNull(spec1.Arrangement.LearnerRecords);
            Assert.AreEqual(1, spec1.Arrangement.LearnerRecords.Count);

            var spec1Learner1 = spec1.Arrangement.LearnerRecords[0];
            Assert.IsNotNull(spec1Learner1);
            Assert.AreEqual("learner a", spec1Learner1.LearnerKey);
            Assert.AreEqual(new DateTime(2017, 8, 4), spec1Learner1.StartDate);
            Assert.AreEqual(new DateTime(2018, 8, 4), spec1Learner1.PlannedEndDate);
            Assert.IsNull(spec1Learner1.ActualEndDate);
            Assert.AreEqual(CompletionStatus.Continuing, spec1Learner1.CompletionStatus);
            Assert.AreEqual(12000, spec1Learner1.TotalTrainingPrice1);
            Assert.AreEqual(new DateTime(2017, 8, 4), spec1Learner1.TotalTrainingPrice1EffectiveDate);
            Assert.AreEqual(3000, spec1Learner1.TotalAssessmentPrice1);
            Assert.AreEqual(new DateTime(2017, 8, 4), spec1Learner1.TotalAssessmentPrice1EffectiveDate);
            Assert.AreEqual(Defaults.StandardCode, spec1Learner1.StandardCode);
        }

        [Test]
        public void ThenItShouldParseIlrWithResidual()
        {
            // Act
            var response = _handler.Handle(_request);

            // Assert
            var spec1 = response.Results[1];
            Assert.IsNotNull(spec1);
            Assert.IsNotNull(spec1.Arrangement.LearnerRecords);
            Assert.AreEqual(1, spec1.Arrangement.LearnerRecords.Count);

            var spec1Learner1 = spec1.Arrangement.LearnerRecords[0];
            Assert.IsNotNull(spec1Learner1);
            Assert.AreEqual("learner a", spec1Learner1.LearnerKey);
            Assert.AreEqual(new DateTime(2017, 8, 4), spec1Learner1.StartDate);
            Assert.AreEqual(new DateTime(2018, 8, 4), spec1Learner1.PlannedEndDate);
            Assert.IsNull(spec1Learner1.ActualEndDate);
            Assert.AreEqual(CompletionStatus.Continuing, spec1Learner1.CompletionStatus);
            Assert.AreEqual(12000, spec1Learner1.TotalTrainingPrice1);
            Assert.AreEqual(new DateTime(2017, 8, 4), spec1Learner1.TotalTrainingPrice1EffectiveDate);
            Assert.AreEqual(3000, spec1Learner1.TotalAssessmentPrice1);
            Assert.AreEqual(new DateTime(2017, 8, 4), spec1Learner1.TotalAssessmentPrice1EffectiveDate);
            Assert.AreEqual(5000, spec1Learner1.ResidualTrainingPrice1);
            Assert.AreEqual(new DateTime(2017, 10, 25), spec1Learner1.ResidualTrainingPrice1EffectiveDate);
            Assert.AreEqual(625, spec1Learner1.ResidualAssessmentPrice1);
            Assert.AreEqual(new DateTime(2017, 10, 25), spec1Learner1.ResidualAssessmentPrice1EffectiveDate);
            Assert.AreEqual(Defaults.StandardCode, spec1Learner1.StandardCode);
        }
    }
}
