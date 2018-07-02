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

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.DomainTests.DetermineWhichEarningsShouldBePaidTests
{
    [TestFixture]
    public class GivenADetermineWhichEarningsShouldBePaidService
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

        [Test, PaymentsDueAutoData]
        [SetupMatchingEarningsAndPastPayments(2)]
        public void CallingTwiceWithTheSameDataReturnsTheSameResults(
            DetermineWhichEarningsShouldBePaidService sut)
        {
            var runOne = sut.DeterminePayableEarnings(
                new List<DatalockOutput>(),
                _earnings,
                new List<RawEarningForMathsOrEnglish>());

            var runTwo = sut.DeterminePayableEarnings(
                new List<DatalockOutput>(),
                _earnings,
                new List<RawEarningForMathsOrEnglish>());

            runOne.Earnings.ShouldAllBeEquivalentTo(runTwo.Earnings, options => options.Excluding(x => x.Id));
            runOne.NonPayableEarnings.ShouldAllBeEquivalentTo(runTwo.NonPayableEarnings);
            runOne.PeriodsToIgnore.ShouldAllBeEquivalentTo(runTwo.PeriodsToIgnore);
        }

        [Test, PaymentsDueAutoData]
        [SetupMatchingEarningsAndPastPayments(1)]
        public void ThenTheCommitmentVersionIdShouldMatchTheEarning(
            DetermineWhichEarningsShouldBePaidService sut)
        {
            var actual = sut.DeterminePayableEarnings(
                _datalockOutput,
                _earnings,
                new List<RawEarningForMathsOrEnglish>());

            actual.Earnings.First().CommitmentVersionId.Should().Be(_datalockOutput.First().CommitmentVersionId);
        }
    }
}
