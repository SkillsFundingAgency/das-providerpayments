using NUnit.Framework;
using SFA.DAS.Payments.Automation.Application.GherkinSpecs.ParseGherkinQuery;
using SFA.DAS.Payments.Automation.Domain.Specifications;

namespace SFA.DAS.Payments.Automation.Application.UnitTests.GherkinSpecs.ParseGherkinQuery.ParseGherkinQueryRequestHandlerTests
{
    public class WhenHandlingGherkinWithAccountBalances
    {
        private ParseGherkinQueryRequest _request;
        private ParseGherkinQueryRequestHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _request = new ParseGherkinQueryRequest
            {
                GherkinSpecs = Properties.Resources.WhenHandlingGherkinWithAccountBalances
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
            Assert.AreEqual(6, response.Results.Length);
        }

        [Test]
        public void ThenItShouldCorrectlyParseIndefinateLevySteps()
        {
            // Act
            var response = _handler.Handle(_request);

            // Assert
            var spec1 = response.Results[0];
            Assert.IsNotNull(spec1);
            Assert.AreEqual("Multiple employers with indefinate levy", spec1.Name);
            Assert.AreEqual(2, spec1.Arrangement.LevyBalances.Count);

            var spec1Account1 = spec1.Arrangement.LevyBalances[0];
            Assert.IsNotNull(spec1Account1);
            Assert.AreEqual("employer 1", spec1Account1.EmployerKey);
            Assert.AreEqual(long.MaxValue, spec1Account1.BalanceForAllPeriods);
            Assert.IsEmpty(spec1Account1.BalancesPerPeriod);

            var spec1Account2 = spec1.Arrangement.LevyBalances[1];
            Assert.IsNotNull(spec1Account2);
            Assert.AreEqual("employer 2", spec1Account2.EmployerKey);
            Assert.AreEqual(long.MaxValue, spec1Account2.BalanceForAllPeriods);
            Assert.IsEmpty(spec1Account2.BalancesPerPeriod);



            var spec2 = response.Results[1];
            Assert.IsNotNull(spec2);
            Assert.AreEqual("Single employer with indefinate levy", spec2.Name);
            Assert.AreEqual(1, spec2.Arrangement.LevyBalances.Count);

            var spec2Account1 = spec2.Arrangement.LevyBalances[0];
            Assert.IsNotNull(spec2Account1);
            Assert.AreEqual("employer 1", spec2Account1.EmployerKey);
            Assert.AreEqual(long.MaxValue, spec2Account1.BalanceForAllPeriods);
            Assert.IsEmpty(spec2Account1.BalancesPerPeriod);
        }

        [Test]
        public void ThenItShouldCorrectlyParseZeroLevySteps()
        {
            // Act
            var response = _handler.Handle(_request);

            // Assert
            var spec1 = response.Results[2];
            Assert.IsNotNull(spec1);
            Assert.AreEqual("Single employer with no levy", spec1.Name);
            Assert.AreEqual(1, spec1.Arrangement.LevyBalances.Count);

            var spec1Account1 = spec1.Arrangement.LevyBalances[0];
            Assert.IsNotNull(spec1Account1);
            Assert.AreEqual(Defaults.EmployerKey, spec1Account1.EmployerKey);
            Assert.AreEqual(0, spec1Account1.BalanceForAllPeriods);
            Assert.IsEmpty(spec1Account1.BalancesPerPeriod);
        }

        [Test]
        public void ThenItShouldCorrectlyParseSpecificLevySteps()
        {
            // Act
            var response = _handler.Handle(_request);

            // Assert
            var spec1 = response.Results[3];
            Assert.IsNotNull(spec1);
            Assert.AreEqual("Multiple employers with specific levy per period", spec1.Name);
            Assert.AreEqual(2, spec1.Arrangement.LevyBalances.Count);

            var spec1Account1 = spec1.Arrangement.LevyBalances[0];
            Assert.IsNotNull(spec1Account1);
            Assert.AreEqual("employer 1", spec1Account1.EmployerKey);
            Assert.IsNull(spec1Account1.BalanceForAllPeriods);
            Assert.AreEqual(4, spec1Account1.BalancesPerPeriod.Count);
            Assert.AreEqual(1000, spec1Account1.BalancesPerPeriod["05/17"]);
            Assert.AreEqual(2000, spec1Account1.BalancesPerPeriod["06/17"]);
            Assert.AreEqual(500, spec1Account1.BalancesPerPeriod["11/17"]);
            Assert.AreEqual(250, spec1Account1.BalancesPerPeriod["12/17"]);

            var spec1Account2 = spec1.Arrangement.LevyBalances[1];
            Assert.IsNotNull(spec1Account2);
            Assert.AreEqual("employer 2", spec1Account2.EmployerKey);
            Assert.IsNull(spec1Account2.BalanceForAllPeriods);
            Assert.AreEqual(4, spec1Account2.BalancesPerPeriod.Count);
            Assert.AreEqual(5000, spec1Account2.BalancesPerPeriod["01/18"]);
            Assert.AreEqual(5000, spec1Account2.BalancesPerPeriod["02/18"]);
            Assert.AreEqual(5000, spec1Account2.BalancesPerPeriod["06/18"]);
            Assert.AreEqual(5000, spec1Account2.BalancesPerPeriod["07/18"]);



            var spec2 = response.Results[4];
            Assert.IsNotNull(spec2);
            Assert.AreEqual("Single employer with specific levy per period", spec2.Name);
            Assert.AreEqual(1, spec2.Arrangement.LevyBalances.Count);

            var spec2Account1 = spec2.Arrangement.LevyBalances[0];
            Assert.IsNotNull(spec2Account1);
            Assert.AreEqual(Defaults.EmployerKey, spec2Account1.EmployerKey);
            Assert.IsNull(spec2Account1.BalanceForAllPeriods);
            Assert.AreEqual(4, spec2Account1.BalancesPerPeriod.Count);
            Assert.AreEqual(1000, spec2Account1.BalancesPerPeriod["05/17"]);
            Assert.AreEqual(2000, spec2Account1.BalancesPerPeriod["06/17"]);
            Assert.AreEqual(500, spec2Account1.BalancesPerPeriod["11/17"]);
            Assert.AreEqual(250, spec2Account1.BalancesPerPeriod["12/17"]);
        }

        [Test]
        public void ThenItShouldCorrectlyParseLevyStepsOfMixedFormat()
        {
            // Act
            var response = _handler.Handle(_request);

            // Assert
            var spec1 = response.Results[5];
            Assert.IsNotNull(spec1);
            Assert.AreEqual("Mixed levy types per employer", spec1.Name);
            Assert.AreEqual(2, spec1.Arrangement.LevyBalances.Count);

            var spec1Account1 = spec1.Arrangement.LevyBalances[0];
            Assert.IsNotNull(spec1Account1);
            Assert.AreEqual("employer 1", spec1Account1.EmployerKey);
            Assert.AreEqual(long.MaxValue, spec1Account1.BalanceForAllPeriods);
            Assert.IsEmpty(spec1Account1.BalancesPerPeriod);

            var spec1Account2 = spec1.Arrangement.LevyBalances[1];
            Assert.IsNotNull(spec1Account2);
            Assert.AreEqual("employer 2", spec1Account2.EmployerKey);
            Assert.IsNull(spec1Account2.BalanceForAllPeriods);
            Assert.AreEqual(4, spec1Account2.BalancesPerPeriod.Count);
            Assert.AreEqual(5000, spec1Account2.BalancesPerPeriod["01/18"]);
            Assert.AreEqual(5000, spec1Account2.BalancesPerPeriod["02/18"]);
            Assert.AreEqual(5000, spec1Account2.BalancesPerPeriod["06/18"]);
            Assert.AreEqual(5000, spec1Account2.BalancesPerPeriod["07/18"]);
        }
    }
}
