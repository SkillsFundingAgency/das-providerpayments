using System;
using System.Linq;
using Castle.Core.Logging;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.TestDataLoader;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.ProductionScenarios
{
    [TestFixture]
    public class ScenarioTesting
    {
        [Test]
        [TestCase("DuplicateDataLocks")]
        [TestCase("IncentivePaidButNotOnprog")]
        [TestCase("SecondIncentivePaidWithOnprog")]
        [TestCase("MathsEnglishWIthNoOnprogInYear")]
        [TestCase("OverlappingDatalocks")]
        [TestCase("Act1MathsEnglishWIthNoOnprogInYear")]
        [TestCase("Act1MathsEnglish")]
        public void ThenThePaymentsGeneratedShouldMatchTheExpectedPayments(string filename)
        {
            var parameters = TestData.LoadFrom(filename);

            var commitmentMatcher = new DatalockValidationService(NullLogger.Instance);
            var datalockOutput = commitmentMatcher.ProcessDatalocks(
                parameters.DatalockOutputs,
                parameters.DatalockValidationErrors,
                parameters.Commitments);

            var datalockComponent = new IDetermineWhichEarningsShouldBePaid();
            var datalockResult = datalockComponent.ValidatePriceEpisodes(
                datalockOutput,
                parameters.RawEarnings,
                parameters.RawEarningsForMathsOrEnglish, 
                new DateTime(2017, 08, 01));

            var sut = new PaymentsDueCalculationService(datalockResult.Earnings, datalockResult.PeriodsToIgnore, parameters.PastPayments);
            var actual = sut.Calculate();

            actual.Should().HaveCount(parameters.Payments.Count);
            actual.Sum(x => x.AmountDue).Should().Be(parameters.Payments.Sum(x => x.AmountDue));

            foreach (var payment in parameters.Payments)
            {
                actual.Should().Contain(x =>
                    x.TransactionType == payment.TransactionType &&
                    x.AmountDue == payment.AmountDue &&
                    x.PriceEpisodeIdentifier == payment.PriceEpisodeIdentifier &&
                    x.DeliveryMonth == payment.DeliveryMonth &&
                    x.DeliveryYear == payment.DeliveryYear);
            }
        }
    }
}
