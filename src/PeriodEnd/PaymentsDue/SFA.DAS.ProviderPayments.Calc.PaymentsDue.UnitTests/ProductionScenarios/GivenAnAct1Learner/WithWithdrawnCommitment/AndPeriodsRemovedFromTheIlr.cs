using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.TestDataLoader;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.ProductionScenarios.GivenAnAct1Learner.WithWithdrawnCommitment
{
    [TestFixture]
    public class AndPeriodsRemovedFromTheIlr
    {
        [Theory, PaymentsDueAutoData]
        public void ThenThereShouldBeRefundsForTheWithdrawnPeriod(
            LearnerProcessParametersBuilder parametersBuilder)
        {
            var parameters = TestData.LoadFrom("LearnerWithWithdrawnCommitmentAndRemovedPeriodsInTheIlr");

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

            actual.Should().HaveCount(2);
        }
    }
}
