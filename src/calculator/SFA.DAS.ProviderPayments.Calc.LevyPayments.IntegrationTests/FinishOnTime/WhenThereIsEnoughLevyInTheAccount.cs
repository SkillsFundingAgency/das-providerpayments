using System;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calc.Common.Application;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.IntegrationTests.Tools;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.IntegrationTests.FinishOnTime
{
    public class WhenThereIsEnoughLevyInTheAccount
    {
        private static readonly object[] PaymentsDue =
        {
            new object[] {TransactionType.Learning, 500.00m},
            new object[] {TransactionType.Learning, 500.12345m},
            new object[] {TransactionType.Completion, 1500.00m},
            new object[] {TransactionType.Completion, 1500.54321m}
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
            var accountId = 1;
            TestDataHelper.AddAccount(accountId);

            var commitmentId = 1L;
            TestDataHelper.AddCommitment(commitmentId, accountId.ToString());

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
            var accountId = 1;
            TestDataHelper.AddAccount(accountId);

            var commitmentId = 1L;
            TestDataHelper.AddCommitment(commitmentId, accountId.ToString());

            TestDataHelper.AddPaymentDueForCommitment(commitmentId, amountDue: amountDue, transactionType: transactionType);

            var taskContext = new IntegrationTaskContext();
            var task = new LevyPaymentsTask();

            // Act
            task.Execute(taskContext);

            // Assert
            var paymentsMade = TestDataHelper.GetPaymentsForCommitment(commitmentId);
            Assert.IsNotNull(paymentsMade);
            Assert.AreEqual(1, paymentsMade.Length);

            Assert.AreEqual((int)FundingSource.Levy, paymentsMade[0].FundingSource);
            Assert.AreEqual((int)transactionType, paymentsMade[0].TransactionType);
            Assert.AreEqual(amountDue, paymentsMade[0].Amount);
        }

        [Test]
        public void ThenMultipleLevyPaymentsAreMadeWhenMultiplePaymentsAreDue()
        {
            // Arrange
            var accountId = 1;
            TestDataHelper.AddAccount(accountId);

            var commitmentId = 1L;
            TestDataHelper.AddCommitment(commitmentId, accountId.ToString());

            TestDataHelper.AddPaymentDueForCommitment(commitmentId, amountDue: 575.12345m);
            TestDataHelper.AddPaymentDueForCommitment(commitmentId, amountDue: 1725.54321m, transactionType: TransactionType.Completion);

            var taskContext = new IntegrationTaskContext();
            var task = new LevyPaymentsTask();

            // Act
            task.Execute(taskContext);

            // Assert
            var paymentsMade = TestDataHelper.GetPaymentsForCommitment(commitmentId);
            Assert.IsNotNull(paymentsMade);
            Assert.AreEqual(2, paymentsMade.Length);

            Assert.AreEqual(1, paymentsMade.Count(p => p.FundingSource == (int)FundingSource.Levy && p.TransactionType == (int)TransactionType.Learning && p.Amount == 575.12345m));
            Assert.AreEqual(1, paymentsMade.Count(p => p.FundingSource == (int)FundingSource.Levy && p.TransactionType == (int)TransactionType.Completion && p.Amount == 1725.54321m));
        }
    }
}
