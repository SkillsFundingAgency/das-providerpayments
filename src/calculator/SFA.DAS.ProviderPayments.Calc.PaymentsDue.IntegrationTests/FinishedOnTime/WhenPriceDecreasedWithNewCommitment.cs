using System;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Tools;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.FinishedOnTime
{
    public class WhenPriceDecreasedWithNewCommitment
    {

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();
        }
        

        [Test]
        public void ThenItShouldCreateOnProgReversalPaymentsAndAttachToOriginalCommitment()
        {
            // Arrange
            var ukprn = 863145;
            var commitmentId1 = 1L;
            var commitmentId2 = 2L;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);
            var uln = new Random().Next(10000000, int.MaxValue);

            TestDataHelper.AddProvider(ukprn);


            TestDataHelper.AddCommitment(commitmentId1, ukprn, learnerRefNumber,
                                        startDate: startDate, endDate: plannedEndDate, uln: uln, frameworkCode: 14231,addPriceEpisodeMatches: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId1, 8, 2016, (int)TransactionType.Learning, 1000, learnerRefNumber);
            TestDataHelper.AddPaymentForCommitment(commitmentId1, 9, 2016, (int)TransactionType.Learning, 1000, learnerRefNumber);

            TestDataHelper.SetOpenCollection(5);

            TestDataHelper.AddCommitment(commitmentId2, ukprn, learnerRefNumber,
                                   startDate: startDate, endDate: plannedEndDate, uln: uln, frameworkCode: 14231,agreedCost :1m);

            TestDataHelper.AddEarningForCommitment(commitmentId2, learnerRefNumber, currentPeriod: 5);

          
            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
         
            //there should be 2 transactions for August
            Assert.AreEqual(1, duePayments.Count(x => x.DeliveryMonth == 8 && x.AmountDue == -999.93333m && x.CommitmentId == commitmentId1 && x.TransactionType ==(int)TransactionType.Learning));
            Assert.AreEqual(1, duePayments.Count(x => x.DeliveryMonth == 9 && x.AmountDue == -999.93333m && x.CommitmentId == commitmentId1 && x.TransactionType == (int)TransactionType.Learning));

            Assert.AreEqual(1, duePayments.Count(x => x.DeliveryMonth == 10 && x.AmountDue == 0.06667m && x.CommitmentId == commitmentId2 && x.TransactionType == (int)TransactionType.Learning));
            Assert.AreEqual(1, duePayments.Count(x => x.DeliveryMonth == 11 && x.AmountDue == 0.06667m && x.CommitmentId == commitmentId2 && x.TransactionType == (int)TransactionType.Learning));
            Assert.AreEqual(1, duePayments.Count(x => x.DeliveryMonth == 12 && x.AmountDue == 0.06667m && x.CommitmentId == commitmentId2 && x.TransactionType == (int)TransactionType.Learning));
        }

        [Test]
        public void ThenItShouldNotUseCurrentCollectionPeriodTransactionForReversal()
        {
            // Arrange
            var ukprn = 863145;
            var commitmentId1 = 1L;
            var commitmentId2 = 2L;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);
            var uln = new Random().Next(10000000, int.MaxValue);

            TestDataHelper.AddProvider(ukprn);
            
            TestDataHelper.AddCommitment(commitmentId1, ukprn, learnerRefNumber,
                                        startDate: startDate, endDate: plannedEndDate, uln: uln, frameworkCode: 14231, addPriceEpisodeMatches: false);
            TestDataHelper.AddPaymentForCommitment(commitmentId1, 8, 2016, (int)TransactionType.Learning, 1000, learnerRefNumber);
            TestDataHelper.AddPaymentForCommitment(commitmentId1, 9, 2016, (int)TransactionType.Learning, 1000, learnerRefNumber,collectionperiodMonth:8, collectionPeriodYear:2016);

            TestDataHelper.SetOpenCollection(2);

            TestDataHelper.AddCommitment(commitmentId2, ukprn, learnerRefNumber,
                                   startDate: startDate, endDate: plannedEndDate, uln: uln, frameworkCode: 14231, agreedCost: 1m);
            TestDataHelper.AddPaymentForCommitment(commitmentId2, 9, 2016, (int)TransactionType.Learning, 500, learnerRefNumber);

            TestDataHelper.AddEarningForCommitment(commitmentId2, learnerRefNumber, currentPeriod: 2);
            
            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);

            //there should be 1 transactions for Sep
            Assert.AreEqual(1, duePayments.Count(x => x.DeliveryMonth == 9 && x.AmountDue == -0.06667m && x.CommitmentId == commitmentId1 && x.TransactionType == (int)TransactionType.Learning));
        }
    }
}
