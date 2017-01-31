using System;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Tools;
using SFA.DAS.Payments.DCFS.Domain;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.FinishedOnTime
{
    public class WhenMakingDuePaymentsForNonDasLearners
    {
        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();
        }

        [Test]
        [TestCase(25L, null, null, null)]
        [TestCase(null, 550, 20, 6)]
        public void ThenItShouldWriteCorrectDetailsForPaymentsDue(long? standardCode, int? frameworkCode, int? programmeType, int? pathwayCode)
        {
            // Arrange
            var ukprn = 863145;
            var uln = 834734;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.SetOpenCollection(1);

            TestDataHelper.AddEarningForNonDas(ukprn, startDate, plannedEndDate, 15000, learnerRefNumber, uln: uln,
                standardCode: standardCode, programmeType: programmeType, frameworkCode: frameworkCode,
                pathwayCode: pathwayCode);

            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            Assert.AreEqual(1, duePayments.Length);

            Assert.IsNull(duePayments[0].CommitmentId);
            Assert.IsNull(duePayments[0].CommitmentVersionId);
            Assert.IsNull(duePayments[0].AccountId);
            Assert.IsNull(duePayments[0].AccountVersionId);
            Assert.AreEqual(uln, duePayments[0].Uln);
            Assert.AreEqual(learnerRefNumber, duePayments[0].LearnRefNumber);
            Assert.AreEqual(1, duePayments[0].AimSeqNumber);
            Assert.AreEqual(ukprn, duePayments[0].Ukprn);
            Assert.AreEqual(DateTime.Today, duePayments[0].IlrSubmissionDateTime);
            Assert.AreEqual(8, duePayments[0].DeliveryMonth);
            Assert.AreEqual(2016, duePayments[0].DeliveryYear);
            Assert.AreEqual((int)TransactionType.Learning, duePayments[0].TransactionType);
            Assert.AreEqual(1000, duePayments[0].AmountDue);

            Assert.AreEqual(standardCode, duePayments[0].StandardCode);
            Assert.AreEqual(frameworkCode, duePayments[0].FrameworkCode);
            Assert.AreEqual(programmeType, duePayments[0].ProgrammeType);
            Assert.AreEqual(pathwayCode, duePayments[0].PathwayCode);

            Assert.AreEqual(0.9m, duePayments[0].SfaContributionPercentage);
            Assert.AreEqual("Non-Levy Funding Line", duePayments[0].FundingLineType);
        }

        [Test]
        public void ThenItShouldMakePaymentsForEachPeriodUptoAndIncludingTheCurrent()
        {
            // Arrange
            var ukprn = 863145;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.SetOpenCollection(5);

            TestDataHelper.AddEarningForNonDas(ukprn, startDate, plannedEndDate, 15000, learnerRefNumber, currentPeriod: 5);

            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            Assert.AreEqual(5, duePayments.Length);

            Assert.IsNull(duePayments[0].CommitmentId);
            Assert.AreEqual(8, duePayments[0].DeliveryMonth);
            Assert.AreEqual(2016, duePayments[0].DeliveryYear);
            Assert.AreEqual(1000, duePayments[0].AmountDue);
            Assert.AreEqual((int)TransactionType.Learning, duePayments[0].TransactionType);

            Assert.IsNull(duePayments[1].CommitmentId);
            Assert.AreEqual(9, duePayments[1].DeliveryMonth);
            Assert.AreEqual(2016, duePayments[1].DeliveryYear);
            Assert.AreEqual(1000, duePayments[1].AmountDue);
            Assert.AreEqual((int)TransactionType.Learning, duePayments[1].TransactionType);

            Assert.IsNull(duePayments[2].CommitmentId);
            Assert.AreEqual(10, duePayments[2].DeliveryMonth);
            Assert.AreEqual(2016, duePayments[2].DeliveryYear);
            Assert.AreEqual(1000, duePayments[2].AmountDue);
            Assert.AreEqual((int)TransactionType.Learning, duePayments[2].TransactionType);

            Assert.IsNull(duePayments[3].CommitmentId);
            Assert.AreEqual(11, duePayments[3].DeliveryMonth);
            Assert.AreEqual(2016, duePayments[3].DeliveryYear);
            Assert.AreEqual(1000, duePayments[3].AmountDue);
            Assert.AreEqual((int)TransactionType.Learning, duePayments[3].TransactionType);

            Assert.IsNull(duePayments[4].CommitmentId);
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
            var uln = 1765935903;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.SetOpenCollection(5);

            TestDataHelper.AddEarningForNonDas(ukprn, startDate, plannedEndDate, 15000, learnerRefNumber, currentPeriod: 5, uln: uln);

            TestDataHelper.AddPaymentForNonDas(ukprn, uln, 8, 2016, (int)TransactionType.Learning, 1000);

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
            var uln = 1765935903;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.SetOpenCollection(5);

            TestDataHelper.AddEarningForNonDas(ukprn, startDate, plannedEndDate, 15000, learnerRefNumber, currentPeriod: 5, uln: uln);

            TestDataHelper.AddPaymentForNonDas(ukprn, uln, 8, 2016, (int)TransactionType.Learning, 500);

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
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.SetOpenCollection(12);

            TestDataHelper.AddEarningForNonDas(ukprn, startDate, plannedEndDate, 15000, learnerRefNumber, currentPeriod: 12);

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
        public void ThenItShouldMakePaymentsForLearningCompletionAndBalancingForAnEarlyCompletion()
        {
            // Arrange
            var ukprn = 863145;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.SetOpenCollection(10);

            TestDataHelper.AddEarningForNonDas(ukprn, startDate, plannedEndDate, 15000, learnerRefNumber, currentPeriod: 10, earlyFinisher: true);

            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            var period10Payments = duePayments.Where(p => p.DeliveryMonth == 5 && p.DeliveryYear == 2017)
                                              .OrderBy(p => p.TransactionType)
                                              .ToArray();
            Assert.AreEqual(3, period10Payments.Length);

            Assert.AreEqual((int)TransactionType.Learning, period10Payments[0].TransactionType);
            Assert.AreEqual(1000, period10Payments[0].AmountDue);

            Assert.AreEqual((int)TransactionType.Completion, period10Payments[1].TransactionType);
            Assert.AreEqual(3000, period10Payments[1].AmountDue);

            Assert.AreEqual((int)TransactionType.Balancing, period10Payments[2].TransactionType);
            Assert.AreEqual(2000, period10Payments[2].AmountDue);
        }
    }
}