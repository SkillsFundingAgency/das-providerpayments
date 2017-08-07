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
            var uln = new Random().Next(10000000, int.MaxValue);

            TestDataHelper.AddProvider(ukprn);

            TestDataHelper.AddCommitment(commitmentId, ukprn, learnerRefNumber, startDate: startDate, endDate: plannedEndDate, uln: uln);

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
            Assert.AreEqual("123", actualPaymentDue.AccountId);
            Assert.AreEqual("NA", actualPaymentDue.AccountVersionId);
            Assert.AreEqual(1, actualPaymentDue.AimSeqNumber);
            Assert.AreEqual(-1000m, actualPaymentDue.AmountDue);
            Assert.AreEqual(1, actualPaymentDue.ApprenticeshipContractType); //
            Assert.AreEqual(commitmentId, actualPaymentDue.CommitmentId);
            Assert.AreEqual("1", actualPaymentDue.CommitmentVersionId);
            Assert.AreEqual(8, actualPaymentDue.DeliveryMonth);
            Assert.AreEqual(2016, actualPaymentDue.DeliveryYear);
            Assert.IsNull(actualPaymentDue.FrameworkCode);
            Assert.AreEqual("Non-Levy Funding Line", actualPaymentDue.FundingLineType); //
            Assert.AreEqual(DateTime.Today, actualPaymentDue.IlrSubmissionDateTime.Date);
            Assert.AreEqual("ZPROG001", actualPaymentDue.LearnAimRef); //
            Assert.AreEqual(startDate, actualPaymentDue.LearningStartDate);
            Assert.AreEqual(learnerRefNumber, actualPaymentDue.LearnRefNumber);
            Assert.IsNull(actualPaymentDue.PathwayCode);
            Assert.AreEqual("25-27-01/04/2017", actualPaymentDue.PriceEpisodeIdentifier); //
            Assert.IsNull(actualPaymentDue.ProgrammeType);
            Assert.AreEqual(0.9m, actualPaymentDue.SfaContributionPercentage); //
            Assert.AreEqual(123456, actualPaymentDue.StandardCode);
            Assert.AreEqual(1, actualPaymentDue.TransactionType);
            Assert.AreEqual(ukprn, actualPaymentDue.Ukprn);
            Assert.AreEqual(uln, actualPaymentDue.Uln);
            Assert.AreEqual(true, actualPaymentDue.UseLevyBalance); //

        }
    }
}
