using System;
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
            var datalockResult = datalockComponent.ValidatePriceEpisodes(parameters.Commitments, parameters.DatalockOutputs, new DateTime(2018, 07, 31));

            var sut = new Learner(parameters.RawEarnings, parameters.RawEarningsForMathsOrEnglish, datalockResult, parameters.PastPayments);
            var actual = sut.CalculatePaymentsDue();

            actual.PayableEarnings.Should().HaveCount(0);
        }
    }
}


