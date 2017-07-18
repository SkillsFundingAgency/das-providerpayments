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
    public class WhenALearnerIsNoLongerInIlr
    {

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();
        }

        [Test]
        public void ThenItShouldCreateReversalPaymentsForPreviousMonthsInIlr()
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

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber.Substring(0, 11) + "a", currentPeriod: 5);

            TestDataHelper.AddPaymentForCommitment(commitmentId, 8, 2016, (int)TransactionType.Learning, 1000, learnerRefNumber);

            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            Assert.AreEqual(1, duePayments.Length);

            var actualPaymentDue = duePayments[0];
            Assert.AreEqual(8, actualPaymentDue.DeliveryMonth);
            Assert.AreEqual(-1000m, actualPaymentDue.AmountDue);
        }
    }
}
