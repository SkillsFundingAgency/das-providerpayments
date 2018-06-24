﻿using System;
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
            SortProviderDataIntoLearnerData parametersBuilder,
            DatalockValidationService commitmentMatcher)
        {
            var parameters = TestData.LoadFrom("LearnerWithWithdrawnCommitmentAndRemovedPeriodsInTheIlr");

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

            actual.Should().HaveCount(2);
        }
    }
}
