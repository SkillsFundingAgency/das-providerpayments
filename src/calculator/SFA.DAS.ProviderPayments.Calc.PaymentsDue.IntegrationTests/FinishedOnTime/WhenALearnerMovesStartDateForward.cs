using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Tools;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.FinishedOnTime
{
    public class WhenALearnerMovesStartDateForward
    {

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();
        }
        

        [Test]
        public void ThenItShouldCreateOnProgReversalPaymentsForPreviousMonthsInIlrWhenStartDateIsMoved()
        {
            // Arrange
            var ukprn = 863145;
            var commitmentId = 1L;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);
            var uln = new Random().Next(10000000, int.MaxValue);

            TestDataHelper.AddProvider(ukprn);


            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber, 
                                        startDate: startDate, endDate: plannedEndDate, uln: uln, frameworkCode: 14231);

            TestDataHelper.SetOpenCollection(5);

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber, currentPeriod: 5);

            //set the eaninngs to 0 to mimic when we dont get earnings for start date moved
            TestDataHelper.RemoveApprenticeEarning(ukprn, learnerRefNumber, 1, 0);
            TestDataHelper.RemoveApprenticeEarning(ukprn, learnerRefNumber, 2, 0);

            TestDataHelper.AddPaymentForCommitment(commitmentId, 8, 2016, (int)TransactionType.Learning, 1000, learnerRefNumber);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 9, 2016, (int)TransactionType.Learning, 1000, learnerRefNumber);

            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            Assert.AreEqual(2, duePayments.Length);

            //there should be 2 transactions for August
            Assert.AreEqual(1, duePayments.Count(x => x.DeliveryMonth == 8));
            Assert.AreEqual(1, duePayments.Count(x => x.TransactionType == 1 && x.DeliveryMonth == 8));

            //for August/Sep it should be 0 now
            Assert.AreEqual(-1000, duePayments.Where(x => x.TransactionType == 1 && x.DeliveryMonth == 8).Sum(x=> x.AmountDue));
            Assert.AreEqual(-1000, duePayments.Where(x => x.TransactionType == 1 && x.DeliveryMonth == 9).Sum(x => x.AmountDue));

            

            
        }

        [Test]
        public void ThenItShouldCreateReversalLearningSupportPaymentsForPreviousMonthsInIlrWhenLearningSupportStartDateIsMoved()
        {
            // Arrange
            var ukprn = 863145;
            var commitmentId = 1L;
            var startDate = new DateTime(2016, 8, 12);
            var plannedEndDate = new DateTime(2017, 8, 27);
            var learnerRefNumber = Guid.NewGuid().ToString("N").Substring(0, 12);
            var uln = new Random().Next(10000000, int.MaxValue);

            TestDataHelper.AddProvider(ukprn);


            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber,
                                        startDate: startDate, endDate: plannedEndDate, uln: uln, frameworkCode: 14231);

            TestDataHelper.SetOpenCollection(5);

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber, currentPeriod: 5);

            TestDataHelper.AddPaymentForCommitment(commitmentId, 8, 2016, (int)TransactionType.Learning, 1000, learnerRefNumber);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 9, 2016, (int)TransactionType.Learning, 1000, learnerRefNumber);

            TestDataHelper.AddPaymentForCommitment(commitmentId, 8, 2016, (int)TransactionType.LearningSupport, 150, learnerRefNumber);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 9, 2016, (int)TransactionType.LearningSupport, 150, learnerRefNumber);

            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            Assert.AreEqual(5, duePayments.Length);

            //there should be 1 transactions for August/Sep
            
            Assert.AreEqual(1, duePayments.Count(x => x.TransactionType == 15 && x.DeliveryMonth == 8 && x.AmountDue==-150m));
            Assert.AreEqual(0, duePayments.Count(x => x.TransactionType == 1 && x.DeliveryMonth == 8));

            
            Assert.AreEqual(1, duePayments.Count(x => x.TransactionType == 15 && x.DeliveryMonth == 9 && x.AmountDue == -150m));
            Assert.AreEqual(0, duePayments.Count(x => x.TransactionType == 1 && x.DeliveryMonth == 8));

            Assert.AreEqual(1, duePayments.Count(x => x.TransactionType == 1 && x.DeliveryMonth == 10 && x.AmountDue == 1000m));
            Assert.AreEqual(1, duePayments.Count(x => x.TransactionType == 1 && x.DeliveryMonth == 11 && x.AmountDue == 1000m));

        }
    }
}
