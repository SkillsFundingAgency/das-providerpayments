using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.ServiceTests.GivenACalculator
{
    [TestFixture]
    public class WithOnlyPastPayments
    {
        [Test, PaymentsDueAutoData]
        public void ThenThereShouldBeRefundsForAllPastPayments(
            List<RequiredPayment> pastPayments,
            PaymentsDueCalculationService sut
        )
        {
            var actual = sut.Calculate(new List<FundingDue>(), new HashSet<int>(), pastPayments);

            actual.Should().HaveCount(pastPayments.Count);
            foreach (var refund in pastPayments)
            {
                actual.Should().Contain(x => x.AmountDue == -refund.AmountDue);
            }
        }
    }
}
