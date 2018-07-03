using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.ServiceTests.GivenACalculator
{
    [TestFixture]
    public class WithBothPastPaymentsAndEarnings
    {
        [TestFixture]
        public class AndThePastPaymentsMatchTheEarnings
        {
            [Test, PaymentsDueAutoData]
            public void ThenThereAreNoPaymentsDue(
                List<FundingDue> earnings,
                PaymentsDueCalculationService sut
            )
            {
                var pastPayments = earnings.Select(x => new RequiredPayment(x)).ToList();
                var actual = sut.Calculate(earnings, new List<int>(), pastPayments);

                actual.Should().HaveCount(0);
            }
        }

        [TestFixture]
        public class AndThereIsOneMoreEarningThanMatchingPastPayments
        {
            [Test, PaymentsDueAutoData]
            public void ThenThePayentsDueMatchesTheEarning(
                List<FundingDue> earnings,
                PaymentsDueCalculationService sut,
                FundingDue test
            )
            {
                var pastPayments = earnings.Select(x => new RequiredPayment(x)).ToList();
                earnings.Add(test);

                var actual = sut.Calculate(earnings, new List<int>(), pastPayments);

                actual.Should().HaveCount(1);
                actual.Sum(x => x.AmountDue).Should().Be(test.AmountDue);
            }
        }

        [TestFixture]
        public class AndThereIsOneMorePastPayment
        {
            [Test, PaymentsDueAutoData]
            public void ThenThePayentsDueMatchesTheEarning(
                List<FundingDue> earnings,
                PaymentsDueCalculationService sut
            )
            {
                var pastPayments = earnings.Select(x => new RequiredPayment(x)).ToList();
                earnings.RemoveAt(0);

                var actual = sut.Calculate(earnings, new List<int>(), pastPayments);

                actual.Should().HaveCount(1);
                actual.Sum(x => x.AmountDue).Should().Be(-pastPayments[0].AmountDue);
            }
        }
    }
}
