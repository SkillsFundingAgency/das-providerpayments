using System.Linq;
using NUnit.Framework;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Infrastructure.Data;
using SFA.DAS.Payments.Calc.ProviderAdjustments.IntegrationTests.Tools;

namespace SFA.DAS.Payments.Calc.ProviderAdjustments.IntegrationTests.Infrastructure.Data.Repositories.DcfsAdjustmentRepository
{
    public class WhenReadingCurrentAdjustments
    {
        private IAdjustmentRepository _repository;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            _repository = new ProviderAdjustments.Infrastructure.Data.Repositories.DcfsAdjustmentRepository(GlobalTestContext.Instance.TransientConnectionString);
        }

        [Test]
        public void ThenAdjustmentsAreReadFromTheDatabase()
        {
            // Arrange
            var ukprn = 10007459;
            TestDataHelper.AddProviderAdjustmentsSubmission(ukprn);

            TestDataHelper.CopyReferenceData();
            
            // Act
            var adjustments = _repository.GetCurrentProviderAdjustments(ukprn);

            Assert.IsNotNull(adjustments);
            Assert.AreEqual(12, adjustments.Length);
            Assert.AreEqual(12, adjustments.Count(a => a.Ukprn == ukprn && a.Amount == 1000.00m));
        }
    }
}