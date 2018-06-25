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
    public class WithNoPastPayments
    {
        [Test, PaymentsDueAutoData]
        public void ThenThePaymentsMatchTheEarnings(
            List<FundingDue> earnings,
            PaymentsDueCalculationService sut
        )
        {
            var actual = sut.Calculate(earnings, new List<int>(), new List<RequiredPaymentEntity>());

            actual.Should().HaveCount(earnings.Count);
            foreach (var fundingDue in earnings)
            {
                actual.Should().Contain(x => x.AmountDue == fundingDue.AmountDue);
            }
        }
    }
}
