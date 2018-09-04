using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.Helpers;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.SetupAttributes;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.DomainTests.CompletionPayments
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
            _earnings[11].TransactionType02 = 1000;

            _datalockOutput = earningsDictionary["DatalockOutput"] as List<DatalockOutput>;
        }

        [Test, PaymentsDueAutoData]
        [SetupMatchingEarningsAndPastPayments(ApprenticeshipContractType.NonLevy)]
        public void IfEarningsAreAct2AndEvidenceIsCanPayThenPayCompletionPayment(
            DetermineWhichEarningsShouldBePaidService sut)
        {
            var actual = sut.DeterminePayableEarnings(
                _datalockOutput,
                _earnings,
                new List<RawEarningForMathsOrEnglish>(), CompletionPaymentsEvidenceHelper.CreateCanPayEvidence());

            var completionPayments = actual.PayableEarnings.Where(x=>x.TransactionType == 2);
            completionPayments.Should().HaveCount(1);
            completionPayments.First().AmountDue.Should().Be(1000);
        }

        [Test, PaymentsDueAutoData]
        [SetupMatchingEarningsAndPastPayments(ApprenticeshipContractType.Levy)]
        public void IfEarningsAreAct1AndEvidenceIsCanPayThenPayCompletionPayment(
            DetermineWhichEarningsShouldBePaidService sut)
        {
            var actual = sut.DeterminePayableEarnings(
                _datalockOutput,
                _earnings,
                new List<RawEarningForMathsOrEnglish>(), CompletionPaymentsEvidenceHelper.CreateCanPayEvidence());

            var completionPayments = actual.PayableEarnings.Where(x => x.TransactionType == 2);
            completionPayments.Should().HaveCount(1);
            completionPayments.First().AmountDue.Should().Be(1000);
        }

        [Test, PaymentsDueAutoData]
        [SetupMatchingEarningsAndPastPayments(ApprenticeshipContractType.Levy)]
        public void IfEarningsAreAct1AndEvidenceIsNotEnoughPaidHoldbackCompletionPayment(
            DetermineWhichEarningsShouldBePaidService sut)
        {
            var actual = sut.DeterminePayableEarnings(
                _datalockOutput,
                _earnings,
                new List<RawEarningForMathsOrEnglish>(),
                new CompletionPaymentEvidence(120, CompletionPaymentEvidenceState.Checkable, 130));

            var completionPayments = actual.PayableEarnings.Where(x => x.TransactionType == 2);
            completionPayments.Should().HaveCount(0);
        }

        [Test, PaymentsDueAutoData]
        [SetupMatchingEarningsAndPastPayments(ApprenticeshipContractType.NonLevy)]
        public void IfEarningsAreAct2AndEvidenceIsNotEnoughPaidHoldbackCompletionPayment(
            DetermineWhichEarningsShouldBePaidService sut)
        {
            var actual = sut.DeterminePayableEarnings(
                _datalockOutput,
                _earnings,
                new List<RawEarningForMathsOrEnglish>(),
                new CompletionPaymentEvidence(129, CompletionPaymentEvidenceState.Checkable, 130));

            var completionPayments = actual.PayableEarnings.Where(x => x.TransactionType == 2);
            completionPayments.Should().HaveCount(0);
        }

        [Test]
        [PaymentsDueInlineAutoData(CompletionPaymentEvidenceState.ExemptRedundancy)]
        [PaymentsDueInlineAutoData(CompletionPaymentEvidenceState.ExemptOwnDelivery)]
        [PaymentsDueInlineAutoData(CompletionPaymentEvidenceState.ExemptOtherReason)]
        [SetupMatchingEarningsAndPastPayments(ApprenticeshipContractType.Levy)]
        public void PayCompletionPaymentAsLearnerIsExempt(CompletionPaymentEvidenceState state,
            DetermineWhichEarningsShouldBePaidService sut)
        {
            var actual = sut.DeterminePayableEarnings(
                _datalockOutput,
                _earnings,
                new List<RawEarningForMathsOrEnglish>(), 
                new CompletionPaymentEvidence(120, state, 130));

            var completionPayments = actual.PayableEarnings.Where(x => x.TransactionType == 2);
            completionPayments.Should().HaveCount(1);
            completionPayments.First().AmountDue.Should().Be(1000);
        }

        [Test, PaymentsDueAutoData]
        [SetupMatchingEarningsAndPastPayments(ApprenticeshipContractType.NonLevy)]
        public void IfEarningsAreAct2AndEvidenceIsErrorOnIlrHoldbackCompletionPayment(
            DetermineWhichEarningsShouldBePaidService sut)
        {
            var actual = sut.DeterminePayableEarnings(
                _datalockOutput,
                _earnings,
                new List<RawEarningForMathsOrEnglish>(),
                new CompletionPaymentEvidence(0, CompletionPaymentEvidenceState.ErrorOnIlr, 0));

            var completionPayments = actual.PayableEarnings.Where(x => x.TransactionType == 2);
            completionPayments.Should().HaveCount(0);
        }

    }
}
