using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.Helpers;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.SetupAttributes;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.DomainTests.DetermineWhichEarningsShouldBePaidTests.MathsAndEnglish
{
    [TestFixture]
    public class GivenADetermineWhichEarningsShouldBePaidService
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

        [Test, PaymentsDueAutoData]
        [SetupMatchingEarningsAndPastPayments(ApprenticeshipContractType.Levy)]
        public void MathsEnglishWithNoPayableOnprogNotPaid(
            DetermineWhichEarningsShouldBePaidService sut)
        {
            _earnings.Clear();
            
            var actual = sut.DeterminePayableEarnings(
                new List<DatalockOutput>(), 
                _earnings,
                _mathsAndEnglishEarnings, 
                CompletionPaymentsEvidenceHelper.CreateCanPayEvidence());

            actual.PayableEarnings.Should().BeEmpty();
        }

        [Test, PaymentsDueAutoData]
        [SetupMatchingEarningsAndPastPayments(ApprenticeshipContractType.NonLevy)]
        public void MathsEnglishWithNoPayableOnprogNotPaidForAct2(
            DetermineWhichEarningsShouldBePaidService sut)
        {
            _earnings.Clear();

            var actual = sut.DeterminePayableEarnings(
                new List<DatalockOutput>(),
                new List<RawEarning>(),
                _mathsAndEnglishEarnings, 
                CompletionPaymentsEvidenceHelper.CreateCanPayEvidence());

            actual.PayableEarnings.Should().BeEmpty();
        }

        [Test, PaymentsDueAutoData]
        [SetupMatchingEarningsAndPastPayments(ApprenticeshipContractType.Levy)]
        public void MathsEnglishWithPayableOnProgPaid(
            DetermineWhichEarningsShouldBePaidService sut)
        {
            var actual = sut.DeterminePayableEarnings(
                _datalockOutput,
                _earnings,
                _mathsAndEnglishEarnings, 
                CompletionPaymentsEvidenceHelper.CreateCanPayEvidence());

            actual.PayableEarnings.Should().HaveCount(24); // M/E as well as onprog * 12
        }

        [Test, PaymentsDueAutoData]
        [SetupMatchingEarningsAndPastPayments(ApprenticeshipContractType.NonLevy)]
        public void MathsEnglishAct2WithNoOtherEarningsArePaid(
            DetermineWhichEarningsShouldBePaidService sut)
        {
            var blankEarning = _earnings[0];
            blankEarning.TransactionType01 = 0;

            var actual = sut.DeterminePayableEarnings(
                new List<DatalockOutput>(),
                new List<RawEarning> {blankEarning},
                _mathsAndEnglishEarnings, 
                CompletionPaymentsEvidenceHelper.CreateCanPayEvidence());

            actual.PayableEarnings.Should().HaveCount(12);
        }

        [Test, PaymentsDueAutoData]
        [SetupMatchingEarningsAndPastPayments(ApprenticeshipContractType.Levy)]
        public void MathsEnglishAct1WithNoOtherEarningsArePaidWhenMatchingDatalock(
            DetermineWhichEarningsShouldBePaidService sut)
        {
            var blankEarning = _earnings[0];
            blankEarning.TransactionType01 = 0;

            var actual = sut.DeterminePayableEarnings(
                _datalockOutput,
                new List<RawEarning>{blankEarning}, 
                _mathsAndEnglishEarnings, 
                CompletionPaymentsEvidenceHelper.CreateCanPayEvidence());

            actual.PayableEarnings.Should().HaveCount(12);
        }

        [Test, PaymentsDueAutoData]
        [SetupMatchingEarningsAndPastPayments(ApprenticeshipContractType.NonLevy)]
        public void MathsEnglishAct2WithNoOtherEarningsAreNotPaidWhenNoMatchingDatalock(
            DetermineWhichEarningsShouldBePaidService sut)
        {
            var actual = sut.DeterminePayableEarnings(
                new List<DatalockOutput>(), 
                new List<RawEarning>(),
                _mathsAndEnglishEarnings, 
                CompletionPaymentsEvidenceHelper.CreateCanPayEvidence());

            actual.PayableEarnings.Should().HaveCount(0);
        }
    }
}
