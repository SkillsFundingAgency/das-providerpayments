using NUnit.Framework;
using System;
using SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests.Tools;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests.FinishOnTime.WhenNoPaymentsAreDue
{
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

            const long commitmentId = 1L;
            TestDataHelper.AddCommitment(commitmentId, accountId);

            _taskContext = new IntegrationTaskContext();
            _uut = new CoInvestedPaymentsTask();

            TestDataHelper.CopyReferenceData();
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
