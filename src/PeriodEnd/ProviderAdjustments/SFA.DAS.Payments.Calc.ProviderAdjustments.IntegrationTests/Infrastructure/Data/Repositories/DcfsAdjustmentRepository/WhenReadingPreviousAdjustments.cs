using System.Linq;
using NUnit.Framework;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Infrastructure.Data;
using SFA.DAS.Payments.Calc.ProviderAdjustments.IntegrationTests.Tools;
using SFA.DAS.Payments.Calc.ProviderAdjustments.IntegrationTests.Utilities;

namespace SFA.DAS.Payments.Calc.ProviderAdjustments.IntegrationTests.Infrastructure.Data.Repositories.DcfsAdjustmentRepository
{
    public class WhenReadingPreviousAdjustments
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
            TestDataHelper.AddProvider(ukprn);
            TestDataHelper.AddPreviousProviderAdjustments(ukprn);

            TestDataHelper.CopyReferenceData();

            // Act
            var adjustments = _repository.GetPreviousProviderAdjustments();

            Assert.IsNotNull(adjustments);
            Assert.AreEqual(12, adjustments.Length);
            Assert.AreEqual(12, adjustments.Count(a => a.Ukprn == ukprn && a.Amount == 500.00m));
        }
    }
}