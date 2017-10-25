using System;
using NUnit.Framework;
using SFA.DAS.Payments.Automation.Application.GherkinSpecs.ParseGherkinQuery;
using SFA.DAS.Payments.Automation.Domain.Specifications;

namespace SFA.DAS.Payments.Automation.Application.UnitTests.GherkinSpecs.ParseGherkinQuery.ParseGherkinQueryRequestHandlerTests
{
    public class WhenHandlingGherkinWithEmploymentStatuses
    {
        private ParseGherkinQueryRequest _request;
        private ParseGherkinQueryRequestHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _request = new ParseGherkinQueryRequest
            {
                GherkinSpecs = Properties.Resources.WhenHandlingGherkinWithEmploymentStatuses
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
            Assert.IsTrue(response.IsSuccess, response.Error?.Message);
        }

        [Test]
        public void ThenItShouldReturnEachScenario()
        {
            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response.Results);
            Assert.AreEqual(3, response.Results.Length);
        }

        [Test]
        public void ThenItShouldCorrectlyParseEmployentStatusWithSmallEmployer()
        {
            // Act
            var response = _handler.Handle(_request);

            // Assert
            var spec1 = response.Results[0];
            Assert.IsNotNull(spec1);
            Assert.AreEqual("With small employer", spec1.Name);
            Assert.AreEqual(1, spec1.Arrangement.EmploymentStatuses.Count);

            var spec1Status1 = spec1.Arrangement.EmploymentStatuses[0];
            Assert.IsNotNull(spec1Status1);
            Assert.AreEqual("employer 1", spec1Status1.EmployerKey);
            Assert.AreEqual(EmploymentStatus.InPaidEmployment, spec1Status1.EmploymentStatus);
            Assert.AreEqual(new DateTime(2017, 8, 5), spec1Status1.EmploymentStatusApplies);
            Assert.AreEqual(EmploymentStatusMonitoringType.SEM, spec1Status1.MonitoringType);
            Assert.AreEqual(1, spec1Status1.MonitoringCode);



            var spec2 = response.Results[1];
            Assert.IsNotNull(spec2);
            Assert.AreEqual("Without small employer", spec2.Name);
            Assert.AreEqual(1, spec2.Arrangement.EmploymentStatuses.Count);

            var spec2Status1 = spec2.Arrangement.EmploymentStatuses[0];
            Assert.IsNotNull(spec2Status1);
            Assert.AreEqual("employer 1", spec2Status1.EmployerKey);
            Assert.AreEqual(EmploymentStatus.InPaidEmployment, spec2Status1.EmploymentStatus);
            Assert.AreEqual(new DateTime(2017, 8, 5), spec2Status1.EmploymentStatusApplies);



            var spec3 = response.Results[2];
            Assert.IsNotNull(spec3);
            Assert.AreEqual("Not in paid employment", spec3.Name);
            Assert.AreEqual(2, spec3.Arrangement.EmploymentStatuses.Count);

            var spec3Status1 = spec3.Arrangement.EmploymentStatuses[0];
            Assert.IsNotNull(spec3Status1);
            Assert.AreEqual("employer 1", spec3Status1.EmployerKey);
            Assert.AreEqual(EmploymentStatus.InPaidEmployment, spec3Status1.EmploymentStatus);
            Assert.AreEqual(new DateTime(2017, 8, 5), spec3Status1.EmploymentStatusApplies);

            var spec3Status2 = spec3.Arrangement.EmploymentStatuses[1];
            Assert.IsNotNull(spec3Status2);
            Assert.IsEmpty(spec3Status2.EmployerKey);
            Assert.AreEqual(EmploymentStatus.NotInPaidEmployment, spec3Status2.EmploymentStatus);
            Assert.AreEqual(new DateTime(2017, 12, 23), spec3Status2.EmploymentStatusApplies);
        }
    }
}
