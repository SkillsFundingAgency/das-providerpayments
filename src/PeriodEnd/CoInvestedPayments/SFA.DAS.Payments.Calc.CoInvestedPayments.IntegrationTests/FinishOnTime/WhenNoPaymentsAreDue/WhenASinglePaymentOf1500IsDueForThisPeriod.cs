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
    public class WhenASinglePaymentOf1500IsDueForThisPeriod
    {
        private CoInvestedPaymentsTask _uut;
        private IntegrationTaskContext _taskContext;
        private long _commitmentId;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            var accountId = 3454356;
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
}
