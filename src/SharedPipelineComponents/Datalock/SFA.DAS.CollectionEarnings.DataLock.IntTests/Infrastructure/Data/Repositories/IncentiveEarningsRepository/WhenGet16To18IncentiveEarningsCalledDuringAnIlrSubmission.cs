using System;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Utilities;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Infrastructure.Data.Repositories.PriceEpisodeRepository
{
    public class WhenGet16To18IncentiveEarningsCalledDuringAnIlrSubmission
    {
        private readonly long _ukprn = 10007459;
        private readonly long _ukprnNoEarnings = 10007458;

        private IIncentiveEarningsRepository _incentiveEarningsRepository;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            _incentiveEarningsRepository = new DataLock.Infrastructure.Data.Repositories.IncentiveEarningsRepository(GlobalTestContext.Instance.SubmissionConnectionString);
        }

        [Test]
        public void ThenNoIncentiveEarningsReturnedForNoEntriesInTheDatabase()
        {
            // Act
            var entities = _incentiveEarningsRepository.GetIncentiveEarnings(_ukprn);

            // Assert
            Assert.IsNotNull(entities);
            Assert.AreEqual(0, entities.Length);
        }

        [Test]
        [TestCase("IlrSubmissionFirstIncentiveEarnings")]
        [TestCase("IlrSubmissionSecondIncentiveEarnings")]
        public void ThenOneResultIsReturnedForAUkprnWithIncentiveEarningsInTheDatabase(string sqlFile)
        {
            // Arrange
            TestDataHelper.ExecuteScript($"IncentiveEarningsRepository\\Submission\\{sqlFile}.sql");

            //Act
           var entities = _incentiveEarningsRepository.GetIncentiveEarnings(_ukprn);

            // Assert
            Assert.IsNotNull(entities);
            Assert.AreEqual(1, entities.Length);
        }

        [Test]
        [TestCase("IlrSubmissionFirstIncentiveEarnings")]
        [TestCase("IlrSubmissionSecondIncentiveEarnings")]
        public void ThenNoResultIsReturnedForAUkprnWithNoDataInTheDatabase(string sqlFile)
        {
            // Arrange
            TestDataHelper.ExecuteScript($"IncentiveEarningsRepository\\Submission\\{sqlFile}.sql");

            //Act
            var entities = _incentiveEarningsRepository.GetIncentiveEarnings(_ukprnNoEarnings);

            // Assert
            Assert.IsNotNull(entities);
            Assert.AreEqual(0, entities.Length);
        }

        [Test]
        [TestCase("IlrSubmissionZeroFirstIncentiveEarnings")]
        [TestCase("IlrSubmissionZeroSecondIncentiveEarnings")]
        public void ThenNoResultsAreReturnedForAUkprnWithZeroIncentiveEarningsInTheDatabase(string sqlFile)
        {
            // Arrange
            TestDataHelper.ExecuteScript($"IncentiveEarningsRepository\\Submission\\{sqlFile}.sql");

            //Act
            var entities = _incentiveEarningsRepository.GetIncentiveEarnings(_ukprn);

            // Assert
            Assert.IsNotNull(entities);
            Assert.AreEqual(0, entities.Length);
        }

        [Test]
        public void ThenMatchingDataIsReturnedForAUkprnWithFirstIncentiveEarningsInTheDatabase()
        {
            // Arrange
            TestDataHelper.ExecuteScript($"IncentiveEarningsRepository\\Submission\\IlrSubmissionFirstIncentiveEarnings.sql");

            //Act
            var entities = _incentiveEarningsRepository.GetIncentiveEarnings(_ukprn);

            // Assert
            Assert.IsNotNull(entities);
            Assert.AreEqual(1, entities.Length);

            Assert.AreEqual(entities[0].LearnRefNumber, "2");
            Assert.AreEqual(entities[0].Ukprn, 10007459);
            Assert.AreEqual(entities[0].Period, 4);
            Assert.AreEqual(entities[0].PriceEpisodeIdentifier, "25-98765-06/08/2017");
            Assert.AreEqual(entities[0].PriceEpisodeFirstEmp1618Pay, 500);
            Assert.AreEqual(entities[0].PriceEpisodeSecondEmp1618Pay, 0);
            
        }

        [Test]
        public void ThenMatchingDataIsReturnedForAUkprnWithSecondIncentiveEarningsInTheDatabase()
        {
            // Arrange
            TestDataHelper.ExecuteScript($"IncentiveEarningsRepository\\Submission\\IlrSubmissionSecondIncentiveEarnings.sql");

            //Act
            var entities = _incentiveEarningsRepository.GetIncentiveEarnings(_ukprn);

            // Assert
            Assert.IsNotNull(entities);
            Assert.AreEqual(1, entities.Length);

            Assert.AreEqual(entities[0].LearnRefNumber, "2");
            Assert.AreEqual(entities[0].Ukprn, 10007459);
            Assert.AreEqual(entities[0].Period, 12);
            Assert.AreEqual(entities[0].PriceEpisodeIdentifier, "25-98765-06/08/2017");
            Assert.AreEqual(entities[0].PriceEpisodeFirstEmp1618Pay, 0);
            Assert.AreEqual(entities[0].PriceEpisodeSecondEmp1618Pay, 500);

        }

    }
}