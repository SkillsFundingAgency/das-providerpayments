using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.SetupAttributes;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.DomainTests.DetermineWhichEarningsShouldBePaidTests.MathsAndEnglish
{
    [TestFixture]
    public class GivenADecider
    {
        private List<RawEarning> _earnings;
        private List<RawEarningForMathsOrEnglish> _mathsAndEnglishEarnings;
        private List<DatalockOutput> _datalockOutput;

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
            _earnings = earningsDictionary["Earnings"] as List<RawEarning>;
            _mathsAndEnglishEarnings = earningsDictionary["MathsAndEnglishEarnings"] as List<RawEarningForMathsOrEnglish>;
            _datalockOutput = earningsDictionary["DatalockOutput"] as List<DatalockOutput>;
        }

        [Theory, PaymentsDueAutoData]
        [SetupMatchingEarningsAndPastPayments(1)]
        public void MathsEnglishWithNoPayableOnprogNotPaid(
            IDetermineWhichEarningsShouldBePaid sut)
        {
            _earnings.Clear();
            
            var actual = sut.DeterminePayableEarnings(
                new List<DatalockOutput>(), 
                _earnings,
                _mathsAndEnglishEarnings);

            actual.Earnings.Should().BeEmpty();
        }

        [Theory, PaymentsDueAutoData]
        [SetupMatchingEarningsAndPastPayments(2)]
        public void MathsEnglishWithNoPayableOnprogNotPaidForAct2(
            IDetermineWhichEarningsShouldBePaid sut)
        {
            _earnings.Clear();

            var actual = sut.DeterminePayableEarnings(
                new List<DatalockOutput>(),
                new List<RawEarning>(),
                _mathsAndEnglishEarnings);

            actual.Earnings.Should().BeEmpty();
        }

        [Theory, PaymentsDueAutoData]
        [SetupMatchingEarningsAndPastPayments(1)]
        public void MathsEnglishWithPayableOnProgPaid(
            IDetermineWhichEarningsShouldBePaid sut)
        {
            var actual = sut.DeterminePayableEarnings(
                _datalockOutput,
                _earnings,
                _mathsAndEnglishEarnings);

            actual.Earnings.Should().HaveCount(24); // M/E as well as onprog * 12
        }

        [Theory, PaymentsDueAutoData]
        [SetupMatchingEarningsAndPastPayments(2)]
        public void MathsEnglishAct2WithNoOtherEarningsArePaid(
            IDetermineWhichEarningsShouldBePaid sut)
        {
            var blankEarning = _earnings[0];
            blankEarning.TransactionType01 = 0;

            var actual = sut.DeterminePayableEarnings(
                new List<DatalockOutput>(),
                new List<RawEarning> {blankEarning},
                _mathsAndEnglishEarnings);

            actual.Earnings.Should().HaveCount(12);
        }

        [Theory, PaymentsDueAutoData]
        [SetupMatchingEarningsAndPastPayments(1)]
        public void MathsEnglishAct1WithNoOtherEarningsArePaidWhenMatchingDatalock(
            IDetermineWhichEarningsShouldBePaid sut)
        {
            var blankEarning = _earnings[0];
            blankEarning.TransactionType01 = 0;

            var actual = sut.DeterminePayableEarnings(
                _datalockOutput,
                new List<RawEarning>{blankEarning}, 
                _mathsAndEnglishEarnings);

            actual.Earnings.Should().HaveCount(12);
        }

        [Theory, PaymentsDueAutoData]
        [SetupMatchingEarningsAndPastPayments(2)]
        public void MathsEnglishAct2WithNoOtherEarningsAreNotPaidWhenNoMatchingDatalock(
            IDetermineWhichEarningsShouldBePaid sut)
        {
            var actual = sut.DeterminePayableEarnings(
                new List<DatalockOutput>(), 
                new List<RawEarning>(),
                _mathsAndEnglishEarnings);

            actual.Earnings.Should().HaveCount(0);
        }
    }
}
