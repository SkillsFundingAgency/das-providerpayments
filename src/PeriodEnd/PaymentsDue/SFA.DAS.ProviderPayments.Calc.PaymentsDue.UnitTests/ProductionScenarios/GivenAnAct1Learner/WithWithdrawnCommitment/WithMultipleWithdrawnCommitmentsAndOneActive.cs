using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services.Dependencies;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.TestDataLoader;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.ProductionScenarios.GivenAnAct1Learner.WithWithdrawnCommitment
{
    [TestFixture]
    public class WithMultipleWithdrawnCommitmentsAndOneActiveButStillDatalocked
    {
        [Theory, PaymentsDueAutoData]
        public void ThenThereShouldBeNoRefunds(IValidateRawDatalocks commitmentMatcher)
        {
            var parameters = TestData.LoadFrom("LearnerWithMultipleWithdrawnCommitmentAndOneActiveCommitment");

            var datalockOutput = commitmentMatcher.ProcessDatalocks(
                parameters.DatalockOutputs, 
                parameters.DatalockValidationErrors,
                parameters.Commitments);

            var datalockComponent = new IShouldBeInTheDatalockComponent();
            var datalockResult = datalockComponent.ValidatePriceEpisodes(
                datalockOutput,
                parameters.RawEarnings,
                parameters.RawEarningsForMathsOrEnglish,
                new DateTime(2017, 08, 01));

            var sut = new Learner(datalockResult.Earnings, datalockResult.PeriodsToIgnore, parameters.PastPayments);
            var actual = sut.CalculatePaymentsDue();

            actual.Should().HaveCount(0);
        }
    }
}


