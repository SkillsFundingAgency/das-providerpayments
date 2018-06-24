using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.Extensions;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.SetupAttributes;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.DomainTests.LearnerTests
{
    [TestFixture]
    public class GivenAnAct1LearnerChangesPrice
    {
        private List<DatalockOutputEntity> _datalocks;
        private List<RawEarning> _earnings;
        private List<RawEarningForMathsOrEnglish> _mathsAndEnglishEarnings;
        private List<RequiredPaymentEntity> _pastPayments;
        private List<Commitment> _commitments;
        private List<DatalockValidationError> _datalockValidationErrors;

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
            _datalocks = earningsDictionary["Datalocks"] as List<DatalockOutputEntity>;
            _earnings = earningsDictionary["Earnings"] as List<RawEarning>;
            _mathsAndEnglishEarnings = earningsDictionary["MathsAndEnglishEarnings"] as List<RawEarningForMathsOrEnglish>;
            _pastPayments = earningsDictionary["PastPayments"] as List<RequiredPaymentEntity>;
            _commitments = earningsDictionary["Commitments"] as List<Commitment>;
            _datalockValidationErrors = earningsDictionary["DatalockValidationErrors"] as List<DatalockValidationError>;
        }

        [Theory, PaymentsDueAutoData]
        [SetupMatchingEarningsAndPastPayments(1, onProgAmount:100)]
        public void ThereArePaymentsForR01(DatalockValidationService commitmentMatcher)
        {
            var datalockOutput = commitmentMatcher.ProcessDatalocks(_datalocks, _datalockValidationErrors, _commitments);

            var datalock = new IDetermineWhichEarningsShouldBePaid();
            var datalockResult = datalock.ValidatePriceEpisodes(datalockOutput,
                _earnings.Take(1).ToList(), _mathsAndEnglishEarnings, new DateTime(2017, 08, 01));

            var sut = new PaymentsDueCalculationService(datalockResult.Earnings, datalockResult.PeriodsToIgnore, _pastPayments.Take(0).ToList());
            var actual = sut.Calculate();

            var expected = _earnings.Skip(0).Take(1).TotalAmount();
            actual.Sum(x => x.AmountDue).Should().Be(expected);
        }

        [Theory, PaymentsDueAutoData]
        [SetupMatchingEarningsAndPastPayments(1, onProgAmount:100)]
        public void ThereArePaymentsForR02WhichIncludeTheMissingAmountFromR01(DatalockValidationService datalockValidator)
        {
            _earnings.ForEach(x => x.TransactionType01 = 300);

            var datalockOutput = datalockValidator.ProcessDatalocks(_datalocks, _datalockValidationErrors, _commitments);
            
            var datalock = new IDetermineWhichEarningsShouldBePaid();
            var datalockResult = datalock.ValidatePriceEpisodes(datalockOutput,
                _earnings.Take(2).ToList(), _mathsAndEnglishEarnings, new DateTime(2017, 08, 01));

            var sut = new PaymentsDueCalculationService(datalockResult.Earnings, datalockResult.PeriodsToIgnore, _pastPayments.Take(1).ToList());
            var actual = sut.Calculate();

            var expected = 500; 
            actual.Sum(x => x.AmountDue).Should().Be(expected);
        }

        [Theory, PaymentsDueAutoData]
        [SetupMatchingEarningsAndPastPayments(1, onProgAmount:100)]
        public void ThereArePaymentsForR02WhichIncludeTheRefundAmountFromR01(
            DatalockValidationService datalockValidator)
        {
            _earnings.ForEach(x => x.TransactionType01 = 75);

            var datalockOutput = datalockValidator.ProcessDatalocks(_datalocks, _datalockValidationErrors, _commitments);

            var datalock = new IDetermineWhichEarningsShouldBePaid();
            var datalockResult = datalock.ValidatePriceEpisodes(datalockOutput,
                _earnings.Take(2).ToList(), _mathsAndEnglishEarnings, new DateTime(2017, 08, 01));

            var sut = new PaymentsDueCalculationService(datalockResult.Earnings, datalockResult.PeriodsToIgnore, _pastPayments.Take(1).ToList());
            var actual = sut.Calculate();

            var expected = 50; 
            actual.Sum(x => x.AmountDue).Should().Be(expected);
        }

        [Theory, PaymentsDueAutoData]
        [SetupMatchingEarningsAndPastPayments(1, onProgAmount:100)]
        public void ThereIsARefundPaymentsForR02BecauseTheBigPriceReductionFromR01(
            DatalockValidationService datalockValidator)
        {
            _earnings.ForEach(x => x.TransactionType01 = 10);

            var datalockOutput = datalockValidator.ProcessDatalocks(_datalocks, _datalockValidationErrors, _commitments);

            var datalock = new IDetermineWhichEarningsShouldBePaid();
            var datalockResult = datalock.ValidatePriceEpisodes(datalockOutput,
                _earnings.Take(2).ToList(), _mathsAndEnglishEarnings, new DateTime(2017, 08, 01));
              
            var sut = new PaymentsDueCalculationService(datalockResult.Earnings, datalockResult.PeriodsToIgnore, _pastPayments.Take(1).ToList());
            var actual = sut.Calculate();

            var expected = -80;
            actual.Sum(x => x.AmountDue).Should().Be(expected);
        }
    }
}
