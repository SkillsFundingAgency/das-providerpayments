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
    public class GivenAnAct1LearnerChangesPrice
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
        [SetupMatchingEarningsAndPastPayments(1, onProgAmount:100)]
        public void ThereArePaymentsForR01()
        {
            var sut = new Learner(_earnings.Take(1), _mathsAndEnglishEarnings, _datalocks, _pastPayments.Take(0));
            var actual = sut.CalculatePaymentsDue();

            var expected = _earnings.Skip(0).Take(1).TotalAmount();
            actual.PayableEarnings.Sum(x => x.AmountDue).Should().Be(expected);
        }

        [Test]
        [SetupMatchingEarningsAndPastPayments(1, onProgAmount:100)]
        public void ThereArePaymentsForR02WhichIncludeTheMissingAmountFromR01()
        {
            _earnings.ForEach(x => x.TransactionType01 = 300);

            var sut = new Learner(_earnings.Take(2), _mathsAndEnglishEarnings, _datalocks, _pastPayments.Take(1));
            var actual = sut.CalculatePaymentsDue();

            var expected = 500; 
            actual.PayableEarnings.Sum(x => x.AmountDue).Should().Be(expected);
        }

        [Test]
        [SetupMatchingEarningsAndPastPayments(1, onProgAmount:100)]
        public void ThereArePaymentsForR02WhichIncludeTheRefundAmountFromR01()
        {
            _earnings.ForEach(x => x.TransactionType01 = 75);

            var sut = new Learner(_earnings.Take(2), _mathsAndEnglishEarnings, _datalocks, _pastPayments.Take(1));
            var actual = sut.CalculatePaymentsDue();

            var expected = 50; 
            actual.PayableEarnings.Sum(x => x.AmountDue).Should().Be(expected);
        }

        [Test]
        [SetupMatchingEarningsAndPastPayments(1, onProgAmount:100)]
        public void ThereIsARefundPaymentsForR02BecauseTheBigPriceReductionFromR01()
        {
            _earnings.ForEach(x => x.TransactionType01 = 10);

            var sut = new Learner(_earnings.Take(2), _mathsAndEnglishEarnings, _datalocks, _pastPayments.Take(1));
            var actual = sut.CalculatePaymentsDue();

            var expected = -80;
            actual.PayableEarnings.Sum(x => x.AmountDue).Should().Be(expected);
        }
    }
}
