using NUnit.Framework;
using SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests.Tools;
using System.Linq;
using SFA.DAS.Payments.DCFS.Domain;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests.FinishOnTime
{
    public class WhenLearnerIsNonDas
    {
        private readonly long _uln = 1000000108;

        private CoInvestedPaymentsTask _uut;
        private IntegrationTaskContext _taskContext;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            TestDataHelper.AddPaymentDueForNonDas(1, _uln, amountDue: 1500m);

            TestDataHelper.CopyReferenceData();

            _taskContext = new IntegrationTaskContext();
            _uut = new CoInvestedPaymentsTask();
        }

        [Test]
        public void ThenNonDasPaymentsAreMade()
        {
            // Act
            _uut.Execute(_taskContext);

            // Assert
            var payments = TestDataHelper.GetPaymentsForUln(_uln);
            Assert.IsNotNull(payments);
            Assert.AreEqual(2, payments.Length);

            Assert.AreEqual(1, payments.Count(p => p.FundingSource == (int) FundingSource.CoInvestedSfa && p.Amount == 1500m * 0.9m));
            Assert.AreEqual(1, payments.Count(p => p.FundingSource == (int) FundingSource.CoInvestedEmployer && p.Amount == 1500m * 0.1m));
        }
    }
}
