using NUnit.Framework;
using System.Linq;
using SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests.Tools;
using SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests.Utilities;
using SFA.DAS.Payments.DCFS.Domain;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests.FinishOnTime.WhenNoPaymentsAreDue
{
    public class WhenASinglePaymentOf1500IsDueForThisPeriodAndTheSfaContributionPercentageVaries
    {
        private CoInvestedPaymentsTask _uut;
        private IntegrationTaskContext _taskContext;
        private long _commitmentId;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            const int accountId = 2345435;
            TestDataHelper.AddAccount(accountId);

            _commitmentId = 1L;
            TestDataHelper.AddCommitment(_commitmentId, accountId);

            _taskContext = new IntegrationTaskContext();
            _uut = new CoInvestedPaymentsTask();
        }

        [Test]
        [TestCase(0.9)]
        [TestCase(0.75)]
        [TestCase(1.00)]
        [TestCase(0.00)]
        public void ThenACoInvestedEmployerPaymentOf150IsMade(decimal sfaContributionPercentage)
        {
            // Arrange
            TestDataHelper.AddPaymentDueForProvider(_commitmentId, 1, amountDue: 1500, sfaContributionPercentage: sfaContributionPercentage);

            TestDataHelper.CopyReferenceData();

            // Act
            _uut.Execute(_taskContext);

            // Assert
            var sfaPayment = 1500m * sfaContributionPercentage;
            var employerPayment = 1500m * (1 - sfaContributionPercentage);

            var payments = TestDataHelper.GetPaymentsForCommitment(_commitmentId);

            if (sfaPayment == 0)
            {
                Assert.IsNull(
                payments.SingleOrDefault(p => p.FundingSource == (int)FundingSource.CoInvestedSfa && p.Amount == sfaPayment));
            }
            else
            {
                Assert.IsNotNull(
                payments.SingleOrDefault(p => p.FundingSource == (int)FundingSource.CoInvestedSfa && p.Amount == sfaPayment));
            }

            if (employerPayment == 0)
            {
                Assert.IsNull(
                payments.SingleOrDefault(p => p.FundingSource == (int)FundingSource.CoInvestedEmployer && p.Amount == employerPayment));
            }
            else
            {
                Assert.IsNotNull(
                    payments.SingleOrDefault(p => p.FundingSource == (int)FundingSource.CoInvestedEmployer && p.Amount == employerPayment));
            }
        }
    }

}
