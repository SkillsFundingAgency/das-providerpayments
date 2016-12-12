using NUnit.Framework;
using SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests.FinishOnTime
{


    public class WhenLearnerIsNonDas
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
}
