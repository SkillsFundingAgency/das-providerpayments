using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.TestDataLoader;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.ProductionScenarios.GivenAnAct1Learner.WithWithdrawnCommitment
{
    [TestFixture]
    public class WithMultipleWithdrawnCommitmentsAndOneActiveButStillDatalocked
    {
        [Test]
        public void ThenThereShouldBeNoRefunds()
        {
            var parameters = TestData.LoadFrom("LearnerWithMultipleWithdrawnCommitmentAndOneActiveCommitment");

            var datalockComponent = new IShouldBeInTheDatalockComponent();
            var datalockResult = datalockComponent.ValidatePriceEpisodes(
                parameters.Commitments,
                parameters.DatalockOutputs,
                parameters.DatalockValidationErrors,
                parameters.RawEarnings,
                parameters.RawEarningsForMathsOrEnglish);

            var sut = new Learner(datalockResult.Earnings, datalockResult.PeriodsToIgnore, parameters.PastPayments);
            var actual = sut.CalculatePaymentsDue();

            actual.Should().HaveCount(0);
        }
    }
}


