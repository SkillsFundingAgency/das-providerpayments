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

        private static readonly Random Random = new Random();

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            var accountId = Guid.NewGuid().ToString();
            TestDataHelper.AddAccount(accountId,isDeds:true);

            _commitmentId = 1L;
            _ukprn = Random.Next(1, int.MaxValue);

            TestDataHelper.AddCommitment(_commitmentId, accountId,ukprn:_ukprn,isDeds:true);
            TestDataHelper.AddProvider(_ukprn);
            TestDataHelper.CopyReferenceData();
            
            _taskContext = new IntegrationTaskContext();
            _uut = new CoInvestedPaymentsTask();
        }


        [Test]
        public void ThenCoInvestedRefundPaymentsAreMade()
        {
            // Arrange
            var requiredPaymentId = Guid.NewGuid().ToString();

            TestDataHelper.AddPaymentDueForProvider2(
                    _commitmentId, 
                    _ukprn,8,2016,
                    amountDue: 1500, 
                    transactionType: TransactionType.Learning,
                    requiredPaymentId:requiredPaymentId,
                    isDeds:true);

            TestDataHelper.AddPaymentHistoryForCommitment(requiredPaymentId, FundingSource.CoInvestedEmployer, 150,8,2016,TransactionType.Learning,true);
            TestDataHelper.AddPaymentHistoryForCommitment(requiredPaymentId, FundingSource.CoInvestedSfa, 1350, 8, 2016, TransactionType.Learning,true);
            TestDataHelper.PopulatePaymentsHistory();

            TestDataHelper.AddPaymentDueForProvider(_commitmentId, _ukprn, amountDue: -1500, sfaContributionPercentage: 0.9m);

           

            // Act
            _uut.Execute(_taskContext);

            // Assert
            //var sfaPayment = 1500m * sfaContributionPercentage;
            //var employerPayment = 1500m * (1 - sfaContributionPercentage);

            //var payments = TestDataHelper.GetPaymentsForCommitment(_commitmentId);
            //Assert.IsNotNull(
            //    payments.SingleOrDefault(p => p.FundingSource == (int)FundingSource.CoInvestedSfa && p.Amount == sfaPayment));
            //Assert.IsNotNull(
            //    payments.SingleOrDefault(p => p.FundingSource == (int)FundingSource.CoInvestedEmployer && p.Amount == employerPayment));
        }
    }
}
