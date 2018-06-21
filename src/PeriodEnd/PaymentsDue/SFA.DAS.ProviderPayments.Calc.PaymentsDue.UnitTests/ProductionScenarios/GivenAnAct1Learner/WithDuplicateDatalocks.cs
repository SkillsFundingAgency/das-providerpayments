using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.TestDataLoader;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.ProductionScenarios.GivenAnAct1Learner
{
    [TestFixture]
    public class AndPeriodsRemovedFromTheIlr
    {
        [Test]
        public void ThenThereShouldBePaymentsForR01ToR03()
        {
            var parameters = TestData.LoadFrom("DuplicateDatalocks");

            var datalockComponent = new IShouldBeInTheDatalockComponent();
            var datalockResult = datalockComponent.ValidatePriceEpisodes(
                parameters.Commitments,
                parameters.DatalockOutputs.ToList(),
                parameters.DatalockValidationErrors,
                parameters.RawEarnings,
                parameters.RawEarningsForMathsOrEnglish);

            var sut = new Learner(datalockResult.Earnings, datalockResult.PeriodsToIgnore, parameters.PastPayments);
            var actual = sut.CalculatePaymentsDue();

            actual.Should().HaveCount(3);
            actual.Sum(x => x.AmountDue).Should().Be(1500);
        }
    }
}
