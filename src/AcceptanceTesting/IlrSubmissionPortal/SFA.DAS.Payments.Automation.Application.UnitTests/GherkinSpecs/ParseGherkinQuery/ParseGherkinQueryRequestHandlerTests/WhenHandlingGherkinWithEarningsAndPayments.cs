using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Payments.Automation.Application.GherkinSpecs.ParseGherkinQuery;

namespace SFA.DAS.Payments.Automation.Application.UnitTests.GherkinSpecs.ParseGherkinQuery.ParseGherkinQueryRequestHandlerTests
{
    public class WhenHandlingGherkinWithEarningsAndPayments
    {
        private ParseGherkinQueryRequest _request;
        private ParseGherkinQueryRequestHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _request = new ParseGherkinQueryRequest
            {
                GherkinSpecs = Properties.Resources.WhenHandlingGherkinWithEarningsAndPayments
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
            Assert.AreEqual(2, response.Results.Length);
        }

        [Test]
        public void ThenItShouldParseResultsForEachProvider()
        {
            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsTrue(response.IsSuccess, response.Error?.Message);

            var scenario1 = response.Results[0];
            Assert.IsNotNull(scenario1);
            Assert.IsNotNull(scenario1.Expectations);
            Assert.IsNotNull(scenario1.Expectations.EarningsAndPayments);
            Assert.AreEqual(1, scenario1.Expectations.EarningsAndPayments.Count);
            Assert.AreEqual("provider a", scenario1.Expectations.EarningsAndPayments[0].ProviderKey);

            var scenario2 = response.Results[1];
            Assert.IsNotNull(scenario2);
            Assert.IsNotNull(scenario2.Expectations);
            Assert.IsNotNull(scenario2.Expectations.EarningsAndPayments);
            Assert.AreEqual(2, scenario2.Expectations.EarningsAndPayments.Count);
            Assert.AreEqual("provider a", scenario2.Expectations.EarningsAndPayments[0].ProviderKey);
            Assert.AreEqual("provider b", scenario2.Expectations.EarningsAndPayments[1].ProviderKey);
        }

        [Test]
        public void ThenItShouldParseResultsForEachPeriod()
        {
            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsTrue(response.IsSuccess, response.Error?.Message);

            var scenario1 = response.Results[0];
            var scenario1Provider1 = scenario1.Expectations.EarningsAndPayments[0];
            Assert.AreEqual(7, scenario1Provider1.EarningAndPaymentsByPeriod.Count);
            Assert.AreEqual("08/17", scenario1Provider1.EarningAndPaymentsByPeriod[0].Period);
            Assert.AreEqual("09/17", scenario1Provider1.EarningAndPaymentsByPeriod[1].Period);
            Assert.AreEqual("10/17", scenario1Provider1.EarningAndPaymentsByPeriod[2].Period);
            Assert.AreEqual("11/17", scenario1Provider1.EarningAndPaymentsByPeriod[3].Period);
            Assert.AreEqual("12/17", scenario1Provider1.EarningAndPaymentsByPeriod[4].Period);
            Assert.AreEqual("08/18", scenario1Provider1.EarningAndPaymentsByPeriod[5].Period);
            Assert.AreEqual("09/18", scenario1Provider1.EarningAndPaymentsByPeriod[6].Period);

            var scenario2 = response.Results[1];
            var scenario2Provider1 = scenario2.Expectations.EarningsAndPayments[0];
            Assert.AreEqual(7, scenario2Provider1.EarningAndPaymentsByPeriod.Count);
            Assert.AreEqual("08/17", scenario2Provider1.EarningAndPaymentsByPeriod[0].Period);
            Assert.AreEqual("09/17", scenario2Provider1.EarningAndPaymentsByPeriod[1].Period);
            Assert.AreEqual("10/17", scenario2Provider1.EarningAndPaymentsByPeriod[2].Period);
            Assert.AreEqual("11/17", scenario2Provider1.EarningAndPaymentsByPeriod[3].Period);
            Assert.AreEqual("12/17", scenario2Provider1.EarningAndPaymentsByPeriod[4].Period);
            Assert.AreEqual("08/18", scenario2Provider1.EarningAndPaymentsByPeriod[5].Period);
            Assert.AreEqual("09/18", scenario2Provider1.EarningAndPaymentsByPeriod[6].Period);

            var scenario2Provider2 = scenario2.Expectations.EarningsAndPayments[1];
            Assert.AreEqual(7, scenario2Provider2.EarningAndPaymentsByPeriod.Count);
            Assert.AreEqual("08/17", scenario2Provider2.EarningAndPaymentsByPeriod[0].Period);
            Assert.AreEqual("09/17", scenario2Provider2.EarningAndPaymentsByPeriod[1].Period);
            Assert.AreEqual("10/17", scenario2Provider2.EarningAndPaymentsByPeriod[2].Period);
            Assert.AreEqual("11/17", scenario2Provider2.EarningAndPaymentsByPeriod[3].Period);
            Assert.AreEqual("12/17", scenario2Provider2.EarningAndPaymentsByPeriod[4].Period);
            Assert.AreEqual("08/18", scenario2Provider2.EarningAndPaymentsByPeriod[5].Period);
            Assert.AreEqual("09/18", scenario2Provider2.EarningAndPaymentsByPeriod[6].Period);
        }

        [Test]
        public void ThenItShouldParseValuesForEachPeriod()
        {
            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsTrue(response.IsSuccess, response.Error?.Message);

            var scenario1 = response.Results[0];
            var scenario1Provider1 = scenario1.Expectations.EarningsAndPayments[0];

            var scenario1Provider1Period1 = scenario1Provider1.EarningAndPaymentsByPeriod[0];
            Assert.AreEqual("08/17", scenario1Provider1Period1.Period);
            Assert.AreEqual(1000, scenario1Provider1Period1.ProviderEarnedTotal);
            Assert.AreEqual(0, scenario1Provider1Period1.ProviderPaidBySfa);
            Assert.AreEqual(0, scenario1Provider1Period1.LevyAccountDebited);
            Assert.AreEqual(1000, scenario1Provider1Period1.SfaLevyEmployerBudget);
            Assert.AreEqual(0, scenario1Provider1Period1.SfaLevyCoFundingBudget);
            Assert.AreEqual(0, scenario1Provider1Period1.SfaLevyAdditionalPaymentsBudget);
            Assert.AreEqual(2, scenario1Provider1Period1.EmployerLevyAccountsDebited.Count);
            Assert.AreEqual("employer 1", scenario1Provider1Period1.EmployerLevyAccountsDebited[0].EmployerKey);
            Assert.AreEqual(100, scenario1Provider1Period1.EmployerLevyAccountsDebited[0].Value);
            Assert.AreEqual("employer 2", scenario1Provider1Period1.EmployerLevyAccountsDebited[1].EmployerKey);
            Assert.AreEqual(150, scenario1Provider1Period1.EmployerLevyAccountsDebited[1].Value);

            var scenario1Provider1Period2 = scenario1Provider1.EarningAndPaymentsByPeriod[1];
            Assert.AreEqual("09/17", scenario1Provider1Period2.Period);
            Assert.AreEqual(1000, scenario1Provider1Period2.ProviderEarnedTotal);
            Assert.AreEqual(1000, scenario1Provider1Period2.ProviderPaidBySfa);
            Assert.AreEqual(1000, scenario1Provider1Period2.LevyAccountDebited);
            Assert.AreEqual(1000, scenario1Provider1Period2.SfaLevyEmployerBudget);
            Assert.AreEqual(0, scenario1Provider1Period2.SfaLevyCoFundingBudget);
            Assert.AreEqual(0, scenario1Provider1Period2.SfaLevyAdditionalPaymentsBudget);
            Assert.AreEqual(2, scenario1Provider1Period2.EmployerLevyAccountsDebited.Count);
            Assert.AreEqual("employer 1", scenario1Provider1Period2.EmployerLevyAccountsDebited[0].EmployerKey);
            Assert.AreEqual(200, scenario1Provider1Period2.EmployerLevyAccountsDebited[0].Value);
            Assert.AreEqual("employer 2", scenario1Provider1Period2.EmployerLevyAccountsDebited[1].EmployerKey);
            Assert.AreEqual(250, scenario1Provider1Period2.EmployerLevyAccountsDebited[1].Value);

            var scenario1Provider1Period3 = scenario1Provider1.EarningAndPaymentsByPeriod[2];
            Assert.AreEqual("10/17", scenario1Provider1Period3.Period);
            Assert.AreEqual(1000, scenario1Provider1Period3.ProviderEarnedTotal);
            Assert.AreEqual(1000, scenario1Provider1Period3.ProviderPaidBySfa);
            Assert.AreEqual(1000, scenario1Provider1Period3.LevyAccountDebited);
            Assert.AreEqual(1000, scenario1Provider1Period3.SfaLevyEmployerBudget);
            Assert.AreEqual(0, scenario1Provider1Period3.SfaLevyCoFundingBudget);
            Assert.AreEqual(0, scenario1Provider1Period3.SfaLevyAdditionalPaymentsBudget);
            Assert.AreEqual(2, scenario1Provider1Period3.EmployerLevyAccountsDebited.Count);
            Assert.AreEqual("employer 1", scenario1Provider1Period3.EmployerLevyAccountsDebited[0].EmployerKey);
            Assert.AreEqual(300, scenario1Provider1Period3.EmployerLevyAccountsDebited[0].Value);
            Assert.AreEqual("employer 2", scenario1Provider1Period3.EmployerLevyAccountsDebited[1].EmployerKey);
            Assert.AreEqual(350, scenario1Provider1Period3.EmployerLevyAccountsDebited[1].Value);

            var scenario1Provider1Period4 = scenario1Provider1.EarningAndPaymentsByPeriod[3];
            Assert.AreEqual("11/17", scenario1Provider1Period4.Period);
            Assert.AreEqual(2000, scenario1Provider1Period4.ProviderEarnedTotal);
            Assert.AreEqual(1000, scenario1Provider1Period4.ProviderPaidBySfa);
            Assert.AreEqual(1000, scenario1Provider1Period4.LevyAccountDebited);
            Assert.AreEqual(1000, scenario1Provider1Period4.SfaLevyEmployerBudget);
            Assert.AreEqual(0, scenario1Provider1Period4.SfaLevyCoFundingBudget);
            Assert.AreEqual(1000, scenario1Provider1Period4.SfaLevyAdditionalPaymentsBudget);
            Assert.AreEqual(2, scenario1Provider1Period4.EmployerLevyAccountsDebited.Count);
            Assert.AreEqual("employer 1", scenario1Provider1Period4.EmployerLevyAccountsDebited[0].EmployerKey);
            Assert.AreEqual(400, scenario1Provider1Period4.EmployerLevyAccountsDebited[0].Value);
            Assert.AreEqual("employer 2", scenario1Provider1Period4.EmployerLevyAccountsDebited[1].EmployerKey);
            Assert.AreEqual(450, scenario1Provider1Period4.EmployerLevyAccountsDebited[1].Value);

            var scenario1Provider1Period5 = scenario1Provider1.EarningAndPaymentsByPeriod[4];
            Assert.AreEqual("12/17", scenario1Provider1Period5.Period);
            Assert.AreEqual(1000, scenario1Provider1Period5.ProviderEarnedTotal);
            Assert.AreEqual(2000, scenario1Provider1Period5.ProviderPaidBySfa);
            Assert.AreEqual(1000, scenario1Provider1Period5.LevyAccountDebited);
            Assert.AreEqual(1000, scenario1Provider1Period5.SfaLevyEmployerBudget);
            Assert.AreEqual(0, scenario1Provider1Period5.SfaLevyCoFundingBudget);
            Assert.AreEqual(0, scenario1Provider1Period5.SfaLevyAdditionalPaymentsBudget);
            Assert.AreEqual(2, scenario1Provider1Period5.EmployerLevyAccountsDebited.Count);
            Assert.AreEqual("employer 1", scenario1Provider1Period5.EmployerLevyAccountsDebited[0].EmployerKey);
            Assert.AreEqual(500, scenario1Provider1Period5.EmployerLevyAccountsDebited[0].Value);
            Assert.AreEqual("employer 2", scenario1Provider1Period5.EmployerLevyAccountsDebited[1].EmployerKey);
            Assert.AreEqual(550, scenario1Provider1Period5.EmployerLevyAccountsDebited[1].Value);

            var scenario1Provider1Period6 = scenario1Provider1.EarningAndPaymentsByPeriod[5];
            Assert.AreEqual("08/18", scenario1Provider1Period6.Period);
            Assert.AreEqual(1000, scenario1Provider1Period6.ProviderEarnedTotal);
            Assert.AreEqual(1000, scenario1Provider1Period6.ProviderPaidBySfa);
            Assert.AreEqual(1000, scenario1Provider1Period6.LevyAccountDebited);
            Assert.AreEqual(0, scenario1Provider1Period6.SfaLevyEmployerBudget);
            Assert.AreEqual(0, scenario1Provider1Period6.SfaLevyCoFundingBudget);
            Assert.AreEqual(1000, scenario1Provider1Period6.SfaLevyAdditionalPaymentsBudget);
            Assert.AreEqual(2, scenario1Provider1Period6.EmployerLevyAccountsDebited.Count);
            Assert.AreEqual("employer 1", scenario1Provider1Period6.EmployerLevyAccountsDebited[0].EmployerKey);
            Assert.AreEqual(600, scenario1Provider1Period6.EmployerLevyAccountsDebited[0].Value);
            Assert.AreEqual("employer 2", scenario1Provider1Period6.EmployerLevyAccountsDebited[1].EmployerKey);
            Assert.AreEqual(650, scenario1Provider1Period6.EmployerLevyAccountsDebited[1].Value);

            var scenario1Provider1Period7 = scenario1Provider1.EarningAndPaymentsByPeriod[6];
            Assert.AreEqual("09/18", scenario1Provider1Period7.Period);
            Assert.AreEqual(0, scenario1Provider1Period7.ProviderEarnedTotal);
            Assert.AreEqual(1000, scenario1Provider1Period7.ProviderPaidBySfa);
            Assert.AreEqual(0, scenario1Provider1Period7.LevyAccountDebited);
            Assert.AreEqual(0, scenario1Provider1Period7.SfaLevyEmployerBudget);
            Assert.AreEqual(0, scenario1Provider1Period7.SfaLevyCoFundingBudget);
            Assert.AreEqual(0, scenario1Provider1Period7.SfaLevyAdditionalPaymentsBudget);
            Assert.AreEqual(2, scenario1Provider1Period7.EmployerLevyAccountsDebited.Count);
            Assert.AreEqual("employer 1", scenario1Provider1Period7.EmployerLevyAccountsDebited[0].EmployerKey);
            Assert.AreEqual(700, scenario1Provider1Period7.EmployerLevyAccountsDebited[0].Value);
            Assert.AreEqual("employer 2", scenario1Provider1Period7.EmployerLevyAccountsDebited[1].EmployerKey);
            Assert.AreEqual(750, scenario1Provider1Period7.EmployerLevyAccountsDebited[1].Value);
        }

    }
}
