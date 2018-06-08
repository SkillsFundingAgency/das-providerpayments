using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.DomainTests.LearnerTests
{
    [TestFixture]
    public class GivenASimpleCaseRefund
    {
        private static readonly IFixture Fixture = new Fixture();

        private static readonly List<RequiredPaymentEntity> PastPayments =
            Fixture.Build<RequiredPaymentEntity>()
                .CreateMany(6)
                .ToList();

        [Test]
        public void ThenThereAreNoNonPaymentsForASimpleAct1Learner()
        {
            // 1 Price Episode 
            // No Maths or English
            // 0 earnings
            // 6 past payments

            var priceEpisode1 = Fixture.Create<string>();

            var datalocks = Fixture.Build<PriceEpisode>()
                .With(x => x.PriceEpisodeIdentifier, priceEpisode1)
                .With(x => x.Payable, true)
                .CreateMany(1);
            var mathsAndEnglishEarnings = new List<RawEarningForMathsOrEnglish>();
            var earnings = new List<RawEarning>();
            

            var sut = new Learner(earnings, mathsAndEnglishEarnings, datalocks, PastPayments);
            var actual = sut.CalculatePaymentsDue();

            actual.NonPayableEarnings.Should().BeEmpty();
        }

        [Test]
        public void ThenThePaymentsAreANegativeOfTheEarningsForASimpleAct1Learner()
        {
            // 1 Price Episode 
            // No Maths or English
            // 0 earnings
            // 6 past payments

            var priceEpisode1 = Fixture.Create<string>();

            var datalocks = Fixture.Build<PriceEpisode>()
                .With(x => x.PriceEpisodeIdentifier, priceEpisode1)
                .With(x => x.Payable, true)
                .CreateMany(1);
            var mathsAndEnglishEarnings = new List<RawEarningForMathsOrEnglish>();
            var earnings = new List<RawEarning>();
            
            var sut = new Learner(earnings, mathsAndEnglishEarnings, datalocks, PastPayments);
            var actual = sut.CalculatePaymentsDue();

            var expected = PastPayments.Sum(x => x.AmountDue) * -1;
            actual.PayableEarnings.Sum(x => x.AmountDue).Should().Be(expected);
        }
    }
}
