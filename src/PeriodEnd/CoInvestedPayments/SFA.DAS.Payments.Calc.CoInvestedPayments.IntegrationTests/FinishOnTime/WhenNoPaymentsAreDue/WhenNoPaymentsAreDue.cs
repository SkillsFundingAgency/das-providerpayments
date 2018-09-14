using NUnit.Framework;
using SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests.Tools;
using SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests.Utilities;

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
            TestDataHelper.CopyReferenceData();

            const int accountId = 768554;
            TestDataHelper.AddAccount(accountId);

            const long commitmentId = 1L;
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
