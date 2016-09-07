using System;
using NUnit.Framework;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Payments;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.IntegrationTests.FinishOnTime
{
    public class WhenThereIsEnoughLevyInTheAccount
    {
        private string _accountId;
        private string _commitmentId;

        [SetUp]
        public void Arrange()
        {
            _accountId = Guid.NewGuid().ToString();
            TestDataHelper.AddAccount(_accountId);

            _commitmentId = Guid.NewGuid().ToString();
            TestDataHelper.AddCommitment(_commitmentId, _accountId);

            TestDataHelper.AddEarningForCommitment(_commitmentId);
        }

        [Test]
        public void ThenASinglePaymentOfLevyIsMadeForTheEarnings()
        {
            // Arrange
            var taskContext = new IntegrationTaskContext();
            var task = new LevyPaymentsTask();

            // Act
            task.Execute(taskContext);

            // Assert
            var paymentsMade = TestDataHelper.GetPaymentsForCommitment(_commitmentId);
            Assert.IsNotNull(paymentsMade);
            Assert.AreEqual(1, paymentsMade.Length);

            Assert.AreEqual((int)FundingSource.Levy, paymentsMade[0].Source);
            Assert.AreEqual(1000m, paymentsMade[0].Amount);
        }
    }
}
