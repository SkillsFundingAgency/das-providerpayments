﻿using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.ReadOnlyClassManipulation;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.SetupAttributes;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.DomainTests.DetermineWhichEarningsShouldBePaidTests.OnProg
{
    [TestFixture]
    public class GivenADecider
    {
        private List<RawEarning> _earnings;
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
            _datalockOutput = earningsDictionary["DatalockOutput"] as List<DatalockOutput>;
        }

        [Theory, PaymentsDueAutoData]
        [SetupMatchingEarningsAndPastPayments(1, academicYear:"1617")]
        public void IfEarningsAreForAPreviousYearTheyShouldBeIgnored(
            DetermineWhichEarningsShouldBePaidService sut)
        {
            var actual = sut.DeterminePayableEarnings(
                _datalockOutput,
                _earnings,
                new List<RawEarningForMathsOrEnglish>());

            actual.Earnings.Should().HaveCount(0);
        }

        [Theory, PaymentsDueAutoData]
        [SetupMatchingEarningsAndPastPayments(1, academicYear:"1819")]
        public void IfEarningsAreForAFutureYearTheyShouldBeIgnored(
            DetermineWhichEarningsShouldBePaidService sut)
        {
            var actual = sut.DeterminePayableEarnings(
                _datalockOutput,
                _earnings,
                new List<RawEarningForMathsOrEnglish>());

            actual.Earnings.Should().HaveCount(0);
        }

        [Theory, PaymentsDueAutoData]
        [SetupMatchingEarningsAndPastPayments(2)]
        public void IfEarningsAreAct2PayThemAll(
            DetermineWhichEarningsShouldBePaidService sut)
        {
            var actual = sut.DeterminePayableEarnings(
                _datalockOutput,
                _earnings,
                new List<RawEarningForMathsOrEnglish>());

            actual.Earnings.Should().HaveCount(12);
        }

        [Theory, PaymentsDueAutoData]
        [SetupMatchingEarningsAndPastPayments(1)]
        public void IfEarningsAreAct1AndHaveAMatchingDatalock(
            DetermineWhichEarningsShouldBePaidService sut)
        {
            var actual = sut.DeterminePayableEarnings(
                _datalockOutput,
                _earnings,
                new List<RawEarningForMathsOrEnglish>());

            actual.Earnings.Should().HaveCount(12);
        }

        [Theory, PaymentsDueAutoData]
        [SetupMatchingEarningsAndPastPayments(1)]
        public void IfEarningsAreAct1WithNoMatchingDatalockIgnorePeriodIsSet(
            DetermineWhichEarningsShouldBePaidService sut)
        {
            var expectedPeriodToIgnore = _datalockOutput[0].Period;
            _datalockOutput.RemoveAt(0);

            var actual = sut.DeterminePayableEarnings(
                _datalockOutput,
                _earnings,
                new List<RawEarningForMathsOrEnglish>());

            actual.PeriodsToIgnore.Should().Contain(expectedPeriodToIgnore);
        }

        [Theory, PaymentsDueAutoData]
        [SetupMatchingEarningsAndPastPayments(1)]
        public void IfEarningsAreAct1WithMatchingDatalockTheyArePaid(
            DetermineWhichEarningsShouldBePaidService sut)
        {
            var actual = sut.DeterminePayableEarnings(
                _datalockOutput,
                _earnings,
                new List<RawEarningForMathsOrEnglish>());

            actual.Earnings.Should().HaveCount(12);
        }

        [Theory, PaymentsDueAutoData]
        [SetupMatchingEarningsAndPastPayments(1)]
        public void EarningsWithMatchingDatalockTransFlag1IncentivesNotPaid(
            DetermineWhichEarningsShouldBePaidService sut)
        {
            for (var i = 0; i < 12; i++)
            {
                _earnings[i].TransactionType01 = 150;
                _earnings[i].TransactionType04 = 500;
                _earnings[i].TransactionType05 = 500;
                _earnings[i].TransactionType06 = 500;
                _earnings[i].TransactionType07 = 500;
            }

            var actual = sut.DeterminePayableEarnings(
                _datalockOutput,
                _earnings,
                new List<RawEarningForMathsOrEnglish>());

            var expectedAmount = 150 * 12;
            actual.Earnings.Should().HaveCount(12);
            actual.Earnings.Sum(x => x.AmountDue).Should().Be(expectedAmount);
        }

        [Theory, PaymentsDueAutoData]
        [SetupMatchingEarningsAndPastPayments(1)]
        public void EarningsWithMatchingDatalockTransFlag2OnlyFirstIncentivesPaid(
            DetermineWhichEarningsShouldBePaidService sut)
        {
            for (var i = 0; i < 12; i++)
            {
                _earnings[i].TransactionType01 = 150;
                _earnings[i].TransactionType04 = 500;
                _earnings[i].TransactionType05 = 500;
                _earnings[i].TransactionType06 = 2500;
                _earnings[i].TransactionType07 = 2500;

                WriteToDatalockOutput.SetTransactionTypeFlag(_datalockOutput[i], 2);
            }

            var actual = sut.DeterminePayableEarnings(
                _datalockOutput,
                _earnings,
                new List<RawEarningForMathsOrEnglish>());

            var expectedAmount = 1000 * 12; // (500 X 2)
            actual.Earnings.Should().HaveCount(24);
            actual.Earnings.Sum(x => x.AmountDue).Should().Be(expectedAmount);
        }

        [Theory, PaymentsDueAutoData]
        [SetupMatchingEarningsAndPastPayments(1)]
        public void EarningsWithMatchingDatalockTransFlag3OnlySecondIncentivesPaid(
            DetermineWhichEarningsShouldBePaidService sut)
        {
            for (var i = 0; i < 12; i++)
            {
                _earnings[i].TransactionType01 = 150;
                _earnings[i].TransactionType04 = 500;
                _earnings[i].TransactionType05 = 500;
                _earnings[i].TransactionType06 = 2500;
                _earnings[i].TransactionType07 = 2500;

                WriteToDatalockOutput.SetTransactionTypeFlag(_datalockOutput[i], 3);
            }

            var actual = sut.DeterminePayableEarnings(
                _datalockOutput,
                _earnings,
                new List<RawEarningForMathsOrEnglish>());

            var expectedAmount = 5000 * 12; // (2500 X 2)
            actual.Earnings.Should().HaveCount(24);
            actual.Earnings.Sum(x => x.AmountDue).Should().Be(expectedAmount);
        }
    }
}