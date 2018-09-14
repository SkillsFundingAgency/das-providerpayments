using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.SetupAttributes;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.DomainTests.DetermineWhichEarningsShouldBePaidTests.Incentives
{
    [TestFixture]
    public class GivenADetermineWhichEarningsShouldBePaidService
    {
        private List<RawEarning> _earnings;
        
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
        }

        [Test, PaymentsDueAutoData]
        [SetupMatchingEarningsAndPastPayments(2)]
        public void IncentivePaymentsAreMarkedWithOneHundredPercentSfaContribution(
            DetermineWhichEarningsShouldBePaidService sut)
        {
            for (var i = 0; i < 12; i++)
            {
                _earnings[i].TransactionType01 = 0;
                _earnings[i].TransactionType04 = 500;
                _earnings[i].TransactionType05 = 500;
                _earnings[i].TransactionType06 = 500;
                _earnings[i].TransactionType07 = 500;
                _earnings[i].TransactionType08 = 500;
                _earnings[i].TransactionType09 = 500;
                _earnings[i].TransactionType10 = 500;
                _earnings[i].TransactionType11 = 500;
                _earnings[i].TransactionType12 = 500;
                _earnings[i].TransactionType13 = 500;
                _earnings[i].TransactionType14 = 500;
                _earnings[i].TransactionType15 = 500;
            }

            var actual = sut.DeterminePayableEarnings(
                new List<DatalockOutput>(), 
                _earnings,
                new List<RawEarningForMathsOrEnglish>());

            foreach (var actualEarning in actual.Earnings)
            {
                actualEarning.SfaContributionPercentage.Should().Be(1);
            }
        }

        [Test, PaymentsDueAutoData]
        [SetupMatchingEarningsAndPastPayments(2)]
        public void OnProgPaymentsAreMarkedWithSameSfaContributionAsEarning(
            DetermineWhichEarningsShouldBePaidService sut)
        {
            for (var i = 0; i < 12; i++)
            {
                _earnings[i].TransactionType01 = 400;
                _earnings[i].TransactionType02 = 400;
                _earnings[i].TransactionType03 = 400;
                _earnings[i].TransactionType04 = 0;
                _earnings[i].TransactionType05 = 0;
                _earnings[i].TransactionType06 = 0;
                _earnings[i].TransactionType07 = 0;
                _earnings[i].TransactionType08 = 0;
                _earnings[i].TransactionType09 = 0;
                _earnings[i].TransactionType10 = 0;
                _earnings[i].TransactionType11 = 0;
                _earnings[i].TransactionType12 = 0;
                _earnings[i].TransactionType13 = 0;
                _earnings[i].TransactionType14 = 0;
                _earnings[i].TransactionType15 = 0;
            }

            var actual = sut.DeterminePayableEarnings(
                new List<DatalockOutput>(),
                _earnings.Take(1).ToList(),
                new List<RawEarningForMathsOrEnglish>());

            foreach (var actualEarning in actual.Earnings)
            {
                actualEarning.SfaContributionPercentage.Should().Be(_earnings[0].SfaContributionPercentage);
            }
        }
    }
}
