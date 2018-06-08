using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.Extensions;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.DomainTests.LearnerTests
{
    [TestFixture]
    public class WhenCalculatingPaymentsDue
    {
        private static readonly IFixture Fixture = new Fixture();

        [Test]
        public void ThenThereAreNoNonPaymentsForASimpleAct1Learner()
        {
            // 1 Price Episode 
            // No Maths or English
            // 6 earnings
            // No past payments

            var priceEpisode1 = Fixture.Create<string>();

            var datalocks = Fixture.Build<PriceEpisode>()
                .With(x => x.PriceEpisodeIdentifier, priceEpisode1)
                .With(x => x.Payable, true)
                .CreateMany(1);
            var mathsAndEnglishEarnings = new List<RawEarningForMathsOrEnglish>();
            var earnings = Fixture.Build<RawEarning>()
                .With(x => x.PriceEpisodeIdentifier, priceEpisode1)
                .CreateMany(6);
            var pastPayments = new List<RequiredPaymentEntity>();

            var sut = new Learner(earnings, mathsAndEnglishEarnings, datalocks, pastPayments);
            var actual = sut.CalculatePaymentsDue();

            actual.NonPayableEarnings.Should().BeEmpty();
        }

        [Test]
        public void ThenThereAreTheCorrectNumberOfPaymentsForASimpleAct1Learner()
        {
            // 1 Price Episode 
            // No Maths or English
            // 6 earnings
            // No past payments

            var priceEpisode1 = Fixture.Create<string>();

            var datalocks = Fixture.Build<PriceEpisode>()
                .With(x => x.PriceEpisodeIdentifier, priceEpisode1)
                .With(x => x.Payable, true)
                .CreateMany(1);
            var mathsAndEnglishEarnings = new List<RawEarningForMathsOrEnglish>();
            var earnings = Fixture.Build<RawEarning>()
                .With(x => x.PriceEpisodeIdentifier, priceEpisode1)
                .CreateMany(6);
            var pastPayments = new List<RequiredPaymentEntity>();

            var sut = new Learner(earnings, mathsAndEnglishEarnings, datalocks, pastPayments);
            var actual = sut.CalculatePaymentsDue();

            var expected = earnings.NumberOfNonZeroTransactions();
            actual.PayableEarnings.Should().HaveCount(expected);
        }

        [Test]
        public void ThenThePaymentsAreCorrectForASimpleAct1Learner()
        {
            // 1 Price Episode 
            // No Maths or English
            // 6 earnings
            // No past payments

            var priceEpisode1 = Fixture.Create<string>();

            var datalocks = Fixture.Build<PriceEpisode>()
                .With(x => x.PriceEpisodeIdentifier, priceEpisode1)
                .With(x => x.Payable, true)
                .CreateMany(1);
            var mathsAndEnglishEarnings = new List<RawEarningForMathsOrEnglish>();
            var earnings = Fixture.Build<RawEarning>()
                .With(x => x.PriceEpisodeIdentifier, priceEpisode1)
                .CreateMany(6);
            var pastPayments = new List<RequiredPaymentEntity>();

            var sut = new Learner(earnings, mathsAndEnglishEarnings, datalocks, pastPayments);
            var actual = sut.CalculatePaymentsDue();

            var expected = earnings.TotalAmount();
            actual.PayableEarnings.Sum(x => x.AmountDue).Should().Be(expected);
        }


        [Test]
        public void ThenThereAreNoNonPayablePaymentsForASimpleAct2Learner()
        {
            // No datalock price episodes
            // No maths and english
            // 6 earnings
            // No past payments

            var priceEpisode1 = Fixture.Create<string>();

            var datalocks = new List<PriceEpisode>();
            var mathsAndEnglishEarnings = new List<RawEarningForMathsOrEnglish>();
            var earnings = Fixture.Build<RawEarning>()
                .With(x => x.PriceEpisodeIdentifier, priceEpisode1)
                .CreateMany(6);
            var pastPayments = new List<RequiredPaymentEntity>();
            
            var sut = new Learner(earnings, mathsAndEnglishEarnings, datalocks, pastPayments);
            var actual = sut.CalculatePaymentsDue();

            actual.NonPayableEarnings.Should().BeEmpty();
        }

        [Test]
        public void ThenThereAreTheCorrectNumberOfPaymentsForASimpleAct2Learner()
        {
            // No datalock price episodes
            // No maths and english
            // 6 earnings
            // No past payments

            var priceEpisode1 = Fixture.Create<string>();

            var datalocks = new List<PriceEpisode>();
            var mathsAndEnglishEarnings = new List<RawEarningForMathsOrEnglish>();
            var earnings = Fixture.Build<RawEarning>()
                .With(x => x.PriceEpisodeIdentifier, priceEpisode1)
                .CreateMany(6)
                .ToList();
            var pastPayments = new List<RequiredPaymentEntity>();

            var sut = new Learner(earnings, mathsAndEnglishEarnings, datalocks, pastPayments);
            var actual = sut.CalculatePaymentsDue();

            var expected = earnings.NumberOfNonZeroTransactions();
            actual.PayableEarnings.Should().HaveCount(expected);
        }
        
        [Test]
        public void ThenThePaymentsAreCorrectForASimpleAct2Learner()
        {
            // No datalock price episodes
            // No maths and english
            // 6 earnings
            // No past payments

            var priceEpisode1 = Fixture.Create<string>();

            var datalocks = new List<PriceEpisode>();
            var mathsAndEnglishEarnings = new List<RawEarningForMathsOrEnglish>();
            var earnings = Fixture.Build<RawEarning>()
                .With(x => x.PriceEpisodeIdentifier, priceEpisode1)
                .CreateMany(6)
                .ToList();
            var pastPayments = new List<RequiredPaymentEntity>();

            var sut = new Learner(earnings, mathsAndEnglishEarnings, datalocks, pastPayments);
            var actual = sut.CalculatePaymentsDue();

            var expected = earnings.TotalAmount();
            actual.PayableEarnings.Sum(x => x.AmountDue).Should().Be(expected);
        }
    }
}
