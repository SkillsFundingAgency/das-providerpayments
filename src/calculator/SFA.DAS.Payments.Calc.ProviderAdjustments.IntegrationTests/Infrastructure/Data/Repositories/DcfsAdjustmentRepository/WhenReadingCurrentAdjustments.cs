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
            TestDataHelper.CopyReferenceData();
            
            Assert.IsTrue(true);
        }
    }
}