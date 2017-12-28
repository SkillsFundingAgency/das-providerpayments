using System;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests.Tools;
using SFA.DAS.Payments.DCFS.Domain;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests.FinishOnTime
{
    public class WhenRefundPaymentsAreDue
    {
        private CoInvestedPaymentsTask _uut;
        private IntegrationTaskContext _taskContext;
        private long _commitmentId;
        private long _ukprn;
        private long _uln;

        private static readonly Random Random = new Random();

        private string _accountId;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            _accountId = Guid.NewGuid().ToString();
            TestDataHelper.AddAccount(_accountId);


            _commitmentId = 1L;
            _ukprn = Random.Next(1, int.MaxValue);
            _uln = Random.Next(1, int.MaxValue);


            TestDataHelper.AddCommitment(_commitmentId, _accountId, ukprn: _ukprn, uln: _uln);
            TestDataHelper.AddCommitment(_commitmentId, _accountId, ukprn: _ukprn, uln: _uln, isDeds: true);

            TestDataHelper.AddProvider(_ukprn);
            TestDataHelper.CopyReferenceData();

            _taskContext = new IntegrationTaskContext();
            _uut = new CoInvestedPaymentsTask();
        }


        [Test]
        public void ThenCoInvestedRefundPaymentsAreMadeWhenFullyRefunding()
        {
            // Arrange
            var requiredPaymentId = Guid.NewGuid().ToString();

            TestDataHelper.AddPaymentDueForProvider2(
                    _commitmentId,
                    _ukprn, 8, 2016,
                    amountDue: 1500,
                    transactionType: TransactionType.Learning,
                    requiredPaymentId: requiredPaymentId,
                    isDeds: true);

            TestDataHelper.AddPaymentHistoryForCommitment(requiredPaymentId, FundingSource.CoInvestedEmployer, 150, 8, 2016, TransactionType.Learning, true);
            TestDataHelper.AddPaymentHistoryForCommitment(requiredPaymentId, FundingSource.CoInvestedSfa, 1350, 8, 2016, TransactionType.Learning, true);
            TestDataHelper.AddPaymentHistoryForCommitment(requiredPaymentId, FundingSource.CoInvestedSfa, 350, 8, 2017, TransactionType.Learning, true);
            TestDataHelper.PopulatePaymentsHistory();

            TestDataHelper.AddPaymentDueForProvider(_commitmentId, _ukprn, amountDue: -1500);

            // Act
            _uut.Execute(_taskContext);

            //Assert
            var payments = TestDataHelper.GetPaymentsForCommitment(_commitmentId);
            Assert.IsTrue(payments.Length == 2);

            Assert.IsNotNull(payments.SingleOrDefault(p => p.FundingSource == (int)FundingSource.CoInvestedSfa && p.Amount == -1350));
            Assert.IsNotNull(payments.SingleOrDefault(p => p.FundingSource == (int)FundingSource.CoInvestedEmployer && p.Amount == -150));
        }

        [Test]
        public void ThenCoInvestedRefundPaymentsAreMadeWhenPartiallyRefunding()
        {
            // Arrange
            var requiredPaymentId = Guid.NewGuid().ToString();

            TestDataHelper.AddPaymentDueForProvider2(
                    _commitmentId,
                    _ukprn, 8, 2016,
                    amountDue: 1500,
                    transactionType: TransactionType.Learning,
                    requiredPaymentId: requiredPaymentId,
                    isDeds: true);

            TestDataHelper.AddPaymentHistoryForCommitment(requiredPaymentId, FundingSource.CoInvestedEmployer, 150, 8, 2016, TransactionType.Learning, true);
            TestDataHelper.AddPaymentHistoryForCommitment(requiredPaymentId, FundingSource.CoInvestedSfa, 1350, 8, 2016, TransactionType.Learning, true);
            TestDataHelper.AddPaymentHistoryForCommitment(requiredPaymentId, FundingSource.CoInvestedSfa, 350, 8, 2017, TransactionType.Learning, true);
            TestDataHelper.PopulatePaymentsHistory();

            TestDataHelper.AddPaymentDueForProvider(_commitmentId, _ukprn, amountDue: -750);

            // Act
            _uut.Execute(_taskContext);

            //Assert
            var payments = TestDataHelper.GetPaymentsForCommitment(_commitmentId);
            Assert.IsTrue(payments.Length == 2);

            var actualPaymentsMessage = "actually paid:\n" + payments.Select(x => x.Amount.ToString() + " " + ((FundingSource)x.FundingSource).ToString()).Aggregate((x, y) => $"{x}\n{y}");
            Assert.IsNotNull(payments.SingleOrDefault(p => p.FundingSource == (int)FundingSource.CoInvestedSfa && p.Amount == -675), $"Expected -675 CoInvestedSfa, {actualPaymentsMessage}");
            Assert.IsNotNull(payments.SingleOrDefault(p => p.FundingSource == (int)FundingSource.CoInvestedEmployer && p.Amount == -75), $"Expected -75 CoInvestedEmployer, {actualPaymentsMessage}");
        }

        [Test]
        public void ThenPaymentsMadeToATemporaryUlnAreMade()
        {
            // Arrange
            var requiredPaymentId = Guid.NewGuid().ToString();
            const long temporaryUln = 999999999L;
            var newUln = Random.Next(1, int.MaxValue);
            const string learnerRef = "LEARNERREF";
            const long temporaryCommitmentId = 2L;

            TestDataHelper.AddCommitment(temporaryCommitmentId, _accountId, ukprn: _ukprn, uln: temporaryUln);
            TestDataHelper.AddCommitment(temporaryCommitmentId, _accountId, ukprn: _ukprn, uln: temporaryUln, isDeds: true);

            TestDataHelper.AddPaymentDueForProvider2(
                temporaryCommitmentId,
                _ukprn, 8, 2016,
                amountDue: 1500,
                transactionType: TransactionType.Learning,
                requiredPaymentId: requiredPaymentId,
                learnerRefNumber: learnerRef,
                isDeds: true);

            TestDataHelper.AddPaymentHistoryForCommitment(requiredPaymentId, FundingSource.CoInvestedEmployer, 1500, 8, 2016, TransactionType.Learning, true);
            TestDataHelper.PopulatePaymentsHistory();

            TestDataHelper.AddPaymentDueForProvider(temporaryCommitmentId, _ukprn, amountDue: -1500, uln: newUln, learnerRefNumber:learnerRef);

            // Act
            _uut.Execute(_taskContext);

            //Assert
            var payments = TestDataHelper.GetPaymentsForCommitment(temporaryCommitmentId);
            Assert.AreEqual(1, payments.Length);

            //Assert.IsNotNull(payments.SingleOrDefault(p => p.FundingSource == (int)FundingSource.CoInvestedSfa && p.Amount == -1350));
            Assert.IsNotNull(payments.SingleOrDefault(p => p.FundingSource == (int)FundingSource.CoInvestedEmployer && p.Amount == -1500));
        }



    }
}
