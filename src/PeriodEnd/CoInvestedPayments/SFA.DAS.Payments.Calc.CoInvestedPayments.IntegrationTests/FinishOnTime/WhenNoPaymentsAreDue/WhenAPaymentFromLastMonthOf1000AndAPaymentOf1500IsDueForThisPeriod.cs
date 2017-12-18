using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests.Tools;
using SFA.DAS.Payments.DCFS.Domain;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests.FinishOnTime.WhenNoPaymentsAreDue
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

            var accountId = 43656457;
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
}
