using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.SetupAttributes;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.DomainTests.DetermineWhichEarningsShouldBePaidTests
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
        [SetupMatchingEarningsAndPastPayments(ApprenticeshipContractType.NonLevy)]
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

            runOne.PayableEarnings.ShouldAllBeEquivalentTo(runTwo.PayableEarnings, options => options.Excluding(x => x.Id));
            runOne.NonPayableEarnings.ShouldAllBeEquivalentTo(runTwo.NonPayableEarnings);
            runOne.PeriodsToIgnore.ShouldAllBeEquivalentTo(runTwo.PeriodsToIgnore);
        }
    }
}
