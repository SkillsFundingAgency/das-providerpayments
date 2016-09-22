using System;
using NUnit.Framework;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.PaymentsDue;
using SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests.Tools;
using SFA.DAS.ProviderPayments.Calc.Common.Application;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests.FinishOnTime
{
    public class WhenPaymentsAreDueForThisPeriod
    {
        private static readonly object[] PaymentsDue =
        {
            new PaymentDue
            {

                
            } 
        };

        private CoInvestedPaymentsTask _uut;
        private IntegrationTaskContext _taskContext;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            var accountId = Guid.NewGuid().ToString();
            TestDataHelper.AddAccount(accountId);

            var commitmentId = Guid.NewGuid().ToString();
            TestDataHelper.AddCommitment(commitmentId, accountId);

            TestDataHelper.AddPaymentDueForProvider(commitmentId, 1,  amountDue: 1000, transactionType: TransactionType.Learning);

            _taskContext = new IntegrationTaskContext();
            _uut = new CoInvestedPaymentsTask();
        }

        [Test]
        public void TheTwoPaymentsAreMade()
        {
            Act();

            // Assert
            var paymentsCount = TestDataHelper.GetPaymentsCount();
            Assert.IsNotNull(paymentsCount);
            Assert.AreEqual(2, paymentsCount);
        }

        private void Act()
        {
            _uut.Execute(_taskContext);
        }

        //[Test]
        //[TestCaseSource(nameof(PaymentsDue))]
        //public void ThenASingleLevyPaymentIsMadeWhenASinglePaymentIsDue(TransactionType transactionType, decimal amountDue)
        //{
        //    // Arrange
        //    var accountId = Guid.NewGuid().ToString();
        //    TestDataHelper.AddAccount(accountId);

        //    var commitmentId = Guid.NewGuid().ToString();
        //    TestDataHelper.AddCommitment(commitmentId, accountId);

        //    TestDataHelper.AddPaymentDueForCommitment(commitmentId, amountDue: amountDue, transactionType: transactionType);

        //    var taskContext = new IntegrationTaskContext();
        //    var task = new CoInvestedPaymentsTask();

        //    // Act
        //    task.Execute(taskContext);

        //    // Assert
        //    var paymentsMade = TestDataHelper.GetPaymentsForCommitment(commitmentId);
        //    Assert.IsNotNull(paymentsMade);
        //    Assert.AreEqual(1, paymentsMade.Length);

        //    Assert.AreEqual((int)FundingSource.Levy, paymentsMade[0].Source);
        //    Assert.AreEqual((int)transactionType, paymentsMade[0].TransactionType);
        //    Assert.AreEqual(amountDue, paymentsMade[0].Amount);
        //}

        //[Test]
        //public void ThenMultipleLevyPaymentsAreMadeWhenMultiplePaymentsAreDue()
        //{
        //    // Arrange
        //    var accountId = Guid.NewGuid().ToString();
        //    TestDataHelper.AddAccount(accountId);

        //    var commitmentId = Guid.NewGuid().ToString();
        //    TestDataHelper.AddCommitment(commitmentId, accountId);

        //    TestDataHelper.AddPaymentDueForCommitment(commitmentId, amountDue: 575.00m);
        //    TestDataHelper.AddPaymentDueForCommitment(commitmentId, amountDue: 1725.00m, transactionType: TransactionType.Completion);

        //    var taskContext = new IntegrationTaskContext();
        //    var task = new CoInvestedPaymentsTask();

        //    // Act
        //    task.Execute(taskContext);

        //    // Assert
        //    var paymentsMade = TestDataHelper.GetPaymentsForCommitment(commitmentId);
        //    Assert.IsNotNull(paymentsMade);
        //    Assert.AreEqual(2, paymentsMade.Length);

        //    Assert.AreEqual(1, paymentsMade.Count(p => p.Source == (int)FundingSource.Levy && p.TransactionType == (int)TransactionType.Learning && p.Amount == 575.00m));
        //    Assert.AreEqual(1, paymentsMade.Count(p => p.Source == (int)FundingSource.Levy && p.TransactionType == (int)TransactionType.Completion && p.Amount == 1725.00m));
        //}
    }
    public class WhenNoPaymentsAreDue
    {
        private static readonly object[] PaymentsDue =
        {
        };

        private CoInvestedPaymentsTask _uut;
        private IntegrationTaskContext _taskContext;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            var accountId = Guid.NewGuid().ToString();
            TestDataHelper.AddAccount(accountId);

            var commitmentId = Guid.NewGuid().ToString();
            TestDataHelper.AddCommitment(commitmentId, accountId);

            _taskContext = new IntegrationTaskContext();
            _uut = new CoInvestedPaymentsTask();
        }

        [Test]
        public void ThenNoPaymentsAreMade()
        {
            Act();

            // Assert
            var paymentsCount = TestDataHelper.GetPaymentsCount();
            Assert.IsNotNull(paymentsCount);
            Assert.AreEqual(0, paymentsCount);
        }

        private void Act()
        {
            _uut.Execute(_taskContext);
        }
    }
}
