using System;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.Tools;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.IntegrationTests.FinishedOnTime
{
    public class WhenALearnerChangesCourseOrAimRef
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

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber, currentPeriod: 5);

            TestDataHelper.AddPaymentForCommitment(commitmentId, 8, 2016, (int)TransactionType.Learning, 1000, learnerRefNumber,learnAimRef:"ZXXXXXXX");

            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            Assert.AreEqual(6, duePayments.Length);

            //there should be 2 transactions for August
            Assert.AreEqual(2, duePayments.Count(x => x.DeliveryMonth == 8));
            Assert.AreEqual(2, duePayments.Count(x => x.TransactionType == 1 && x.DeliveryMonth == 8));

            //one new transaction with the corrected ZPROG
            Assert.AreEqual(1, duePayments.Count(x => x.TransactionType == 1 && x.DeliveryMonth == 8 & x.AmountDue == 1000.00m && x.LearnAimRef== "ZPROG001"));

            var actualPaymentDue = duePayments.Single(x=> x.DeliveryMonth ==8 && x.AmountDue == -1000m);

            Assert.AreEqual("123", actualPaymentDue.AccountId);
            Assert.AreEqual("NA", actualPaymentDue.AccountVersionId);
            Assert.AreEqual(1, actualPaymentDue.AimSeqNumber);
            Assert.AreEqual(-1000m, actualPaymentDue.AmountDue);
            Assert.AreEqual(1, actualPaymentDue.ApprenticeshipContractType);
            Assert.AreEqual(commitmentId, actualPaymentDue.CommitmentId);
            Assert.AreEqual("1", actualPaymentDue.CommitmentVersionId);
            Assert.AreEqual(8, actualPaymentDue.DeliveryMonth);
            Assert.AreEqual(2016, actualPaymentDue.DeliveryYear);
            Assert.IsNull(actualPaymentDue.FrameworkCode);
            Assert.AreEqual("Non-Levy Funding Line", actualPaymentDue.FundingLineType);
            Assert.AreEqual(DateTime.Today, actualPaymentDue.IlrSubmissionDateTime.Date);
            Assert.AreEqual("ZXXXXXXX", actualPaymentDue.LearnAimRef);
            Assert.AreEqual(startDate, actualPaymentDue.LearningStartDate);
            Assert.AreEqual(learnerRefNumber, actualPaymentDue.LearnRefNumber);
            Assert.IsNull(actualPaymentDue.PathwayCode);
            Assert.AreEqual("25-27-01/04/2017", actualPaymentDue.PriceEpisodeIdentifier);
            Assert.IsNull(actualPaymentDue.ProgrammeType);
            Assert.AreEqual(0.9m, actualPaymentDue.SfaContributionPercentage);
            Assert.AreEqual(123456, actualPaymentDue.StandardCode);
            Assert.AreEqual(1, actualPaymentDue.TransactionType);
            Assert.AreEqual(ukprn, actualPaymentDue.Ukprn);
            Assert.AreEqual(uln, actualPaymentDue.Uln);
            Assert.AreEqual(true, actualPaymentDue.UseLevyBalance);

        }

        [Test]
        public void ThenItShouldCreateReversalPaymentsForMathsEnglishPreviousMonthsInIlr()
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

            TestDataHelper.AddEarningForCommitment(commitmentId, learnerRefNumber, currentPeriod: 5);
            TestDataHelper.AddMathsAndEnglishEarningForCommitment(commitmentId, learnerRefNumber.Substring(0, 11) + "a", currentPeriod: 5);


            TestDataHelper.AddPaymentForCommitment(commitmentId, 8, 2016, (int)TransactionType.Learning, 1000, learnerRefNumber);
            TestDataHelper.AddPaymentForCommitment(commitmentId, 8, 2016, (int)TransactionType.OnProgrammeMathsAndEnglish, 39.25m, learnerRefNumber, learnAimRef: "ZXXXXXXX");


            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            Assert.AreEqual(5, duePayments.Length);

            //there should be only one transaction for August 
            Assert.AreEqual(1, duePayments.Count(x=> x.DeliveryMonth == 8));

            //there should only be maths/english transation for august
            Assert.AreEqual(1, duePayments.Count(x => x.TransactionType == 13));

            var actualPaymentDue = duePayments.Single(x=> x.TransactionType == 13);

            Assert.AreEqual("123", actualPaymentDue.AccountId);
            Assert.AreEqual("NA", actualPaymentDue.AccountVersionId);
            Assert.AreEqual(1, actualPaymentDue.AimSeqNumber);
            Assert.AreEqual(-39.25, actualPaymentDue.AmountDue);
            Assert.AreEqual(1, actualPaymentDue.ApprenticeshipContractType);
            Assert.AreEqual(commitmentId, actualPaymentDue.CommitmentId);
            Assert.AreEqual("1", actualPaymentDue.CommitmentVersionId);
            Assert.AreEqual(8, actualPaymentDue.DeliveryMonth);
            Assert.AreEqual(2016, actualPaymentDue.DeliveryYear);
            Assert.IsNull(actualPaymentDue.FrameworkCode);
            Assert.AreEqual("Non-Levy Funding Line", actualPaymentDue.FundingLineType);
            Assert.AreEqual(DateTime.Today, actualPaymentDue.IlrSubmissionDateTime.Date);
            Assert.AreEqual("ZXXXXXXX", actualPaymentDue.LearnAimRef);
            Assert.AreEqual(startDate, actualPaymentDue.LearningStartDate);
            Assert.AreEqual(learnerRefNumber, actualPaymentDue.LearnRefNumber);
            Assert.IsNull(actualPaymentDue.PathwayCode);
            Assert.AreEqual("25-27-01/04/2017", actualPaymentDue.PriceEpisodeIdentifier);
            Assert.IsNull(actualPaymentDue.ProgrammeType);
            Assert.AreEqual(0.9m, actualPaymentDue.SfaContributionPercentage);
            Assert.AreEqual(123456, actualPaymentDue.StandardCode);
            Assert.AreEqual(TransactionType.OnProgrammeMathsAndEnglish, (TransactionType)actualPaymentDue.TransactionType);
            Assert.AreEqual(ukprn, actualPaymentDue.Ukprn);
            Assert.AreEqual(uln, actualPaymentDue.Uln);
            Assert.AreEqual(true, actualPaymentDue.UseLevyBalance);

        }

        [Test]
        public void ThenItShouldCreateReversalPaymentsForPreviousMonthsInIlrWhenCourseChanged()
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

            TestDataHelper.AddPaymentForCommitment(commitmentId, 8, 2016, (int)TransactionType.Learning, 1000, learnerRefNumber
                                                , frameworkCode: 99999);

            TestDataHelper.CopyReferenceData();

            // Act
            var context = new ExternalContextStub();
            var task = new PaymentsDueTask();
            task.Execute(context);

            // Assert
            var duePayments = TestDataHelper.GetRequiredPaymentsForProvider(ukprn);
            Assert.AreEqual(6, duePayments.Length);

            //there should be 2 transactions for August
            Assert.AreEqual(2, duePayments.Count(x => x.DeliveryMonth == 8));
            Assert.AreEqual(2, duePayments.Count(x => x.TransactionType == 1 && x.DeliveryMonth == 8));

            //one new transaction with the corrected ZPROG
            Assert.AreEqual(1, duePayments.Count(x => x.TransactionType == 1 && x.DeliveryMonth == 8 & x.AmountDue == 1000.00m && x.LearnAimRef == "ZPROG001"));

            var actualPaymentDue = duePayments.Single(x => x.DeliveryMonth == 8 && x.AmountDue == -1000m);

            Assert.AreEqual("123", actualPaymentDue.AccountId);
            Assert.AreEqual("NA", actualPaymentDue.AccountVersionId);
            Assert.AreEqual(1, actualPaymentDue.AimSeqNumber);
            Assert.AreEqual(-1000m, actualPaymentDue.AmountDue);
            Assert.AreEqual(1, actualPaymentDue.ApprenticeshipContractType);
            Assert.AreEqual(commitmentId, actualPaymentDue.CommitmentId);
            Assert.AreEqual("1", actualPaymentDue.CommitmentVersionId);
            Assert.AreEqual(8, actualPaymentDue.DeliveryMonth);
            Assert.AreEqual(2016, actualPaymentDue.DeliveryYear);
            Assert.AreEqual(99999,actualPaymentDue.FrameworkCode);
            Assert.AreEqual("Non-Levy Funding Line", actualPaymentDue.FundingLineType);
            Assert.AreEqual(DateTime.Today, actualPaymentDue.IlrSubmissionDateTime.Date);
            Assert.AreEqual("ZPROG001", actualPaymentDue.LearnAimRef);
            Assert.AreEqual(startDate, actualPaymentDue.LearningStartDate);
            Assert.AreEqual(learnerRefNumber, actualPaymentDue.LearnRefNumber);
            Assert.IsNull(actualPaymentDue.PathwayCode);
            Assert.AreEqual("25-27-01/04/2017", actualPaymentDue.PriceEpisodeIdentifier);
            Assert.IsNull(actualPaymentDue.ProgrammeType);
            Assert.AreEqual(0.9m, actualPaymentDue.SfaContributionPercentage);
            Assert.AreEqual(123456, actualPaymentDue.StandardCode);
            Assert.AreEqual(1, actualPaymentDue.TransactionType);
            Assert.AreEqual(ukprn, actualPaymentDue.Ukprn);
            Assert.AreEqual(uln, actualPaymentDue.Uln);
            Assert.AreEqual(true, actualPaymentDue.UseLevyBalance);

        }
    }
}
