using System;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Tools;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.FinishedOnTime
{
    public class WhenALearnerHasBeenPaidIncorrectlyAfterContractTypeChanges
    {

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();
        }

        [Test]
        [Ignore("This is not currently a scenario that seems possible")]
        public void ThenPaymentsFromPreviousPeriodsWhereContractTypeChangedFromNonDasToDasWillBeCorrectlyRefunded()
        {
            // Arrange
            const int ukprn = 863145;
            const long commitmentId = 1L;
            var startDate = new DateTime(2017, 8, 12);
            var plannedEndDate = new DateTime(2018, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);
            var uln = new Random().Next(10000000, int.MaxValue);
            const int currentPeriod = 9;
            const decimal amountDue = 1000M;

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber, startDate: startDate, endDate: plannedEndDate, uln: uln);

            TestDataHelper.AddPaymentForCommitment(commitmentId, 8, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, collectionPeriodYear: 2017, collectionperiodMonth: 8);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 8, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, apprenticeshipContractType: 2, collectionPeriodYear: 2017, collectionperiodMonth: 8, useLevy: false);

            TestDataHelper.AddPaymentForCommitment(commitmentId, 9, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, collectionPeriodYear: 2017, collectionperiodMonth: 9, collectionPeriod: 2);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 9, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, apprenticeshipContractType: 2, collectionPeriodYear: 2017, collectionperiodMonth: 9, collectionPeriod: 2, useLevy: false);

            TestDataHelper.AddPaymentForCommitment(commitmentId, 10, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, collectionPeriodYear: 2017, collectionperiodMonth: 10, collectionPeriod: 3);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 10, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, apprenticeshipContractType: 2, collectionPeriodYear: 2017, collectionperiodMonth: 10, collectionPeriod: 3, useLevy: false);

            TestDataHelper.AddPaymentForCommitment(commitmentId, 11, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, collectionPeriodYear: 2017, collectionperiodMonth: 11, collectionPeriod: 4);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 11, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, apprenticeshipContractType: 2, collectionPeriodYear: 2017, collectionperiodMonth: 11, collectionPeriod: 4, useLevy: false);

            TestDataHelper.AddPaymentForCommitment(commitmentId, 12, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, collectionPeriodYear: 2017, collectionperiodMonth: 12, collectionPeriod: 5);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 12, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, apprenticeshipContractType: 2, collectionPeriodYear: 2017, collectionperiodMonth: 12, collectionPeriod: 5, useLevy: false);

            TestDataHelper.AddPaymentForCommitment(commitmentId, 1, 2018, (int)TransactionType.Learning, amountDue, learnerRefNumber, collectionPeriodYear: 2018, collectionperiodMonth: 1, collectionPeriod: 6);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 2, 2018, (int)TransactionType.Learning, amountDue, learnerRefNumber, collectionPeriodYear: 2018, collectionperiodMonth: 2, collectionPeriod: 7);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 3, 2018, (int)TransactionType.Learning, amountDue, learnerRefNumber, collectionPeriodYear: 2018, collectionperiodMonth: 3, collectionPeriod: 8);

            TestDataHelper.SetOpenCollection(currentPeriod, "1718");

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber, currentPeriod: currentPeriod);

            TestDataHelper.DeleteLearningDeliveryFAM(ukprn, learnerRefNumber, "1", "ACT");
            TestDataHelper.AddLearningDeliveryFam(ukprn, learnerRefNumber, "1", "ACT", 2, new DateTime(2017, 8, 1), new DateTime(2017, 12, 31));
            TestDataHelper.AddLearningDeliveryFam(ukprn, learnerRefNumber, "1", "ACT", 1, new DateTime(2018, 1, 1), null);

            TestDataHelper.AddPaymentHistory();

            TestDataHelper.CopyReferenceData("1718");

            //Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);

            for (var month = 8; month <= 12; month++)
            {
                var currentMonth = month;
                var currentMonthPayments = duePayments.Where(p => p.DeliveryMonth == currentMonth && p.DeliveryYear == 2017).ToList();

                Assert.AreEqual(1, currentMonthPayments.Count);

                var currentMonthPayment = currentMonthPayments.First();

                Assert.AreEqual(-amountDue, currentMonthPayment.AmountDue);

                Assert.AreEqual(1, currentMonthPayment.ApprenticeshipContractType);
            }

            var lastMonthPayment = duePayments.FirstOrDefault(x => x.DeliveryMonth == 4 && x.DeliveryYear == 2018);

            Assert.IsNotNull(lastMonthPayment);

            Assert.AreEqual(amountDue, lastMonthPayment.AmountDue);

            Assert.AreEqual(1, lastMonthPayment.ApprenticeshipContractType);
        }

        [Test]
        public void ThenWhenTheContractTypeIsRetrospectivelyChangedFromNonDasToDasFromTheFirstPeriodThePreviousPaymentsGetRefunded()
        {
            // Arrange
            const int ukprn = 863145;
            const long commitmentId = 1L;
            var startDate = new DateTime(2017, 8, 12);
            var plannedEndDate = new DateTime(2018, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);
            var uln = new Random().Next(10000000, int.MaxValue);
            const int currentPeriod = 9;
            const decimal amountDue = 1000M;

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber, startDate: startDate, endDate: plannedEndDate, uln: uln);

            TestDataHelper.AddPaymentForCommitment(commitmentId, 8, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, apprenticeshipContractType: 2, collectionPeriodYear: 2017, collectionperiodMonth: 8, collectionPeriod: 1, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 9, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, apprenticeshipContractType: 2, collectionPeriodYear: 2017, collectionperiodMonth: 9, collectionPeriod: 2, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 10, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, apprenticeshipContractType: 2, collectionPeriodYear: 2017, collectionperiodMonth: 10, collectionPeriod: 3, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 11, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, apprenticeshipContractType: 2, collectionPeriodYear: 2017, collectionperiodMonth: 11, collectionPeriod: 4, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 12, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, apprenticeshipContractType: 2, collectionPeriodYear: 2017, collectionperiodMonth: 12, collectionPeriod: 5, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 1, 2018, (int)TransactionType.Learning, amountDue, learnerRefNumber, apprenticeshipContractType: 2, collectionPeriodYear: 2018, collectionperiodMonth: 1, collectionPeriod: 6, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 2, 2018, (int)TransactionType.Learning, amountDue, learnerRefNumber, apprenticeshipContractType: 2, collectionPeriodYear: 2018, collectionperiodMonth: 2, collectionPeriod: 7, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 3, 2018, (int)TransactionType.Learning, amountDue, learnerRefNumber, apprenticeshipContractType: 2, collectionPeriodYear: 2018, collectionperiodMonth: 3, collectionPeriod: 8, useLevy: false);

            TestDataHelper.SetOpenCollection(currentPeriod, "1718");

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber, currentPeriod: currentPeriod);

            TestDataHelper.DeleteLearningDeliveryFAM(ukprn, learnerRefNumber, "1", "ACT");
            TestDataHelper.AddLearningDeliveryFam(ukprn, learnerRefNumber, "1", "ACT", 2, new DateTime(2017, 8, 1), new DateTime(2017, 8, 1));
            TestDataHelper.AddLearningDeliveryFam(ukprn, learnerRefNumber, "1", "ACT", 1, new DateTime(2017, 8, 1), null);

            TestDataHelper.AddPaymentHistory();

            TestDataHelper.CopyReferenceData("1718");

            //Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);

            Assert.AreEqual(17, duePayments.Length);

            var act1Payments = duePayments.Where(a => a.ApprenticeshipContractType == 1).ToList();

            Assert.AreEqual(9, act1Payments.Count);

            act1Payments.ForEach(a => Assert.AreEqual(amountDue, a.AmountDue));

            var act2Payments = duePayments.Where(a => a.ApprenticeshipContractType == 2).ToList();

            Assert.AreEqual(8, act2Payments.Count);

            act2Payments.ForEach(a => Assert.AreEqual(-amountDue, a.AmountDue));
        }

        [Test]
        public void ThenWhenTheContractTypeIsChangedFromNonDasToDasFromTheCurrentPeriodTheNoPreviousPaymentsGetRefunded()
        {
            // Arrange
            const int ukprn = 863145;
            const long commitmentId = 1L;
            var startDate = new DateTime(2017, 8, 12);
            var plannedEndDate = new DateTime(2018, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);
            var uln = new Random().Next(10000000, int.MaxValue);
            const int currentPeriod = 9;
            const decimal amountDue = 1000M;

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber, startDate: startDate, endDate: plannedEndDate, uln: uln);

            TestDataHelper.AddPaymentForCommitment(commitmentId, 8, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, apprenticeshipContractType: 2, collectionPeriodYear: 2017, collectionperiodMonth: 8, collectionPeriod: 1, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 9, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, apprenticeshipContractType: 2, collectionPeriodYear: 2017, collectionperiodMonth: 9, collectionPeriod: 2, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 10, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, apprenticeshipContractType: 2, collectionPeriodYear: 2017, collectionperiodMonth: 10, collectionPeriod: 3, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 11, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, apprenticeshipContractType: 2, collectionPeriodYear: 2017, collectionperiodMonth: 11, collectionPeriod: 4, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 12, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, apprenticeshipContractType: 2, collectionPeriodYear: 2017, collectionperiodMonth: 12, collectionPeriod: 5, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 1, 2018, (int)TransactionType.Learning, amountDue, learnerRefNumber, apprenticeshipContractType: 2, collectionPeriodYear: 2018, collectionperiodMonth: 1, collectionPeriod: 6, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 2, 2018, (int)TransactionType.Learning, amountDue, learnerRefNumber, apprenticeshipContractType: 2, collectionPeriodYear: 2018, collectionperiodMonth: 2, collectionPeriod: 7, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 3, 2018, (int)TransactionType.Learning, amountDue, learnerRefNumber, apprenticeshipContractType: 2, collectionPeriodYear: 2018, collectionperiodMonth: 3, collectionPeriod: 8, useLevy: false);

            TestDataHelper.SetOpenCollection(currentPeriod, "1718");

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber, currentPeriod: currentPeriod);

            TestDataHelper.DeleteLearningDeliveryFAM(ukprn, learnerRefNumber, "1", "ACT");
            TestDataHelper.AddLearningDeliveryFam(ukprn, learnerRefNumber, "1", "ACT", 2, new DateTime(2017, 8, 1), new DateTime(2018, 3, 31));
            TestDataHelper.AddLearningDeliveryFam(ukprn, learnerRefNumber, "1", "ACT", 1, new DateTime(2018, 4, 1), null);

            TestDataHelper.AddPaymentHistory();

            TestDataHelper.CopyReferenceData("1718");

            //Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);

            Assert.AreEqual(1, duePayments.Length);

            var act1Payments = duePayments.Where(a => a.ApprenticeshipContractType == 1).ToList();

            Assert.AreEqual(1, act1Payments.Count);

            act1Payments.ForEach(a => Assert.AreEqual(amountDue, a.AmountDue));
        }

        [Test]
        public void ThenWhenTheContractTypeWasChangedFromNonDasToDasFromAPreviousPeriodThenNoExtraRefundsOrPaymentsOccur()
        {
            // Arrange
            const int ukprn = 863145;
            const long commitmentId = 1L;
            var startDate = new DateTime(2017, 8, 12);
            var plannedEndDate = new DateTime(2018, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);
            var uln = new Random().Next(10000000, int.MaxValue);
            const int currentPeriod = 9;
            const decimal amountDue = 1000M;

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber, startDate: startDate, endDate: plannedEndDate, uln: uln);

            TestDataHelper.AddPaymentForCommitment(commitmentId, 8, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, apprenticeshipContractType: 2, collectionPeriodYear: 2017, collectionperiodMonth: 8, collectionPeriod: 1, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 9, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, apprenticeshipContractType: 2, collectionPeriodYear: 2017, collectionperiodMonth: 9, collectionPeriod: 2, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 10, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, apprenticeshipContractType: 2, collectionPeriodYear: 2017, collectionperiodMonth: 10, collectionPeriod: 3, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 11, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, apprenticeshipContractType: 2, collectionPeriodYear: 2017, collectionperiodMonth: 11, collectionPeriod: 4, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 12, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, apprenticeshipContractType: 2, collectionPeriodYear: 2017, collectionperiodMonth: 12, collectionPeriod: 5, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 1, 2018, (int)TransactionType.Learning, amountDue, learnerRefNumber, collectionPeriodYear: 2018, collectionperiodMonth: 1, collectionPeriod: 6, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 2, 2018, (int)TransactionType.Learning, amountDue, learnerRefNumber, collectionPeriodYear: 2018, collectionperiodMonth: 2, collectionPeriod: 7, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 3, 2018, (int)TransactionType.Learning, amountDue, learnerRefNumber, collectionPeriodYear: 2018, collectionperiodMonth: 3, collectionPeriod: 8, useLevy: false);

            TestDataHelper.SetOpenCollection(currentPeriod, "1718");

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber, currentPeriod: currentPeriod);

            TestDataHelper.DeleteLearningDeliveryFAM(ukprn, learnerRefNumber, "1", "ACT");
            TestDataHelper.AddLearningDeliveryFam(ukprn, learnerRefNumber, "1", "ACT", 2, new DateTime(2017, 8, 1), new DateTime(2017, 12, 31));
            TestDataHelper.AddLearningDeliveryFam(ukprn, learnerRefNumber, "1", "ACT", 1, new DateTime(2018, 1, 1), null);

            TestDataHelper.AddPaymentHistory();

            TestDataHelper.CopyReferenceData("1718");

            //Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);

            Assert.AreEqual(1, duePayments.Length);

            var act1Payments = duePayments.Where(a => a.ApprenticeshipContractType == 1).ToList();

            Assert.AreEqual(1, act1Payments.Count);

            act1Payments.ForEach(a => Assert.AreEqual(amountDue, a.AmountDue));
        }

        [Test]
        public void ThenWhenTheContractTypeIsRetrospectivelyChangedFromDasToNonDasFromTheFirstPeriodThePreviousPaymentsGetRefunded()
        {
            // Arrange
            const int ukprn = 863145;
            const long commitmentId = 1L;
            var startDate = new DateTime(2017, 8, 12);
            var plannedEndDate = new DateTime(2018, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);
            var uln = new Random().Next(10000000, int.MaxValue);
            const int currentPeriod = 9;
            const decimal amountDue = 1000M;

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber, startDate: startDate, endDate: plannedEndDate, uln: uln);

            TestDataHelper.AddPaymentForCommitment(commitmentId, 8, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, collectionPeriodYear: 2017, collectionperiodMonth: 8);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 9, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, collectionPeriodYear: 2017, collectionperiodMonth: 9, collectionPeriod: 2);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 10, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, collectionPeriodYear: 2017, collectionperiodMonth: 10, collectionPeriod: 3);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 11, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, collectionPeriodYear: 2017, collectionperiodMonth: 11, collectionPeriod: 4);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 12, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, collectionPeriodYear: 2017, collectionperiodMonth: 12, collectionPeriod: 5);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 1, 2018, (int)TransactionType.Learning, amountDue, learnerRefNumber, collectionPeriodYear: 2018, collectionperiodMonth: 1, collectionPeriod: 6);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 2, 2018, (int)TransactionType.Learning, amountDue, learnerRefNumber, collectionPeriodYear: 2018, collectionperiodMonth: 2, collectionPeriod: 7);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 3, 2018, (int)TransactionType.Learning, amountDue, learnerRefNumber, collectionPeriodYear: 2018, collectionperiodMonth: 3, collectionPeriod: 8);

            TestDataHelper.SetOpenCollection(currentPeriod, "1718");

            TestDataHelper.AddEarningForNonDas(ukprn, new DateTime(2017, 8, 1), plannedEndDate, 12000, learnerRefNumber);

            TestDataHelper.DeleteLearningDeliveryFAM(ukprn, learnerRefNumber, "1", "ACT");
            TestDataHelper.AddLearningDeliveryFam(ukprn, learnerRefNumber, "1", "ACT", 1, new DateTime(2017, 8, 1), new DateTime(2017, 8, 1));
            TestDataHelper.AddLearningDeliveryFam(ukprn, learnerRefNumber, "1", "ACT", 2, new DateTime(2017, 8, 1), null);

            TestDataHelper.AddPaymentHistory();

            TestDataHelper.CopyReferenceData("1718");

            //Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);

            Assert.AreEqual(17, duePayments.Length);

            var act1Payments = duePayments.Where(a => a.ApprenticeshipContractType == 1).ToList();

            Assert.AreEqual(8, act1Payments.Count);

            act1Payments.ForEach(a => Assert.AreEqual(-amountDue, a.AmountDue));

            var act2Payments = duePayments.Where(a => a.ApprenticeshipContractType == 2).ToList();

            Assert.AreEqual(9, act2Payments.Count);

            act2Payments.ForEach(a => Assert.AreEqual(amountDue * 0.8M, a.AmountDue));
        }

        [Test]
        public void ThenWhenTheContractTypeIsChangedFromDasToNonDasFromTheCurrentPeriodTheNoPreviousPaymentsGetRefunded()
        {
            // Arrange
            const int ukprn = 863145;
            const long commitmentId = 1L;
            var startDate = new DateTime(2017, 8, 12);
            var plannedEndDate = new DateTime(2018, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);
            var uln = new Random().Next(10000000, int.MaxValue);
            const int currentPeriod = 9;
            const decimal amountDue = 1000M;

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber, startDate: startDate, endDate: plannedEndDate, uln: uln);

            TestDataHelper.AddPaymentForCommitment(commitmentId, 8, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, collectionPeriodYear: 2017, collectionperiodMonth: 8, collectionPeriod: 1, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 9, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, collectionPeriodYear: 2017, collectionperiodMonth: 9, collectionPeriod: 2, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 10, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, collectionPeriodYear: 2017, collectionperiodMonth: 10, collectionPeriod: 3, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 11, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, collectionPeriodYear: 2017, collectionperiodMonth: 11, collectionPeriod: 4, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 12, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, collectionPeriodYear: 2017, collectionperiodMonth: 12, collectionPeriod: 5, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 1, 2018, (int)TransactionType.Learning, amountDue, learnerRefNumber, collectionPeriodYear: 2018, collectionperiodMonth: 1, collectionPeriod: 6, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 2, 2018, (int)TransactionType.Learning, amountDue, learnerRefNumber, collectionPeriodYear: 2018, collectionperiodMonth: 2, collectionPeriod: 7, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 3, 2018, (int)TransactionType.Learning, amountDue, learnerRefNumber, collectionPeriodYear: 2018, collectionperiodMonth: 3, collectionPeriod: 8, useLevy: false);

            TestDataHelper.SetOpenCollection(currentPeriod, "1718");

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber, currentPeriod: 8);
            TestDataHelper.AddEarningForNonDas(ukprn, new DateTime(2018, 4, 1), plannedEndDate, 12000, learnerRefNumber);

            TestDataHelper.DeleteLearningDeliveryFAM(ukprn, learnerRefNumber, "1", "ACT");
            TestDataHelper.AddLearningDeliveryFam(ukprn, learnerRefNumber, "1", "ACT", 1, new DateTime(2017, 8, 1), new DateTime(2018, 3, 31));
            TestDataHelper.AddLearningDeliveryFam(ukprn, learnerRefNumber, "1", "ACT", 2, new DateTime(2018, 4, 1), null);

            TestDataHelper.AddPaymentHistory();

            TestDataHelper.CopyReferenceData("1718");

            //Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);

            Assert.AreEqual(1, duePayments.Length);

            var act2Payments = duePayments.Where(a => a.ApprenticeshipContractType == 2).ToList();

            Assert.AreEqual(1, act2Payments.Count);

            act2Payments.ForEach(a => Assert.AreEqual(amountDue * 0.8M, a.AmountDue));
        }

        [Test]
        public void ThenWhenTheContractTypeWasChangedFromDasToNonDasFromAPreviousPeriodThenNoExtraRefundsOrPaymentsOccur()
        {
            // Arrange
            const int ukprn = 863145;
            const long commitmentId = 1L;
            var startDate = new DateTime(2017, 8, 12);
            var plannedEndDate = new DateTime(2018, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);
            var uln = new Random().Next(10000000, int.MaxValue);
            const int currentPeriod = 9;
            const decimal amountDue = 1000M;
            var dasEndDate = new DateTime(2017, 12, 31);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber, startDate: startDate, endDate: plannedEndDate, uln: uln);

            TestDataHelper.AddPaymentForCommitment(commitmentId, 8, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, collectionPeriodYear: 2017, collectionperiodMonth: 8, collectionPeriod: 1, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 9, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, collectionPeriodYear: 2017, collectionperiodMonth: 9, collectionPeriod: 2, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 10, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, collectionPeriodYear: 2017, collectionperiodMonth: 10, collectionPeriod: 3, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 11, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, collectionPeriodYear: 2017, collectionperiodMonth: 11, collectionPeriod: 4, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 12, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, collectionPeriodYear: 2017, collectionperiodMonth: 12, collectionPeriod: 5, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 1, 2018, (int)TransactionType.Learning, 800M, learnerRefNumber, apprenticeshipContractType: 2, collectionPeriodYear: 2018, collectionperiodMonth: 1, collectionPeriod: 6, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 2, 2018, (int)TransactionType.Learning, 800M, learnerRefNumber, apprenticeshipContractType: 2, collectionPeriodYear: 2018, collectionperiodMonth: 2, collectionPeriod: 7, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 3, 2018, (int)TransactionType.Learning, 800M, learnerRefNumber, apprenticeshipContractType: 2, collectionPeriodYear: 2018, collectionperiodMonth: 3, collectionPeriod: 8, useLevy: false);

            TestDataHelper.SetOpenCollection(currentPeriod, "1718");

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber, currentPeriod: 5, endPeriod: 5, endDate: dasEndDate);
            TestDataHelper.AddEarningForNonDas(ukprn, new DateTime(2018, 1, 1), plannedEndDate, 12000, learnerRefNumber, 1, 12, startPeriod: 6, endPeriod: 9);

            TestDataHelper.DeleteLearningDeliveryFAM(ukprn, learnerRefNumber, "1", "ACT");
            TestDataHelper.AddLearningDeliveryFam(ukprn, learnerRefNumber, "1", "ACT", 1, new DateTime(2017, 8, 1), dasEndDate);
            TestDataHelper.AddLearningDeliveryFam(ukprn, learnerRefNumber, "1", "ACT", 2, dasEndDate.AddDays(1), null);

            TestDataHelper.AddPaymentHistory();

            TestDataHelper.CopyReferenceData("1718");

            //Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);

            Assert.AreEqual(1, duePayments.Length);

            var act2Payments = duePayments.Where(a => a.ApprenticeshipContractType == 2).ToList();

            Assert.AreEqual(1, act2Payments.Count);

            act2Payments.ForEach(a => Assert.AreEqual(800M, a.AmountDue));
        }

        [Test]
        public void ThenWhenContractTypeChangedFromDasToNonDasInAPreviousAcademicYearNoRefundsOrAdditionalPaymentsAreMade()
        {
            // Arrange
            const int ukprn = 863145;
            const long commitmentId = 1L;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2018, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);
            var uln = new Random().Next(10000000, int.MaxValue);
            const int currentPeriod = 9;
            const decimal amountDue = 800M;
            var dasEndDate = new DateTime(2016, 12, 31);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber, startDate: startDate, endDate: plannedEndDate, uln: uln);

            TestDataHelper.AddPaymentForCommitment(commitmentId, 8, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, apprenticeshipContractType: 2, collectionPeriodYear: 2017, collectionperiodMonth: 8, collectionPeriod: 1, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 9, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, apprenticeshipContractType: 2, collectionPeriodYear: 2017, collectionperiodMonth: 9, collectionPeriod: 2, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 10, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, apprenticeshipContractType: 2, collectionPeriodYear: 2017, collectionperiodMonth: 10, collectionPeriod: 3, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 11, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, apprenticeshipContractType: 2, collectionPeriodYear: 2017, collectionperiodMonth: 11, collectionPeriod: 4, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 12, 2017, (int)TransactionType.Learning, amountDue, learnerRefNumber, apprenticeshipContractType: 2, collectionPeriodYear: 2017, collectionperiodMonth: 12, collectionPeriod: 5, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 1, 2018, (int)TransactionType.Learning, amountDue, learnerRefNumber, apprenticeshipContractType: 2, collectionPeriodYear: 2018, collectionperiodMonth: 1, collectionPeriod: 6, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 2, 2018, (int)TransactionType.Learning, amountDue, learnerRefNumber, apprenticeshipContractType: 2, collectionPeriodYear: 2018, collectionperiodMonth: 2, collectionPeriod: 7, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 3, 2018, (int)TransactionType.Learning, amountDue, learnerRefNumber, apprenticeshipContractType: 2, collectionPeriodYear: 2018, collectionperiodMonth: 3, collectionPeriod: 8, useLevy: false);

            TestDataHelper.SetOpenCollection(currentPeriod, "1718");

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber, currentPeriod: 5, endPeriod: 5, endDate: dasEndDate);
            TestDataHelper.AddEarningForNonDas(ukprn, new DateTime(2017, 1, 1), startDate.AddYears(1).AddDays(-1), 12000, learnerRefNumber);
            TestDataHelper.AddEarningForNonDas(ukprn, new DateTime(2017, 8, 1), plannedEndDate, 12000, learnerRefNumber);

            TestDataHelper.DeleteLearningDeliveryFAM(ukprn, learnerRefNumber, "1", "ACT");
            TestDataHelper.AddLearningDeliveryFam(ukprn, learnerRefNumber, "1", "ACT", 1, new DateTime(2016, 8, 1), dasEndDate);
            TestDataHelper.AddLearningDeliveryFam(ukprn, learnerRefNumber, "1", "ACT", 2, dasEndDate.AddDays(1), null);

            TestDataHelper.AddPaymentHistory();

            TestDataHelper.CopyReferenceData("1718");

            //Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);

            Assert.AreEqual(1, duePayments.Length);

            var act2Payments = duePayments.Where(a => a.ApprenticeshipContractType == 2).ToList();

            Assert.AreEqual(1, act2Payments.Count);

            act2Payments.ForEach(a => Assert.AreEqual(amountDue, a.AmountDue));
        }

        [Test]
        public void ThenWhenContractTypeChangedFromNonDasToDasInAPreviousAcademicYearNoRefundsOrAdditionalPaymentsAreMade()
        {
            // Arrange
            const int ukprn = 863145;
            const long commitmentId = 1L;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2018, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);
            var uln = new Random().Next(10000000, int.MaxValue);
            const int currentPeriod = 9;
            const decimal dasAmountDue = 1000M;
            var nonDasEndDate = new DateTime(2016, 12, 31);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber, startDate: startDate, endDate: plannedEndDate, uln: uln);

            TestDataHelper.AddPaymentForCommitment(commitmentId, 8, 2017, (int)TransactionType.Learning, dasAmountDue, learnerRefNumber, apprenticeshipContractType: 1, collectionPeriodYear: 2017, collectionperiodMonth: 8, collectionPeriod: 1, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 9, 2017, (int)TransactionType.Learning, dasAmountDue, learnerRefNumber, apprenticeshipContractType: 1, collectionPeriodYear: 2017, collectionperiodMonth: 9, collectionPeriod: 2, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 10, 2017, (int)TransactionType.Learning, dasAmountDue, learnerRefNumber, apprenticeshipContractType: 1, collectionPeriodYear: 2017, collectionperiodMonth: 10, collectionPeriod: 3, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 11, 2017, (int)TransactionType.Learning, dasAmountDue, learnerRefNumber, apprenticeshipContractType: 1, collectionPeriodYear: 2017, collectionperiodMonth: 11, collectionPeriod: 4, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 12, 2017, (int)TransactionType.Learning, dasAmountDue, learnerRefNumber, apprenticeshipContractType: 1, collectionPeriodYear: 2017, collectionperiodMonth: 12, collectionPeriod: 5, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 1, 2018, (int)TransactionType.Learning, dasAmountDue, learnerRefNumber, apprenticeshipContractType: 1, collectionPeriodYear: 2018, collectionperiodMonth: 1, collectionPeriod: 6, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 2, 2018, (int)TransactionType.Learning, dasAmountDue, learnerRefNumber, apprenticeshipContractType: 1, collectionPeriodYear: 2018, collectionperiodMonth: 2, collectionPeriod: 7, useLevy: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 3, 2018, (int)TransactionType.Learning, dasAmountDue, learnerRefNumber, apprenticeshipContractType: 1, collectionPeriodYear: 2018, collectionperiodMonth: 3, collectionPeriod: 8, useLevy: false);

            TestDataHelper.SetOpenCollection(currentPeriod, "1718");

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber, endDate: new DateTime(2017,7,31), startDate: nonDasEndDate.AddDays(1));
            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber, startDate: new DateTime(2017,8,1));
            TestDataHelper.AddEarningForNonDas(ukprn, new DateTime(2016, 8, 1), nonDasEndDate, 12000, learnerRefNumber);
            
            TestDataHelper.DeleteLearningDeliveryFAM(ukprn, learnerRefNumber, "1", "ACT");
            TestDataHelper.AddLearningDeliveryFam(ukprn, learnerRefNumber, "1", "ACT", 2, new DateTime(2016, 8, 1), nonDasEndDate);
            TestDataHelper.AddLearningDeliveryFam(ukprn, learnerRefNumber, "1", "ACT", 1, nonDasEndDate.AddDays(1), null);

            TestDataHelper.AddPaymentHistory();

            TestDataHelper.CopyReferenceData("1718");

            //Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);

            Assert.AreEqual(1, duePayments.Length);

            var act1Payments = duePayments.Where(a => a.ApprenticeshipContractType == 1).ToList();

            Assert.AreEqual(1, act1Payments.Count);

            act1Payments.ForEach(a => Assert.AreEqual(dasAmountDue, a.AmountDue));
        }
    }
}
