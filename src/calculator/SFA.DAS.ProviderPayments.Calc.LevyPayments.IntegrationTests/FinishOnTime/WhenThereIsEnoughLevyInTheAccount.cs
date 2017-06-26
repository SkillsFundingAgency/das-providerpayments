using System.Linq;
using NUnit.Framework;

using SFA.DAS.ProviderPayments.Calc.LevyPayments.IntegrationTests.Tools;
using SFA.DAS.Payments.DCFS.Domain;
using System;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.IntegrationTests.FinishOnTime
{
    public class WhenThereIsEnoughLevyInTheAccount
    {
        private static readonly Random Random = new Random();

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

            TestDataHelper.CopyReferenceData();

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

            TestDataHelper.CopyReferenceData();

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

            TestDataHelper.CopyReferenceData();

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

        [Test]
        public void ThenMultipleLevyRefundPaymentsAreMadeWhenMultipleRefundsAreDue()
        {
            // Arrange
            var accountId = 1;
            var ukprn =  Random.Next(1, int.MaxValue); 
            TestDataHelper.AddAccount(accountId);
            TestDataHelper.AddProvider(ukprn);

            var commitmentId = 1L;
            TestDataHelper.AddCommitment(commitmentId, accountId.ToString(), ukprn: ukprn);

            TestDataHelper.CopyReferenceData();

            TestDataHelper.AddPaymentDueForCommitment(commitmentId, amountDue: -575.12345m);

            TestDataHelper.AddPaymentDueForCommitment(commitmentId, amountDue: -1725.54321m, transactionType: TransactionType.Completion);
            TestDataHelper.AddPaymentHistoryForCommitment(commitmentId);

            TestDataHelper.PopulatePaymentsHistory();
            var taskContext = new IntegrationTaskContext();
            var task = new LevyPaymentsTask();

            // Act
            task.Execute(taskContext);

            // Assert
            var paymentsMade = TestDataHelper.GetPaymentsForCommitment(commitmentId);
            Assert.IsNotNull(paymentsMade);
            Assert.AreEqual(2, paymentsMade.Length);

            Assert.AreEqual(1, paymentsMade.Count(p => p.FundingSource == (int)FundingSource.Levy && p.TransactionType == (int)TransactionType.Learning && p.Amount == -575.12345m));
            Assert.AreEqual(1, paymentsMade.Count(p => p.FundingSource == (int)FundingSource.Levy && p.TransactionType == (int)TransactionType.Completion && p.Amount == -1725.54321m));
        }

        [Test]
        public void ThenLevyRefundPaymentsAreMadeWhenPartiallyRefunding()
        {
            // Arrange
            var accountId = 1;
            var ukprn = Random.Next(1, int.MaxValue);
            TestDataHelper.AddAccount(accountId);
            TestDataHelper.AddProvider(ukprn);

            var commitmentId = 1L;
            TestDataHelper.AddCommitment(commitmentId, accountId.ToString(), ukprn: ukprn);

            TestDataHelper.CopyReferenceData();

            TestDataHelper.AddPaymentDueForCommitment(commitmentId, amountDue: -212.15326m);
            TestDataHelper.AddPaymentHistoryForCommitment(commitmentId);
            TestDataHelper.AddPaymentHistoryForCommitment(commitmentId, 8, 2016, -575.12345m);

            TestDataHelper.PopulatePaymentsHistory();
            var taskContext = new IntegrationTaskContext();
            var task = new LevyPaymentsTask();

            // Act
            task.Execute(taskContext);

            // Assert
            var paymentsMade = TestDataHelper.GetPaymentsForCommitment(commitmentId);
            Assert.IsNotNull(paymentsMade);
            Assert.AreEqual(1, paymentsMade.Length);

            var actualPaymentsMessage = "actually paid:\n" + paymentsMade.Select(x => x.Amount.ToString() + " " + ((FundingSource)x.FundingSource).ToString()).Aggregate((x, y) => $"{x}\n{y}");
            Assert.AreEqual(1, paymentsMade.Count(p => p.FundingSource == (int)FundingSource.Levy && p.TransactionType == (int)TransactionType.Learning && p.Amount == -212.15326m), $"Expected -212.15326 Levy, {actualPaymentsMessage}");
        }


        [Test]
        public void ThenRefundPaymentsShouldbeMadeFirstBeforeProcessingPayments()
        {
            // Arrange
            var accountId = 1;
            var ukprn1 = Random.Next(1, int.MaxValue);
            var ukprn2 = Random.Next(1, int.MaxValue);

            TestDataHelper.AddAccount(accountId,balance:1150m);

            TestDataHelper.AddProvider(ukprn1);
            TestDataHelper.AddProvider(ukprn2);

            var commitmentId1 = 1L;
            var commitmentId2 = 2L;

            TestDataHelper.AddCommitment(commitmentId1, accountId.ToString(), ukprn: ukprn1);
            TestDataHelper.AddCommitment(commitmentId2, accountId.ToString(), ukprn: ukprn2);

            TestDataHelper.CopyReferenceData();

            TestDataHelper.AddPaymentDueForCommitment(commitmentId1, amountDue: -575m);
            TestDataHelper.AddPaymentHistoryForCommitment(commitmentId1);
            TestDataHelper.PopulatePaymentsHistory();

            TestDataHelper.AddPaymentDueForCommitment(commitmentId2, amountDue: 1725m, transactionType: TransactionType.Completion);
            
            var taskContext = new IntegrationTaskContext();
            var task = new LevyPaymentsTask();

            // Act
            task.Execute(taskContext);

            // Assert
            var refundsMade = TestDataHelper.GetPaymentsForCommitment(commitmentId1);
            Assert.IsNotNull(refundsMade);
            Assert.AreEqual(1, refundsMade.Length);
            Assert.AreEqual(1, refundsMade.Count(p => p.FundingSource == (int)FundingSource.Levy && p.TransactionType == (int)TransactionType.Learning && p.Amount == -575m));

            var paymentsMade = TestDataHelper.GetPaymentsForCommitment(commitmentId2);
            Assert.IsNotNull(paymentsMade);
            Assert.AreEqual(1, paymentsMade.Length);
            Assert.AreEqual(1, paymentsMade.Count(p => p.FundingSource == (int)FundingSource.Levy && p.TransactionType == (int)TransactionType.Completion && p.Amount == 1725m));

            var accountBalance = TestDataHelper.GetAccountBalance(accountId);
            Assert.IsNotNull(accountBalance);
            Assert.AreEqual(1150, accountBalance[0]);
        }

    }
}
