using System;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Tools;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.FinishedOnTime
{
    public class WhenMakingDuePayments
    {
        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();
        }

        [Test]
        public void ThenItShouldWriteCorrectDetailsForPaymentsDue()
        {
            // Arrange
            var ukprn = 863145;
            var uln = 834734;
            var commitmentId = 1L;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber, uln: uln, startDate: startDate, endDate: plannedEndDate);

            TestDataHelper.SetOpenCollection(1);

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber, currentPeriod: 1);

            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            Assert.AreEqual(1, duePayments.Length);

            Assert.AreEqual(commitmentId, duePayments[0].CommitmentId);
            Assert.AreEqual("1", duePayments[0].CommitmentVersionId);
            Assert.AreEqual("123", duePayments[0].AccountId);
            Assert.AreEqual("20170401", duePayments[0].AccountVersionId);
            Assert.AreEqual(uln, duePayments[0].Uln);
            Assert.AreEqual(learnerRefNumber, duePayments[0].LearnRefNumber);
            Assert.AreEqual(1, duePayments[0].AimSeqNumber);
            Assert.AreEqual(ukprn, duePayments[0].Ukprn);
            Assert.AreEqual(DateTime.Today, duePayments[0].IlrSubmissionDateTime);
            Assert.AreEqual(8, duePayments[0].DeliveryMonth);
            Assert.AreEqual(2016, duePayments[0].DeliveryYear);
            Assert.AreEqual((int)TransactionType.Learning, duePayments[0].TransactionType);
            Assert.AreEqual(1000, duePayments[0].AmountDue);
        }

        [Test]
        public void ThenItShouldMakePaymentsForEachPeriodUptoAndIncludingTheCurrent()
        {
            // Arrange
            var ukprn = 863145;
            var commitmentId = 1L;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber, startDate: startDate, endDate: plannedEndDate);

            TestDataHelper.SetOpenCollection(5);

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber, currentPeriod: 5);

            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            Assert.AreEqual(5, duePayments.Length);

            Assert.AreEqual(commitmentId, duePayments[0].CommitmentId);
            Assert.AreEqual(8, duePayments[0].DeliveryMonth);
            Assert.AreEqual(2016, duePayments[0].DeliveryYear);
            Assert.AreEqual(1000, duePayments[0].AmountDue);
            Assert.AreEqual((int)TransactionType.Learning, duePayments[0].TransactionType);

            Assert.AreEqual(commitmentId, duePayments[1].CommitmentId);
            Assert.AreEqual(9, duePayments[1].DeliveryMonth);
            Assert.AreEqual(2016, duePayments[1].DeliveryYear);
            Assert.AreEqual(1000, duePayments[1].AmountDue);
            Assert.AreEqual((int)TransactionType.Learning, duePayments[1].TransactionType);

            Assert.AreEqual(commitmentId, duePayments[2].CommitmentId);
            Assert.AreEqual(10, duePayments[2].DeliveryMonth);
            Assert.AreEqual(2016, duePayments[2].DeliveryYear);
            Assert.AreEqual(1000, duePayments[2].AmountDue);
            Assert.AreEqual((int)TransactionType.Learning, duePayments[2].TransactionType);

            Assert.AreEqual(commitmentId, duePayments[3].CommitmentId);
            Assert.AreEqual(11, duePayments[3].DeliveryMonth);
            Assert.AreEqual(2016, duePayments[3].DeliveryYear);
            Assert.AreEqual(1000, duePayments[3].AmountDue);
            Assert.AreEqual((int)TransactionType.Learning, duePayments[3].TransactionType);

            Assert.AreEqual(commitmentId, duePayments[4].CommitmentId);
            Assert.AreEqual(12, duePayments[4].DeliveryMonth);
            Assert.AreEqual(2016, duePayments[4].DeliveryYear);
            Assert.AreEqual(1000, duePayments[4].AmountDue);
            Assert.AreEqual((int)TransactionType.Learning, duePayments[4].TransactionType);
        }

        [Test]
        public void ThenItShouldNotMakePaymentsForPeriodWhereEarningHasBeenFullyPaidAlready()
        {
            // Arrange
            var ukprn = 863145;
            var commitmentId = 1L;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber, startDate: startDate, endDate: plannedEndDate);

            TestDataHelper.SetOpenCollection(5);

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber, currentPeriod: 5);

            TestDataHelper.AddPaymentForCommitment(commitmentId, 8, 2016, (int)TransactionType.Learning, 1000);

            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            Assert.AreEqual(4, duePayments.Length);
            Assert.False(duePayments.Any(p => p.DeliveryMonth == 8 && p.DeliveryYear == 2016));
        }

        [Test]
        public void ThenItShouldMakePartPaymentsForPeriodWhereEarningHasBeenPartiallyPaidAlready()
        {
            // Arrange
            var ukprn = 863145;
            var commitmentId = 1L;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber, startDate: startDate, endDate: plannedEndDate);

            TestDataHelper.SetOpenCollection(5);

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber, currentPeriod: 5);

            TestDataHelper.AddPaymentForCommitment(commitmentId, 8, 2016, (int)TransactionType.Learning, 500);

            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            Assert.AreEqual(5, duePayments.Length);

            var actualPayment = duePayments.SingleOrDefault(p => p.DeliveryMonth == 8 && p.DeliveryYear == 2016);
            Assert.IsNotNull(actualPayment);
            Assert.AreEqual(500m, actualPayment.AmountDue);
        }

        [Test]
        public void ThenItShouldMakePaymentsForLearningAndCompletionInFinalMonth()
        {
            // Arrange
            var ukprn = 863145;
            var commitmentId = 1L;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber, startDate: startDate, endDate: plannedEndDate);

            TestDataHelper.SetOpenCollection(12);

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber, currentPeriod: 12);

            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            var period12Payments = duePayments.Where(p => p.DeliveryMonth == 7 && p.DeliveryYear == 2017)
                                              .OrderBy(p => p.TransactionType)
                                              .ToArray();
            Assert.AreEqual(2, period12Payments.Length);

            Assert.AreEqual((int)TransactionType.Learning, period12Payments[0].TransactionType);
            Assert.AreEqual(1000, period12Payments[0].AmountDue);

            Assert.AreEqual((int)TransactionType.Completion, period12Payments[1].TransactionType);
            Assert.AreEqual(3000, period12Payments[1].AmountDue);
        }

        [Test]
        public void ThenItShouldNotMakePaymentsIfDoesNotPassDataLock()
        {
            // Arrange
            var ukprn = 863145;
            var commitmentId = 1L;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber, startDate: startDate, endDate: plannedEndDate, passedDataLock: false);

            TestDataHelper.SetOpenCollection(5);

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber, currentPeriod: 5);

            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            Assert.AreEqual(0, duePayments.Length);
        }

    }
}