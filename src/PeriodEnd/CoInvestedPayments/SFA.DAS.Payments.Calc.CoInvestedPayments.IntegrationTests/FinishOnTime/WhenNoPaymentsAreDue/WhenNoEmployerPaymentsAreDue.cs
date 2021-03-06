﻿using NUnit.Framework;
using SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests.Tools;
using SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests.Utilities;
using SFA.DAS.Payments.DCFS.Domain;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests.FinishOnTime.WhenNoPaymentsAreDue
{
    public class WhenNoEmployerPaymentsAreDue
    {
        private CoInvestedPaymentsTask _uut;
        private IntegrationTaskContext _taskContext;
        private long _commitmentId;


        [SetUp]
        public void Arrange()
        {

            TestDataHelper.Clean();

            const int accountId = 987767;
            TestDataHelper.AddAccount(accountId);

            _commitmentId = 1L;
            TestDataHelper.AddCommitment(_commitmentId, accountId);

            TestDataHelper.AddPaymentDueForProvider(_commitmentId, 1, amountDue: 1500, transactionType: TransactionType.Learning, sfaContributionPercentage: 1.0m);

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
            Assert.AreEqual(1, paymentsCount);
        }

        private void Act()
        {
            _uut.Execute(_taskContext);
        }
    }
}
