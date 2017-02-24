using NUnit.Framework;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.IntegrationTests.Tools;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.IntegrationTests.Infrastructure.Data.Repositories.DcfsPaymentDueRepository
{
    public class WhenGettingPaymentsDueForCommitment
    {
        private IPaymentDueRepository _repository;

        private static readonly int[][] PaymentsDue =
        {
            new[] {5, 2017},
            new[] {2, 2017},
            new[] {5, 2016}
        };

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            _repository = new LevyPayments.Infrastructure.Data.Repositories.DcfsPaymentDueRepository(GlobalTestContext.Instance.ConnectionString);
        }

        [Test]
        public void ThenThePaymentsAreOrderedByDeliveryYearAndDeliveryMonth()
        {
            // Arrange
            var accountId = 1;
            TestDataHelper.AddAccount(accountId);

            var commitmentId = 1;
            TestDataHelper.AddCommitment(commitmentId, accountId.ToString());

            foreach (var paymentDue in PaymentsDue)
            {
                TestDataHelper.AddPaymentDueForCommitment(commitmentId, deliveryMonth: paymentDue[0], deliveryYear: paymentDue[1]);
            }

            // Act
            var payments = _repository.GetPaymentsDueForCommitment(commitmentId);

            // Assert
            Assert.IsNotNull(payments);
            Assert.AreEqual(3, payments.Length);

            Assert.AreEqual(PaymentsDue[2][0], payments[0].DeliveryMonth);
            Assert.AreEqual(PaymentsDue[2][1], payments[0].DeliveryYear);
            Assert.AreEqual(PaymentsDue[1][0], payments[1].DeliveryMonth);
            Assert.AreEqual(PaymentsDue[1][1], payments[1].DeliveryYear);
            Assert.AreEqual(PaymentsDue[0][0], payments[2].DeliveryMonth);
            Assert.AreEqual(PaymentsDue[0][1], payments[2].DeliveryYear);
        }

        [Test]
        [TestCase(TransactionType.First16To18EmployerIncentive)]
        [TestCase(TransactionType.First16To18ProviderIncentive)]
        [TestCase(TransactionType.Second16To18EmployerIncentive)]
        [TestCase(TransactionType.Second16To18ProviderIncentive)]
        [TestCase(TransactionType.OnProgrammeMathsAndEnglish)]
        [TestCase(TransactionType.BalancingMathsAndEnglish)]
        public void ThenSfaFullyFundedPaymentsDueAreIgnored(TransactionType transactionType)
        {
            // Arrange
            var accountId = 1;
            TestDataHelper.AddAccount(accountId);

            var commitmentId = 1;
            TestDataHelper.AddCommitment(commitmentId, accountId.ToString());

            TestDataHelper.AddPaymentDueForCommitment(commitmentId, deliveryMonth: 5, deliveryYear: 2017, transactionType: transactionType);

            // Act
            var payments = _repository.GetPaymentsDueForCommitment(commitmentId);

            // Assert
            Assert.IsNotNull(payments);
            Assert.AreEqual(0, payments.Length);
        }
    }
}