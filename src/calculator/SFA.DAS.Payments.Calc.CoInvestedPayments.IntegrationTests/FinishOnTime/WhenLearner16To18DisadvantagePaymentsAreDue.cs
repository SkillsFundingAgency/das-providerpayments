﻿using SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests.Tools;
using System;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Domain;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests.FinishOnTime
{
    public class WhenLearner16To18DisadvantagePaymentsAreDue
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

            TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, deliveryMonth: 11, deliveryYear: 17, amountDue: 1000, transactionType: TransactionType.Learning);
            TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, deliveryMonth: 11, deliveryYear: 17, amountDue: 500, transactionType:TransactionType.FirstDisadvantagePayment);


            TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, deliveryMonth: 7, deliveryYear: 18, amountDue: 1000, transactionType: TransactionType.Learning);
            TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, deliveryMonth: 7, deliveryYear: 18, amountDue: 500, transactionType: TransactionType.SecondDisadvantagePayment);
            
            TestDataHelper.CopyReferenceData();

            _taskContext = new IntegrationTaskContext();
            _uut = new CoInvestedPaymentsTask();
        }

       
        [Test]
        public void ThenResultShouldBe_()
        {
            Act();

            var payments = TestDataHelper.GetPaymentsForCommitment(_commitmentId);
            var incentivePayments = payments.Where(x => x.FundingSource ==(int) FundingSource.FullyFundedSfa);

            Assert.IsNotNull(incentivePayments);
            Assert.AreEqual(incentivePayments.Count(),2);

            Assert.NotNull(incentivePayments.Single(p => p.TransactionType == (int)TransactionType.FirstDisadvantagePayment && p.Amount == 500));

            Assert.NotNull(incentivePayments.Single(p => p.TransactionType == (int)TransactionType.SecondDisadvantagePayment && p.Amount == 500));
        }

        private void Act()
        {
            _uut.Execute(_taskContext);
        }
    }
}