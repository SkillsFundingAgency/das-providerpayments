using System;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Payments;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.IntegrationTests.FinishOnTime
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

            TestDataHelper.AddEarningForCommitment(commitmentId);

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
            TestDataHelper.AddEarningForCommitment(commitmentId, currentPeriod: 12, startDate: startDate, endDate: endDate, actualEndDate: endDate);

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

    }
}
