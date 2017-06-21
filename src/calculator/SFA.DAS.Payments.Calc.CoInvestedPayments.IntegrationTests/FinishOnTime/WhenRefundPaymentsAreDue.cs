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

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            var accountId = Guid.NewGuid().ToString();
            TestDataHelper.AddAccount(accountId, isDeds: false);


            _commitmentId = 1L;
            _ukprn = Random.Next(1, int.MaxValue);
            _uln = Random.Next(1, int.MaxValue);


            TestDataHelper.AddCommitment(_commitmentId, accountId, ukprn: _ukprn, uln: _uln, isDeds: false);
            TestDataHelper.AddCommitment(_commitmentId, accountId, ukprn: _ukprn, uln: _uln, isDeds: true);

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
            TestDataHelper.PopulatePaymentsHistory();

            TestDataHelper.AddPaymentDueForProvider(_commitmentId, _ukprn, amountDue: -1500, sfaContributionPercentage: 0.9m);

            // Act
            _uut.Execute(_taskContext);

            //Assert
            var payments = TestDataHelper.GetPaymentsForCommitment(_commitmentId);
            Assert.IsTrue(payments.Count() == 2);

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
            TestDataHelper.PopulatePaymentsHistory();

            TestDataHelper.AddPaymentDueForProvider(_commitmentId, _ukprn, amountDue: -750, sfaContributionPercentage: 0.9m);

            // Act
            _uut.Execute(_taskContext);

            //Assert
            var payments = TestDataHelper.GetPaymentsForCommitment(_commitmentId);
            Assert.IsTrue(payments.Count() == 2);

            var actualPaymentsMessage = "actually paid:\n" + payments.Select(x => x.Amount.ToString() + " " + ((FundingSource)x.FundingSource).ToString()).Aggregate((x, y) => $"{x}\n{y}");
            Assert.IsNotNull(payments.SingleOrDefault(p => p.FundingSource == (int)FundingSource.CoInvestedSfa && p.Amount == -675), $"Expected -675 CoInvestedSfa, {actualPaymentsMessage}");
            Assert.IsNotNull(payments.SingleOrDefault(p => p.FundingSource == (int)FundingSource.CoInvestedEmployer && p.Amount == -75), $"Expected -75 CoInvestedEmployer, {actualPaymentsMessage}");
        }
    }
}
