using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.Calculator.IntegrationTests.Tools;

namespace SFA.DAS.CollectionEarnings.Calculator.IntegrationTests.Infrastructure.Data.Repositories.LearningDeliveryToProcessRepository.GetAllLearningDeliveriesToProcess
{
    public class WhenCalled
    {
        private readonly string _transientConnectionString = GlobalTestContext.Instance.ConnectionString;

        private ILearningDeliveryToProcessRepository _repository;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            _repository = new Calculator.Infrastructure.Data.Repositories.LearningDeliveryToProcessRepository(_transientConnectionString);
        }

        [Test]
        public void ThenNoLearningDeliveriesReturnedForNoEntriesInTheDatabase()
        {
            // Act
            var learningDeliveries = _repository.GetAllLearningDeliveriesToProcess();

            // Assert
            Assert.IsNotNull(learningDeliveries);
            Assert.AreEqual(0, learningDeliveries.Length);
        }

        [Test]
        public void ThenNoLearningDeliveriesReturnedForWrongEntriesInTheDatabase()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrDataNoLearningDeliveriesToProcess.sql");

            // Act
            var learningDeliveries = _repository.GetAllLearningDeliveriesToProcess();

            // Assert
            Assert.IsNotNull(learningDeliveries);
            Assert.AreEqual(0, learningDeliveries.Length);
        }

        [Test]
        public void ThenOneLearningDeliveryReturnedForOneEntryInTheDatabase()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrDataOneLearningDeliveryToProcess.sql");

            // Act
            var learningDeliveries = _repository.GetAllLearningDeliveriesToProcess();

            // Assert
            Assert.IsNotNull(learningDeliveries);
            Assert.AreEqual(1, learningDeliveries.Length);
        }

        [Test]
        public void ThenMultipleLearningDeliveriesReturnedForMultipleEntriesInTheDatabase()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrDataMultipleLearningDeliveriesToProcess.sql");

            // Act
            var learningDeliveries = _repository.GetAllLearningDeliveriesToProcess();

            // Assert
            Assert.IsNotNull(learningDeliveries);
            Assert.AreEqual(10, learningDeliveries.Length);
        }
    }
}