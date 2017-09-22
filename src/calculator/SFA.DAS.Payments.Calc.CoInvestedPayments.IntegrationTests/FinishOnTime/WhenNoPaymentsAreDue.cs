﻿using System;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests.Tools;
using SFA.DAS.Payments.DCFS.Domain;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests.FinishOnTime
{
    public class WhenAPaymentFromLastMonthOf1000AndAPaymentOf1500IsDueForThisPeriod
    {
        private CoInvestedPaymentsTask _uut;
        private IntegrationTaskContext _taskContext;
        private long _commitmentId;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            var accountId = Guid.NewGuid().ToString();
            TestDataHelper.AddAccount(accountId);

            _commitmentId = 1L;
            TestDataHelper.AddCommitment(_commitmentId, accountId);

            TestDataHelper.AddPaymentDueForProvider(_commitmentId, 1, amountDue: 1500, transactionType: TransactionType.Learning);
            TestDataHelper.AddPaymentDueForProvider(_commitmentId, 1, amountDue: 1000, transactionType: TransactionType.Learning);

            TestDataHelper.CopyReferenceData();

            _taskContext = new IntegrationTaskContext();
            _uut = new CoInvestedPaymentsTask();
        }

        [Test]
        public void TheFourPaymentsAreMade()
        {
            Act();

            var paymentsCount = TestDataHelper.GetPaymentsCount();
            Assert.IsNotNull(paymentsCount);
            Assert.AreEqual(4, paymentsCount);
        }
        [Test]
        public void ThenACoInvestedSfaPaymentOf1350IsMade()
        {
            Act();

            var payments = TestDataHelper.GetPaymentsForCommitment(_commitmentId);
            Assert.IsNotNull(payments);
            Assert.IsNotNull(
                payments.SingleOrDefault(p => p.FundingSource == (int)FundingSource.CoInvestedSfa && p.Amount == 1350));
        }
        [Test]
        public void ThenACoInvestedEmployerPaymentOf150IsMade()
        {
            Act();

            var payments = TestDataHelper.GetPaymentsForCommitment(_commitmentId);
            Assert.IsNotNull(
                payments.SingleOrDefault(p => p.FundingSource == (int)FundingSource.CoInvestedEmployer && p.Amount == 150));
        }
        [Test]
        public void ThenACoInvestedSfaPaymentOf900IsMade()
        {
            Act();

            var payments = TestDataHelper.GetPaymentsForCommitment(_commitmentId);
            Assert.IsNotNull(payments);
            Assert.IsNotNull(
                payments.SingleOrDefault(p => p.FundingSource == (int)FundingSource.CoInvestedSfa && p.Amount == 900));
        }
        [Test]
        public void ThenACoInvestedEmployerPaymentOf100IsMade()
        {
            Act();

            var payments = TestDataHelper.GetPaymentsForCommitment(_commitmentId);
            Assert.IsNotNull(
                payments.SingleOrDefault(p => p.FundingSource == (int)FundingSource.CoInvestedEmployer && p.Amount == 100));
        }

        private void Act()
        {
            _uut.Execute(_taskContext);
        }
    }
    public class WhenASinglePaymentOf1500IsDueForThisPeriod
    {
        private CoInvestedPaymentsTask _uut;
        private IntegrationTaskContext _taskContext;
        private long _commitmentId;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            var accountId = Guid.NewGuid().ToString();
            TestDataHelper.AddAccount(accountId);

            _commitmentId = 1L;
            TestDataHelper.AddCommitment(_commitmentId, accountId);

            TestDataHelper.AddPaymentDueForProvider(_commitmentId, 1, amountDue: 1500, transactionType: TransactionType.Learning);

            TestDataHelper.CopyReferenceData();

            _taskContext = new IntegrationTaskContext();
            _uut = new CoInvestedPaymentsTask();
        }

        [Test]
        public void TheTwoPaymentsAreMade()
        {
            Act();

            var paymentsCount = TestDataHelper.GetPaymentsCount();
            Assert.IsNotNull(paymentsCount);
            Assert.AreEqual(2, paymentsCount);
        }
        [Test]
        public void ThenACoInvestedSfaPaymentOf1350IsMade()
        {
            Act();

            var payments = TestDataHelper.GetPaymentsForCommitment(_commitmentId);
            Assert.IsNotNull(payments);
            Assert.IsNotNull(
                payments.SingleOrDefault(p => p.FundingSource == (int)FundingSource.CoInvestedSfa && p.Amount == 1350));
        }
        [Test]
        public void ThenACoInvestedEmployerPaymentOf150IsMade()
        {
            Act();

            var payments = TestDataHelper.GetPaymentsForCommitment(_commitmentId);
            Assert.IsNotNull(
                payments.SingleOrDefault(p => p.FundingSource == (int)FundingSource.CoInvestedEmployer && p.Amount == 150));
        }

        private void Act()
        {
            _uut.Execute(_taskContext);
        }
    }
    public class WhenNoPaymentsAreDue
    {
        private CoInvestedPaymentsTask _uut;
        private IntegrationTaskContext _taskContext;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            var accountId = Guid.NewGuid().ToString();
            TestDataHelper.AddAccount(accountId);

            var commitmentId = 1L;
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
    public class WhenASinglePaymentOf1500IsDueForThisPeriodAndTheSfaContributionPercentageVaries
    {
        private CoInvestedPaymentsTask _uut;
        private IntegrationTaskContext _taskContext;
        private long _commitmentId;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            var accountId = Guid.NewGuid().ToString();
            TestDataHelper.AddAccount(accountId);

            _commitmentId = 1L;
            TestDataHelper.AddCommitment(_commitmentId, accountId);

            _taskContext = new IntegrationTaskContext();
            _uut = new CoInvestedPaymentsTask();
        }

        [Test]
        [TestCase(0.9)]
        [TestCase(0.75)]
        [TestCase(1.00)]
        [TestCase(0.00)]
        public void ThenACoInvestedEmployerPaymentOf150IsMade(decimal sfaContributionPercentage)
        {
            // Arrange
            TestDataHelper.AddPaymentDueForProvider(_commitmentId, 1, amountDue: 1500, sfaContributionPercentage: sfaContributionPercentage);

            TestDataHelper.CopyReferenceData();

            // Act
            _uut.Execute(_taskContext);

            // Assert
            var sfaPayment = 1500m * sfaContributionPercentage;
            var employerPayment = 1500m * (1 - sfaContributionPercentage);

            var payments = TestDataHelper.GetPaymentsForCommitment(_commitmentId);

            if (sfaPayment == 0)
            {
                Assert.IsNull(
                payments.SingleOrDefault(p => p.FundingSource == (int)FundingSource.CoInvestedSfa && p.Amount == sfaPayment));
            }
            else
            {
                Assert.IsNotNull(
                payments.SingleOrDefault(p => p.FundingSource == (int)FundingSource.CoInvestedSfa && p.Amount == sfaPayment));
            }

            if (employerPayment == 0)
            {
                Assert.IsNull(
                payments.SingleOrDefault(p => p.FundingSource == (int)FundingSource.CoInvestedEmployer && p.Amount == employerPayment));
            }
            else
            {
                Assert.IsNotNull(
                    payments.SingleOrDefault(p => p.FundingSource == (int)FundingSource.CoInvestedEmployer && p.Amount == employerPayment));
            }
        }
    }

    public class WhenNoEmployerPaymentsAreDue
    {
        private CoInvestedPaymentsTask _uut;
        private IntegrationTaskContext _taskContext;
        private long _commitmentId;


        [SetUp]
        public void Arrange()
        {

            TestDataHelper.Clean();

            var accountId = Guid.NewGuid().ToString();
            TestDataHelper.AddAccount(accountId);

            _commitmentId = 1L;
            TestDataHelper.AddCommitment(_commitmentId, accountId);

            TestDataHelper.AddPaymentDueForProvider(_commitmentId, 1, amountDue: 1500, transactionType: TransactionType.Learning,sfaContributionPercentage:1.0m);

            TestDataHelper.CopyReferenceData();

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
            Assert.AreEqual(1, paymentsCount);
        }

        private void Act()
        {
            _uut.Execute(_taskContext);
        }
    }

    public class WhenNoPaymentsAreDueBecauseManualAdjustmentsProcessedTransaction
    {
        private CoInvestedPaymentsTask _uut;
        private IntegrationTaskContext _taskContext;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();


            var requiredPaymentId = Guid.NewGuid().ToString();
            TestDataHelper.AddPaymentDueForNonDas(1,100, amountDue: 1500, transactionType: TransactionType.Learning, sfaContributionPercentage: 1.0m,requiredPaymentId:requiredPaymentId);

            TestDataHelper.AddRequiredPaymentForReversal(requiredPaymentId);

            TestDataHelper.CopyReferenceData();
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
