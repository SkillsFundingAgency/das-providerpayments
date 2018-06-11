using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.Extensions;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.SetupAttributes;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.DomainTests.LearnerTests
{
    [TestFixture]
    public class GivenAnAct1LearnerWithChangingDatalock
    {
        private static readonly IFixture Fixture = new Fixture();

        [TestFixture]
        public class DatalockSuccessInR01AndR02FailureInR03SuccessInR04
        {
            private List<PriceEpisode> Datalocks;
            private List<RawEarning> Earnings;
            private List<RawEarningForMathsOrEnglish> MathsAndEnglishEarnings;
            private List<RequiredPaymentEntity> PastPayments;

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
                    
                }
                Datalocks = earningsDictionary["Datalocks"] as List<PriceEpisode>;
                Earnings = earningsDictionary["Earnings"] as List<RawEarning>;
                MathsAndEnglishEarnings = earningsDictionary["MathsAndEnglishEarnings"] as List<RawEarningForMathsOrEnglish>;
                PastPayments = earningsDictionary["PastPayments"] as List<RequiredPaymentEntity>;
            }

            [Test]
            [SetupMatchingEarningsAndPastPayments(1)]
            public void WithPassingDatalock_ThereArePaymentsForR01()
            {
                Datalocks[0].Payable = true;

                var sut = new Learner(Earnings.Take(1), MathsAndEnglishEarnings, Datalocks, PastPayments.Take(0));
                var actual = sut.CalculatePaymentsDue();

                var expected = Earnings.Skip(0).Take(1).TotalAmount();
                actual.PayableEarnings.Sum(x => x.AmountDue).Should().Be(expected);
            }

            [Test]
            [SetupMatchingEarningsAndPastPayments(1)]
            public void WithPassingDatalock_ThereArePaymentsForR02()
            {
                Datalocks[0].Payable = true;

                var sut = new Learner(Earnings.Take(2), MathsAndEnglishEarnings, Datalocks, PastPayments.Take(1));
                var actual = sut.CalculatePaymentsDue();

                var expected = Earnings.Skip(1).Take(1).TotalAmount();
                actual.PayableEarnings.Sum(x => x.AmountDue).Should().Be(expected);
            }

            [Test]
            [SetupMatchingEarningsAndPastPayments(1)]
            public void WithFailingDatalock_ThereAreNoPaymentsForR03()
            {
                Datalocks[0].Payable = false;
                var sut = new Learner(Earnings.Take(3), MathsAndEnglishEarnings, Datalocks, PastPayments.Take(2));
                var actual = sut.CalculatePaymentsDue();

                var expected = 0;
                actual.PayableEarnings.Sum(x => x.AmountDue).Should().Be(expected);
            }

            [Test]
            [SetupMatchingEarningsAndPastPayments(1)]
            public void WithPassingDatalock_ThereArePaymentsForR04()
            {
                Datalocks[0].Payable = true;
                var sut = new Learner(Earnings.Take(4), MathsAndEnglishEarnings, Datalocks, PastPayments.Take(2));
                var actual = sut.CalculatePaymentsDue();

                var expected = Earnings.Skip(2).Take(2).TotalAmount();
                actual.PayableEarnings.Sum(x => x.AmountDue).Should().Be(expected);
            }
        }
    }
}
