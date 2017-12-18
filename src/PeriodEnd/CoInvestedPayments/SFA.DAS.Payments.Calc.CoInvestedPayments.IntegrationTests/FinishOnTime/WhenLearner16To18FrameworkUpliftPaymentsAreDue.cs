using SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests.Tools;
using System;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Domain;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests.FinishOnTime
{
    public class WhenLearner16To18FrameworkUpliftPaymentsAreDue
    {
        private CoInvestedPaymentsTask _uut;
        private IntegrationTaskContext _taskContext;
        private long _commitmentId;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            var accountId = 235434;
            TestDataHelper.AddAccount(accountId);

            _commitmentId = 1L;
            TestDataHelper.AddCommitment(_commitmentId, accountId);

            TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, deliveryMonth: 11, deliveryYear: 17, amountDue: 1000, transactionType: TransactionType.Learning);
            TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, deliveryMonth: 11, deliveryYear: 17, amountDue: 120, transactionType:TransactionType.OnProgramme16To18FrameworkUplift);


            TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, deliveryMonth: 7, deliveryYear: 18, amountDue: 2000, transactionType: TransactionType.Completion);
            TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, deliveryMonth: 7, deliveryYear: 18, amountDue: 240, transactionType: TransactionType.Balancing16To18FrameworkUplift);
            TestDataHelper.AddPaymentDueForProvider2(_commitmentId, 1, deliveryMonth: 7, deliveryYear: 18, amountDue: 360, transactionType: TransactionType.Completion16To18FrameworkUplift);
            
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
            Assert.AreEqual(incentivePayments.Count(),3);

            Assert.NotNull(incentivePayments.Single(p => p.TransactionType == (int)TransactionType.OnProgramme16To18FrameworkUplift && p.Amount == 120));

            Assert.NotNull(incentivePayments.Single(p => p.TransactionType == (int)TransactionType.Balancing16To18FrameworkUplift && p.Amount == 240));
            Assert.NotNull(incentivePayments.Single(p => p.TransactionType == (int)TransactionType.Completion16To18FrameworkUplift && p.Amount == 360));
        }

        private void Act()
        {
            _uut.Execute(_taskContext);
        }
    }
}
