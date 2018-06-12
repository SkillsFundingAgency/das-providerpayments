using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.SetupAttributes;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.DomainTests.LearnerTests
{
    [TestFixture]
    public class GivenAnAct1LearnerWithOverlappingCommitments
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
            _datalocks.Add(new PriceEpisode(_datalocks[0].PriceEpisodeIdentifier, _datalocks[0].PayablePeriods, -1, null, -1, null ));
        }

        [Test]
        [SetupMatchingEarningsAndPastPayments(1, onProgAmount: 100)]
        public void NoPaymentsAreMadeEvenThoughTheDatalocksArePassing()
        {
            var sut = new Learner(_earnings.Take(1), _mathsAndEnglishEarnings, _datalocks, _pastPayments.Take(0));
            var actual = sut.CalculatePaymentsDue();

            var expected = 0;
            actual.PayableEarnings.Sum(x => x.AmountDue).Should().Be(expected);
        }

        [Test]
        [SetupMatchingEarningsAndPastPayments(1, onProgAmount: 100)]
        public void Control_PaymentsAreMadeWithOneDatalock()
        {
            var sut = new Learner(_earnings.Take(1), _mathsAndEnglishEarnings, _datalocks.Take(1), _pastPayments.Take(0));
            var actual = sut.CalculatePaymentsDue();

            var expected = 100;
            actual.PayableEarnings.Sum(x => x.AmountDue).Should().Be(expected);
        }
    }
}
