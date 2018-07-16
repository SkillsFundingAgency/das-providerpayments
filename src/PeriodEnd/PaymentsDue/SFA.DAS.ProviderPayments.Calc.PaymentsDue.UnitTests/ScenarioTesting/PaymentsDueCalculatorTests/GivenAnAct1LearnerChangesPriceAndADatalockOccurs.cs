﻿using System;
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
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.Utilities.SetupAttributes;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.UnitTests.ScenarioTesting.PaymentsDueCalculatorTests
{
    [TestFixture]
    public class GivenAnAct1LearnerChangesPriceAndADatalockOccurs
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
        [SetupMatchingEarningsAndPastPayments(ApprenticeshipContractType.Levy, onProgAmount: 100, mathsEnglishAmount: 0)]
        public void WithPassingDatalock_ThereArePaymentsForR01(
            [Frozen] Mock<ICollectionPeriodRepository> collectionPeriodRepository,
            DetermineWhichEarningsShouldBePaidService datalock,
            PaymentsDueCalculationService sut,
            DatalockValidationService datalockValidator)
        {
            var datalockOutput = datalockValidator.GetSuccessfulDatalocks(_datalocks, _datalockValidationErrors, _commitments);

            collectionPeriodRepository.Setup(x => x.GetCurrentCollectionPeriod())
                .Returns(new CollectionPeriodEntity { AcademicYear = "1718" });

            var datalockResult = datalock.DeterminePayableEarnings(datalockOutput,
                _earnings.Take(1).ToList(), _mathsAndEnglishEarnings);

            var actual = sut.Calculate(datalockResult.PayableEarnings, datalockResult.PeriodsToIgnore, _pastPayments.Take(0).ToList());

            var expected = _earnings.Skip(0).Take(1).TotalAmount();
            actual.Sum(x => x.AmountDue).Should().Be(expected);
        }

        [Test, PaymentsDueAutoData]
        [SetupMatchingEarningsAndPastPayments(ApprenticeshipContractType.Levy, onProgAmount: 100, datalockSuccess: false, mathsEnglishAmount: 0)]
        public void WithFailingDatalockAndAnIncreaseInPrice_ThereAreNoPaymentsForR02(
            [Frozen] Mock<ICollectionPeriodRepository> collectionPeriodRepository,
            DetermineWhichEarningsShouldBePaidService datalock,
            PaymentsDueCalculationService sut,
            DatalockValidationService datalockValidator)
        {
            _earnings.ForEach(x => x.TransactionType01 = 300);

            var datalockOutput = datalockValidator.GetSuccessfulDatalocks(_datalocks, _datalockValidationErrors, _commitments);

            collectionPeriodRepository.Setup(x => x.GetCurrentCollectionPeriod())
                .Returns(new CollectionPeriodEntity { AcademicYear = "1718" });

            var datalockResult = datalock.DeterminePayableEarnings(datalockOutput,
                _earnings.Take(2).ToList(), _mathsAndEnglishEarnings);

            var actual = sut.Calculate(datalockResult.PayableEarnings, datalockResult.PeriodsToIgnore, _pastPayments.Take(1).ToList());

            var expected = 0;
            actual.Sum(x => x.AmountDue).Should().Be(expected);
        }

        [Test, PaymentsDueAutoData]
        [SetupMatchingEarningsAndPastPayments(ApprenticeshipContractType.Levy, onProgAmount: 100, datalockSuccess: false, mathsEnglishAmount: 0)]
        public void WithFailingDatalockAndADecreaseInPrice_ThereAreNoPaymentsForR02(
            [Frozen] Mock<ICollectionPeriodRepository> collectionPeriodRepository,
            DetermineWhichEarningsShouldBePaidService datalock,
            PaymentsDueCalculationService sut,
            DatalockValidationService datalockValidator)
        {
            _earnings.ForEach(x => x.TransactionType01 = 50);

            var datalockOutput = datalockValidator.GetSuccessfulDatalocks(_datalocks, _datalockValidationErrors, _commitments);

            collectionPeriodRepository.Setup(x => x.GetCurrentCollectionPeriod())
                .Returns(new CollectionPeriodEntity { AcademicYear = "1718" });

            var datalockResult = datalock.DeterminePayableEarnings(datalockOutput,
                _earnings.Take(2).ToList(), _mathsAndEnglishEarnings);

            var actual = sut.Calculate(datalockResult.PayableEarnings, datalockResult.PeriodsToIgnore, _pastPayments.Take(1).ToList());

            var expected = 0;
            actual.Sum(x => x.AmountDue).Should().Be(expected);
        }

        [Test, PaymentsDueAutoData]
        [SetupMatchingEarningsAndPastPayments(ApprenticeshipContractType.Levy, onProgAmount: 100, datalockSuccess: false, mathsEnglishAmount: 0)]
        public void WithFailingDatalockAndNoChangeInPrice_ThereAreNoPaymentsForR02(
            [Frozen] Mock<ICollectionPeriodRepository> collectionPeriodRepository,
            DetermineWhichEarningsShouldBePaidService datalock,
            PaymentsDueCalculationService sut,
            DatalockValidationService datalockValidator)
        {
            var datalockOutput = datalockValidator.GetSuccessfulDatalocks(_datalocks, _datalockValidationErrors, _commitments);

            collectionPeriodRepository.Setup(x => x.GetCurrentCollectionPeriod())
                .Returns(new CollectionPeriodEntity { AcademicYear = "1718" });

            var datalockResult = datalock.DeterminePayableEarnings(datalockOutput,
                _earnings.Take(2).ToList(), _mathsAndEnglishEarnings);

            var actual = sut.Calculate(datalockResult.PayableEarnings, datalockResult.PeriodsToIgnore, _pastPayments.Take(1).ToList());

            var expected = 0;
            actual.Sum(x => x.AmountDue).Should().Be(expected);
        }

        [Test, PaymentsDueAutoData]
        [SetupMatchingEarningsAndPastPayments(ApprenticeshipContractType.Levy, onProgAmount: 100, mathsEnglishAmount: 0)]
        public void WithPassingDatalockInR03ButHavingMadeNoPaymentsInR02AndAPriceIncrease_ThereArePaymentsForR03(
            [Frozen] Mock<ICollectionPeriodRepository> collectionPeriodRepository,
            DetermineWhichEarningsShouldBePaidService datalock,
            PaymentsDueCalculationService sut,
            DatalockValidationService datalockValidator)
        {
            _earnings.ForEach(x => x.TransactionType01 = 150);

            var datalockOutput = datalockValidator.GetSuccessfulDatalocks(_datalocks, _datalockValidationErrors, _commitments);

            collectionPeriodRepository.Setup(x => x.GetCurrentCollectionPeriod())
                .Returns(new CollectionPeriodEntity { AcademicYear = "1718" });

            var datalockResult = datalock.DeterminePayableEarnings(datalockOutput,
                _earnings.Take(3).ToList(), _mathsAndEnglishEarnings);

            var actual = sut.Calculate(datalockResult.PayableEarnings, datalockResult.PeriodsToIgnore, _pastPayments.Take(1).ToList());

            var expected = 350;
            actual.Sum(x => x.AmountDue).Should().Be(expected);
        }
    }
}
