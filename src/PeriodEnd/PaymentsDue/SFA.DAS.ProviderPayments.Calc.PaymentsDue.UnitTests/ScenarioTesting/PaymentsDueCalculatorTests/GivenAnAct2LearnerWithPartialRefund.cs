using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.Extensions;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.Helpers;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.SetupAttributes;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.ScenarioTesting.PaymentsDueCalculatorTests
{
    [TestFixture]
    public class GivenAnAct2LearnerWithPartialRefund
    {
        [TestFixture]
        public class PriceChangeFrom500To750InR03
        {
            private List<DatalockOutputEntity> _datalocks;
            private List<RawEarning> _earnings;
            private List<RawEarningForMathsOrEnglish> _mathsAndEnglishEarnings;
            private List<RequiredPayment> _pastPayments;
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
                _pastPayments = earningsDictionary["PastPayments"] as List<RequiredPayment>;
                _commitments = earningsDictionary["Commitments"] as List<Commitment>;
                _datalockValidationErrors = earningsDictionary["DatalockValidationErrors"] as List<DatalockValidationError>;

            }

            [Test, PaymentsDueAutoData]
            [SetupMatchingEarningsAndPastPayments(ApprenticeshipContractType.NonLevy, onProgAmount: 500, mathsEnglishAmount: 0)]
            public void ThereArePaymentsForR01Of500(
                [Frozen] Mock<ICollectionPeriodRepository> collectionPeriodRepository,
                DetermineWhichEarningsShouldBePaidService datalock,
                PaymentsDueCalculationService sut,
                DatalockValidationService datalockValidator)
            {
                var datalockOutput = datalockValidator.GetSuccessfulDatalocks(_datalocks, _datalockValidationErrors, _commitments);

                collectionPeriodRepository.Setup(x => x.GetCurrentCollectionPeriod())
                    .Returns(new CollectionPeriodEntity { AcademicYear = "1718" });

                var datalockResult = datalock.DeterminePayableEarnings(datalockOutput,
                    _earnings.Take(1).ToList(), _mathsAndEnglishEarnings, CompletionPaymentsEvidenceHelper.CreateCanPayEvidence());

                var actual = sut.Calculate(datalockResult.PayableEarnings, datalockResult.PeriodsToIgnore, _pastPayments.Take(0).ToList());

                var expected = 500;
                actual.Sum(x => x.AmountDue).Should().Be(expected);
            }

            [Test, PaymentsDueAutoData]
            [SetupMatchingEarningsAndPastPayments(ApprenticeshipContractType.NonLevy, onProgAmount: 500, mathsEnglishAmount: 0)]
            public void ThereArePaymentsForR02Of500(
                [Frozen] Mock<ICollectionPeriodRepository> collectionPeriodRepository,
                DetermineWhichEarningsShouldBePaidService datalock,
                PaymentsDueCalculationService sut,
                DatalockValidationService datalockValidator)
            {
                var datalockOutput = datalockValidator.GetSuccessfulDatalocks(_datalocks, _datalockValidationErrors, _commitments);

                collectionPeriodRepository.Setup(x => x.GetCurrentCollectionPeriod())
                    .Returns(new CollectionPeriodEntity { AcademicYear = "1718" });

                var datalockResult = datalock.DeterminePayableEarnings(datalockOutput,
                    _earnings.Take(2).ToList(), _mathsAndEnglishEarnings, CompletionPaymentsEvidenceHelper.CreateCanPayEvidence());

                var actual = sut.Calculate(datalockResult.PayableEarnings, datalockResult.PeriodsToIgnore, _pastPayments.Take(1).ToList());

                var expected = 500;
                actual.Sum(x => x.AmountDue).Should().Be(expected);
            }

            [Test, PaymentsDueAutoData]
            [SetupMatchingEarningsAndPastPayments(ApprenticeshipContractType.NonLevy, onProgAmount: 500, mathsEnglishAmount: 0)]
            public void WithAPriceIncreaseTo750_ThereAreCorrectPaymentsForR03(
                [Frozen] Mock<ICollectionPeriodRepository> collectionPeriodRepository,
                DetermineWhichEarningsShouldBePaidService datalock,
                PaymentsDueCalculationService sut,
                DatalockValidationService datalockValidator)
            {
                _earnings[0].TransactionType01 = 750;
                _earnings[1].TransactionType01 = 750;
                _earnings[2].TransactionType01 = 750;

                var datalockOutput = datalockValidator.GetSuccessfulDatalocks(_datalocks, _datalockValidationErrors, _commitments);

                collectionPeriodRepository.Setup(x => x.GetCurrentCollectionPeriod())
                    .Returns(new CollectionPeriodEntity { AcademicYear = "1718" });

                var datalockResult = datalock.DeterminePayableEarnings(datalockOutput,
                    _earnings.Take(3).ToList(), _mathsAndEnglishEarnings, CompletionPaymentsEvidenceHelper.CreateCanPayEvidence());

                var actual = sut.Calculate(datalockResult.PayableEarnings, datalockResult.PeriodsToIgnore, _pastPayments.Take(2).ToList());

                var expected = 1250;
                actual.Sum(x => x.AmountDue).Should().Be(expected);

                actual.Should().HaveCount(3);
                actual[0].AmountDue.Should().Be(250);
                actual[1].AmountDue.Should().Be(250);
                actual[2].AmountDue.Should().Be(750);
            }

            [Test, PaymentsDueAutoData]
            [SetupMatchingEarningsAndPastPayments(ApprenticeshipContractType.NonLevy, onProgAmount: 500, mathsEnglishAmount: 0)]
            public void ThereArePaymentsForR04Of750(
                [Frozen] Mock<ICollectionPeriodRepository> collectionPeriodRepository,
                DetermineWhichEarningsShouldBePaidService datalock,
                PaymentsDueCalculationService sut,
                DatalockValidationService datalockValidator)
            {
                _earnings[0].TransactionType01 = 750;
                _earnings[1].TransactionType01 = 750;
                _earnings[2].TransactionType01 = 750;
                _earnings[3].TransactionType01 = 750;

                _pastPayments[0].AmountDue = 500;
                _pastPayments[1].AmountDue = 500;
                _pastPayments[2].AmountDue = 750;
                _pastPayments[3].AmountDue = 250;
                _pastPayments[4].AmountDue = 250;

                _pastPayments[0].CopySignificantPaymentPropertiesTo(_pastPayments[3]);
                _pastPayments[1].CopySignificantPaymentPropertiesTo(_pastPayments[4]);

                var datalockOutput = datalockValidator.GetSuccessfulDatalocks(_datalocks, _datalockValidationErrors, _commitments);

                collectionPeriodRepository.Setup(x => x.GetCurrentCollectionPeriod())
                    .Returns(new CollectionPeriodEntity { AcademicYear = "1718" });

                var datalockResult = datalock.DeterminePayableEarnings(datalockOutput,
                    _earnings.Take(4).ToList(), _mathsAndEnglishEarnings, CompletionPaymentsEvidenceHelper.CreateCanPayEvidence());

                var actual = sut.Calculate(datalockResult.PayableEarnings, datalockResult.PeriodsToIgnore, _pastPayments.Take(5).ToList());

                var expected = 750;
                actual.Sum(x => x.AmountDue).Should().Be(expected);
            }
        }
    }
}
