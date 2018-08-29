using System.Linq;
using NUnit.Framework;
using SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests.Tools;
using SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests.Utilities;
using SFA.DAS.Payments.DCFS.Domain;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests.FinishOnTime
{
    public class WhenDpp195AcceptanceScenario2
    {
        private CoInvestedPaymentsTask _uut;
        private IntegrationTaskContext _taskContext;
        private long _commitmentId;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            const int accountId = 423445;
            TestDataHelper.AddAccount(accountId);

            _commitmentId = 1L;
            TestDataHelper.AddCommitment(_commitmentId, accountId);

            TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, 9, 17, amountDue: 923.07692m);
            TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, 10, 17, amountDue: 923.07692m);
            TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, 11, 17, amountDue: 923.07692m);
            TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, 12, 17, amountDue: 923.07692m);
            TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, 1, 18, amountDue: 923.07692m);
            TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, 2, 18, amountDue: 923.07692m);
            TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, 3, 18, amountDue: 923.07692m);
            TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, 4, 18, amountDue: 923.07692m);
            TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, 5, 18, amountDue: 923.07692m);
            TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, 6, 18, amountDue: 923.07692m);
            TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, 7, 18, amountDue: 923.07692m);
            TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, 8, 18, amountDue: 923.07692m);
            TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, 9, 18, amountDue: 3923.07692m);

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
            Assert.AreEqual(12, payments.Count(p => p.FundingSource == (int)FundingSource.CoInvestedSfa && p.Amount == 830.76923m));
            Assert.AreEqual(12, payments.Count(p => p.FundingSource == (int)FundingSource.CoInvestedEmployer && p.Amount == 92.30769m));
            Assert.AreEqual(1, payments.Count(p => p.FundingSource == (int)FundingSource.CoInvestedSfa && p.Amount == 3530.76923m));
            Assert.AreEqual(1, payments.Count(p => p.FundingSource == (int)FundingSource.CoInvestedEmployer && p.Amount == 392.30769m));
        }

        private void Act()
        {
            _uut.Execute(_taskContext);
        }
    }

}
