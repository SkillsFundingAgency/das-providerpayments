using NUnit.Framework;
using SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests.Tools;
using SFA.DAS.Payments.DCFS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests.Utilities;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests.FinishOnTime.WhenNoPaymentsAreDue
{
    public class WhenNoPaymentsAreDueBecauseManualAdjustmentsProcessedTransaction
    {
        private CoInvestedPaymentsTask _uut;
        private IntegrationTaskContext _taskContext;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();


            var requiredPaymentId = Guid.NewGuid().ToString();
            TestDataHelper.AddPaymentDueForNonDas(1, 100, amountDue: 1500, transactionType: TransactionType.Learning, sfaContributionPercentage: 1.0m, requiredPaymentId: requiredPaymentId);

            TestDataHelper.AddRequiredPaymentForReversal(requiredPaymentId);

            TestDataHelper.CopyReferenceData();
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
