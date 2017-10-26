using System;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Tools;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Infrastructure.Data.Repositories.PriceEpisodeRepository
{
    public class WhenGetProviderPriceEpisodesCalledDuringAnIlrSubmission
    {
        private readonly long _ukprn = 10007459;
        private readonly long _ukprnNoPriceEpisodes = 10007458;

        private IPriceEpisodeRepository _priceEpisodeRepository;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            _priceEpisodeRepository = new DataLock.Infrastructure.Data.Repositories.PriceEpisodeRepository(GlobalTestContext.Instance.SubmissionConnectionString);
        }

        [Test]
        public void ThenNoPriceEpisodesReturnedForNoEntriesInTheDatabase()
        {
            // Act
            var priceEpisodes = _priceEpisodeRepository.GetProviderPriceEpisodes(_ukprn);

            // Assert
            Assert.IsNotNull(priceEpisodes);
            Assert.AreEqual(0, priceEpisodes.Length);
        }

        [Test]
        public void ThenNoPriceEpisodesReturnedForAUkprnWithNoEntriesInTheDatabase()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrSubmissionMultipleLearningDeliveries.sql");

            // Act
            var priceEpisodes = _priceEpisodeRepository.GetProviderPriceEpisodes(_ukprnNoPriceEpisodes);

            // Assert
            Assert.IsNotNull(priceEpisodes);
            Assert.AreEqual(0, priceEpisodes.Length);
        }

        [Test]
        public void ThenPriceEpisodesReturnedForMultipleEntriesInTheDatabase()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrSubmissionMultipleLearningDeliveries.sql");

            // Act
            var priceEpisodes = _priceEpisodeRepository.GetProviderPriceEpisodes(_ukprn);

            // Assert
            Assert.IsNotNull(priceEpisodes);
            Assert.AreEqual(7, priceEpisodes.Length);
        }

        [Test]
        public void ThenPriceEpisodesReturnedForOneLearnerWithMultipleNegotiatedPriceEpisodesInTheDatabase()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrSubmissionLearnerChangesEmployers.sql");

            // Act
            var priceEpisodes = _priceEpisodeRepository.GetProviderPriceEpisodes(_ukprn);

            // Assert
            Assert.IsNotNull(priceEpisodes);
            Assert.AreEqual(2, priceEpisodes.Length);
            Assert.AreEqual(1, priceEpisodes.Count(l => l.StartDate == new DateTime(2017, 8, 1)));
            Assert.AreEqual(1, priceEpisodes.Count(l => l.StartDate == new DateTime(2017, 11, 1)));
        }

        [Test]
        public void ThenOnePriceEpisodeReturnedForOneLearnerThatChangesEmployersFromDasToNonDasInTheDatabase()
        {
            // Arrange
            TestDataHelper.ExecuteScript("IlrSubmissionLearnerChangesEmployersDasToNonDas.sql");

            // Act
            var priceEpisodes = _priceEpisodeRepository.GetProviderPriceEpisodes(_ukprn);

            // Assert
            Assert.IsNotNull(priceEpisodes);
            Assert.AreEqual(1, priceEpisodes.Length);
            Assert.AreEqual(1, priceEpisodes.Count(l => l.StartDate == new DateTime(2017, 8, 1)));
        }
    }
}