using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.Extensions;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.SetupAttributes;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.DomainTests.LearnerTests
{
    [TestFixture]
    public class GivenAnAct2LearnerWithPartialRefund
    {
        [TestFixture]
        public class PriceChangeFrom500To750InR03
        {
            private List<PriceEpisode> _datalocks;
            private List<RawEarning> _earnings;
            private List<RawEarningForMathsOrEnglish> _mathsAndEnglishEarnings;
            private List<RequiredPaymentEntity> _pastPayments;

            [SetUp]
            public void Setup()
            {
                var list = TestContext.CurrentContext.Test.Properties["EarningsDictionary"];
                if (list.Count == 0)
                {
                    throw new Exception("Please include a setup attribute in your test");
                }
                var earningsDictionary = list[0] as Dictionary<string, object>;
                if (earningsDictionary == null)
                {
                    throw new Exception("Please include a setup attribute in your test");
                }
                _datalocks = earningsDictionary["Datalocks"] as List<PriceEpisode>;
                _earnings = earningsDictionary["Earnings"] as List<RawEarning>;
                _mathsAndEnglishEarnings = earningsDictionary["MathsAndEnglishEarnings"] as List<RawEarningForMathsOrEnglish>;
                _pastPayments = earningsDictionary["PastPayments"] as List<RequiredPaymentEntity>;
            }

            [Test]
            [SetupMatchingEarningsAndPastPayments(2, onProgAmount: 500)]
            public void ThereArePaymentsForR01Of500()
            {
                var sut = new Learner(_earnings.Take(1), _mathsAndEnglishEarnings, _datalocks, _pastPayments.Take(0));
                var actual = sut.CalculatePaymentsDue();

                var expected = 500;
                actual.PayableEarnings.Sum(x => x.AmountDue).Should().Be(expected);
            }

            [Test]
            [SetupMatchingEarningsAndPastPayments(2, onProgAmount: 500)]
            public void ThereArePaymentsForR02Of500()
            {
                var sut = new Learner(_earnings.Take(2), _mathsAndEnglishEarnings, _datalocks, _pastPayments.Take(1));
                var actual = sut.CalculatePaymentsDue();

                var expected = 500;
                actual.PayableEarnings.Sum(x => x.AmountDue).Should().Be(expected);
            }

            [Test]
            [SetupMatchingEarningsAndPastPayments(2, onProgAmount: 500)]
            public void WithAPriceIncreaseTo750_ThereAreCorrectPaymentsForR03()
            {
                _earnings[0].TransactionType01 = 750;
                _earnings[1].TransactionType01 = 750;
                _earnings[2].TransactionType01 = 750;

                var sut = new Learner(_earnings.Take(3), _mathsAndEnglishEarnings, _datalocks, _pastPayments.Take(2));
                var actual = sut.CalculatePaymentsDue();

                var expected = 1250;
                actual.PayableEarnings.Sum(x => x.AmountDue).Should().Be(expected);

                actual.PayableEarnings.Should().HaveCount(3);
                actual.PayableEarnings[0].AmountDue.Should().Be(250);
                actual.PayableEarnings[1].AmountDue.Should().Be(250);
                actual.PayableEarnings[2].AmountDue.Should().Be(750);
            }

            [Test]
            [SetupMatchingEarningsAndPastPayments(2, onProgAmount: 500)]
            public void ThereArePaymentsForR04Of750()
            {
                _earnings[0].TransactionType01 = 750;
                _earnings[1].TransactionType01 = 750;
                _earnings[2].TransactionType01 = 750;
                _earnings[3].TransactionType01 = 750;

                _pastPayments[0].AmountDue = 500;
                _pastPayments[1].AmountDue = 500;
                _pastPayments[2].AmountDue = 750;
                _pastPayments[3].AmountDue = 250;
                _pastPayments[4].AmountDue = 250;

                _pastPayments[0].CopySignificantPaymentPropertiesTo(_pastPayments[3]);
                _pastPayments[1].CopySignificantPaymentPropertiesTo(_pastPayments[4]);

                var sut = new Learner(_earnings.Take(4), _mathsAndEnglishEarnings, _datalocks, _pastPayments.Take(5));
                var actual = sut.CalculatePaymentsDue();

                var expected = 750;
                actual.PayableEarnings.Sum(x => x.AmountDue).Should().Be(expected);
            }
        }
    }
}
