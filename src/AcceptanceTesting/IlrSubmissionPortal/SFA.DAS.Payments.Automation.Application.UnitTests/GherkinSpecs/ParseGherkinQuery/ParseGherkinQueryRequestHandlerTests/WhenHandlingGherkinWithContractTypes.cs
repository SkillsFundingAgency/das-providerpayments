using System;
using NUnit.Framework;
using SFA.DAS.Payments.Automation.Application.GherkinSpecs.ParseGherkinQuery;
using SFA.DAS.Payments.Automation.Domain.Specifications;

namespace SFA.DAS.Payments.Automation.Application.UnitTests.GherkinSpecs.ParseGherkinQuery.ParseGherkinQueryRequestHandlerTests
{
    public class WhenHandlingGherkinWithContractTypes
    {
        private ParseGherkinQueryRequest _request;
        private ParseGherkinQueryRequestHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _request = new ParseGherkinQueryRequest
            {
                GherkinSpecs = Properties.Resources.WhenHandlingGherkinWithContractTypes
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

            // Assert
            Assert.IsNotNull(response.Results);
            Assert.AreEqual(1, response.Results.Length);
        }

        [Test]
        public void ThenItShouldCorrectlyParseContractTypes()
        {
            // Act
            var response = _handler.Handle(_request);

            // Assert
            var spec1 = response.Results[0];
            Assert.IsNotNull(spec1);
            Assert.AreEqual("contract types", spec1.Name);
            Assert.AreEqual(2, spec1.Arrangement.ContractTypes.Count);

            var spec1ContractType1 = spec1.Arrangement.ContractTypes[0];
            Assert.IsNotNull(spec1ContractType1);
            Assert.AreEqual(ContractType.ContractWithEmployer, spec1ContractType1.ContractType);
            Assert.AreEqual(new DateTime(2017, 8, 3), spec1ContractType1.DateFrom);
            Assert.AreEqual(new DateTime(2017, 11, 2), spec1ContractType1.DateTo);

            var spec1ContractType2 = spec1.Arrangement.ContractTypes[1];
            Assert.IsNotNull(spec1ContractType2);
            Assert.AreEqual(ContractType.ContractWithSfa, spec1ContractType2.ContractType);
            Assert.AreEqual(new DateTime(2017, 11, 3), spec1ContractType2.DateFrom);
            Assert.AreEqual(new DateTime(2018, 8, 4), spec1ContractType2.DateTo);
        }
    }
}
