using System;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Payments;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.IntegrationTests.FinishOnTime
{
    public class WhenThereIsEnoughLevyInTheAccount
    {
        [Test]
        public void ThenASinglePaymentOfLevyIsMadeForTheEarningsWhereLearningIsNotComplete()
        {
            // Arrange
            var accountId = Guid.NewGuid().ToString();
            TestDataHelper.AddAccount(accountId);

            var commitmentId = Guid.NewGuid().ToString();
            TestDataHelper.AddCommitment(commitmentId, accountId);

            TestDataHelper.AddPaymentDueForCommitment(commitmentId);

            var taskContext = new IntegrationTaskContext();
            var task = new LevyPaymentsTask();

            // Act
            task.Execute(taskContext);

            // Assert
            var paymentsMade = TestDataHelper.GetPaymentsForCommitment(commitmentId);
            Assert.IsNotNull(paymentsMade);
            Assert.AreEqual(1, paymentsMade.Length);

            Assert.AreEqual((int)FundingSource.Levy, paymentsMade[0].Source);
            Assert.AreEqual((int)TransactionType.Learning, paymentsMade[0].TransactionType);
            Assert.AreEqual(1000m, paymentsMade[0].Amount);
        }

        [Test]
        public void ThenThereShouldBeACompletionPaymentWhereLearningIsComplete()
        {
            // Arrange
            var accountId = Guid.NewGuid().ToString();
            TestDataHelper.AddAccount(accountId);

            var commitmentId = Guid.NewGuid().ToString();
            TestDataHelper.AddCommitment(commitmentId, accountId);

            var startDate = new DateTime(2017, 4, 1);
            var endDate = startDate.AddYears(1);
            TestDataHelper.AddPaymentDueForCommitment(commitmentId, currentPeriod: 12, startDate: startDate, endDate: endDate, actualEndDate: endDate);

            var taskContext = new IntegrationTaskContext();
            var task = new LevyPaymentsTask();

            // Act
            task.Execute(taskContext);

            // Assert
            var paymentsMade = TestDataHelper.GetPaymentsForCommitment(commitmentId);
            Assert.IsNotNull(paymentsMade);
            Assert.AreEqual(1, paymentsMade.Length);

            Assert.AreEqual((int)FundingSource.Levy, paymentsMade[0].Source);
            Assert.AreEqual((int)TransactionType.Completion, paymentsMade[0].TransactionType);
            Assert.AreEqual(3000m, paymentsMade[0].Amount);
        }

        [Test]
        public void ThenThereShouldBeACompletionPaymentAndALearningPaymentWhereLearningIsCompleteOnTheCensusDate()
        {
            // Arrange
            var accountId = Guid.NewGuid().ToString();
            TestDataHelper.AddAccount(accountId);

            var commitmentId = Guid.NewGuid().ToString();
            TestDataHelper.AddCommitment(commitmentId, accountId);

            var startDate = new DateTime(2017, 4, 1);
            var endDate = new DateTime(2018, 5, 31);
            TestDataHelper.AddPaymentDueForCommitment(commitmentId, currentPeriod: 12, startDate: startDate, endDate: endDate, actualEndDate: endDate);

            var taskContext = new IntegrationTaskContext();
            var task = new LevyPaymentsTask();

            // Act
            task.Execute(taskContext);

            // Assert
            var paymentsMade = TestDataHelper.GetPaymentsForCommitment(commitmentId);
            Assert.IsNotNull(paymentsMade);
            Assert.AreEqual(2, paymentsMade.Length);

            Assert.IsTrue(paymentsMade.Any(p => p.Source == (int) FundingSource.Levy
                                                && p.TransactionType == (int) TransactionType.Learning
                                                && p.Amount == 1000m));

            Assert.IsTrue(paymentsMade.Any(p => p.Source == (int)FundingSource.Levy
                                                && p.TransactionType == (int)TransactionType.Completion
                                                && p.Amount == 3000m));
        }

    }
}
