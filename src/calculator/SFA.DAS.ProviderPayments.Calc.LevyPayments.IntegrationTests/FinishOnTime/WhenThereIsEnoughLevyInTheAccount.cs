using System;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.Common.Application;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Payments;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.IntegrationTests.Tools;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.IntegrationTests.FinishOnTime
{
    public class WhenThereIsEnoughLevyInTheAccount
    {
        private static readonly object[] PaymentsDue =
        {
            new object[] {TransactionType.Learning, 500.00m},
            new object[] {TransactionType.Completion, 1500.00m}
        };

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();
        }

        [Test]
        public void ThenNoLevyPaymentsAreMadeWhenNoPaymentsAreDue()
        {
            // Arrange
            var accountId = Guid.NewGuid().ToString();
            TestDataHelper.AddAccount(accountId);

            var commitmentId = Guid.NewGuid().ToString();
            TestDataHelper.AddCommitment(commitmentId, accountId);

            var taskContext = new IntegrationTaskContext();
            var task = new LevyPaymentsTask();

            // Act
            task.Execute(taskContext);

            // Assert
            var paymentsMade = TestDataHelper.GetPaymentsForCommitment(commitmentId);
            Assert.IsNotNull(paymentsMade);
            Assert.AreEqual(0, paymentsMade.Length);
        }

        [Test]
        [TestCaseSource(nameof(PaymentsDue))]
        public void ThenASingleLevyPaymentIsMadeWhenASinglePaymentIsDue(TransactionType transactionType, decimal amountDue)
        {
            // Arrange
            var accountId = Guid.NewGuid().ToString();
            TestDataHelper.AddAccount(accountId);

            var commitmentId = Guid.NewGuid().ToString();
            TestDataHelper.AddCommitment(commitmentId, accountId);

            TestDataHelper.AddPaymentDueForCommitment(commitmentId, amountDue: amountDue, transactionType: transactionType);

            var taskContext = new IntegrationTaskContext();
            var task = new LevyPaymentsTask();

            // Act
            task.Execute(taskContext);

            // Assert
            var paymentsMade = TestDataHelper.GetPaymentsForCommitment(commitmentId);
            Assert.IsNotNull(paymentsMade);
            Assert.AreEqual(1, paymentsMade.Length);

            Assert.AreEqual((int)FundingSource.Levy, paymentsMade[0].Source);
            Assert.AreEqual((int)transactionType, paymentsMade[0].TransactionType);
            Assert.AreEqual(amountDue, paymentsMade[0].Amount);
        }

        [Test]
        public void ThenMultipleLevyPaymentsAreMadeWhenMultiplePaymentsAreDue()
        {
            // Arrange
            var accountId = Guid.NewGuid().ToString();
            TestDataHelper.AddAccount(accountId);

            var commitmentId = Guid.NewGuid().ToString();
            TestDataHelper.AddCommitment(commitmentId, accountId);

            TestDataHelper.AddPaymentDueForCommitment(commitmentId, amountDue: 575.00m);
            TestDataHelper.AddPaymentDueForCommitment(commitmentId, amountDue: 1725.00m, transactionType: TransactionType.Completion);

            var taskContext = new IntegrationTaskContext();
            var task = new LevyPaymentsTask();

            // Act
            task.Execute(taskContext);

            // Assert
            var paymentsMade = TestDataHelper.GetPaymentsForCommitment(commitmentId);
            Assert.IsNotNull(paymentsMade);
            Assert.AreEqual(2, paymentsMade.Length);

            Assert.AreEqual(1, paymentsMade.Count(p => p.Source == (int)FundingSource.Levy && p.TransactionType == (int)TransactionType.Learning && p.Amount == 575.00m));
            Assert.AreEqual(1, paymentsMade.Count(p => p.Source == (int)FundingSource.Levy && p.TransactionType == (int)TransactionType.Completion && p.Amount == 1725.00m));
        }
    }
}
