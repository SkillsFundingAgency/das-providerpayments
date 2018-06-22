using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.Extensions;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.TestDataLoader;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.ProductionScenarios
{
    [TestFixture]
    public class SheetsWithPayments
    {
        [Test]
        [TestCase("DuplicateDataLocks")]
        [TestCase("IncentivePaidButNotOnprog")]
        [TestCase("SecondIncentivePaidWithOnprog")]
        [TestCase("MathsEnglishWIthNoOnprogInYear")]
        [TestCase("OverlappingDatalocks")]
        [TestCase("Act1MathsEnglishWIthNoOnprogInYear")]
        [TestCase("Act1MathsEnglish")]
        [TestCase("SfaContributionPercentageZero")]
        public void ThenThePaymentsGeneratedShouldMatchTheExpectedPayments(string filename)
        {
            var parameters = TestData.LoadFrom(filename);

            var datalockComponent = new IShouldBeInTheDatalockComponent();
            var datalockResult = datalockComponent.ValidatePriceEpisodes(
                parameters.Commitments,
                parameters.DatalockOutputs.ToList(),
                parameters.DatalockValidationErrors,
                parameters.RawEarnings,
                parameters.RawEarningsForMathsOrEnglish, 
                new DateTime(2017, 08, 01));

            var sut = new Learner(datalockResult.Earnings, datalockResult.PeriodsToIgnore, parameters.PastPayments);
            var actual = sut.CalculatePaymentsDue();

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
