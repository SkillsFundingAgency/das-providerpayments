using System.Linq;
using NUnit.Framework;
using SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests.Tools;
using SFA.DAS.Payments.DCFS.Domain;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests.FinishOnTime
{
    public class WhenDpp195AcceptanceScenario1
    {
        private CoInvestedPaymentsTask _uut;
        private IntegrationTaskContext _taskContext;
        private long _commitmentId;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            const int accountId = 342543;
            TestDataHelper.AddAccount(accountId);

            _commitmentId = 1L;
            TestDataHelper.AddCommitment(_commitmentId, accountId);

            TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, 10, 17);
            TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, 11, 17);
            TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, 12, 17);
            TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, 1, 18);
            TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, 2, 18);
            TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, 3, 18);
            TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, 4, 18);
            TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, 5, 18);
            TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, 6, 18);
            TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, 7, 18);
            TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, 8, 18);
            TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, 9, 18);
            TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, 10, 18, amountDue: 3000);

            TestDataHelper.CopyReferenceData();

            _taskContext = new IntegrationTaskContext();
            _uut = new CoInvestedPaymentsTask();
        }

        [Test]
        public void The26PaymentsAreMade()
        {
            Act();

            var paymentsCount = TestDataHelper.GetPaymentsCount();
            Assert.IsNotNull(paymentsCount);
            Assert.AreEqual(26, paymentsCount);
        }
        [Test]
        public void ThenResultShouldBe_()
        {
            Act();

            var payments = TestDataHelper.GetPaymentsForCommitment(_commitmentId);
            Assert.IsNotNull(payments);
            Assert.AreEqual(12, payments.Count(p => p.FundingSource == (int)FundingSource.CoInvestedSfa && p.Amount == 900));
            Assert.AreEqual(12, payments.Count(p => p.FundingSource == (int)FundingSource.CoInvestedEmployer && p.Amount == 100));
            Assert.AreEqual(1, payments.Count(p => p.FundingSource == (int)FundingSource.CoInvestedSfa && p.Amount == 2700));
            Assert.AreEqual(1, payments.Count(p => p.FundingSource == (int)FundingSource.CoInvestedEmployer && p.Amount == 300));
        }

        private void Act()
        {
            _uut.Execute(_taskContext);
        }
    }
}