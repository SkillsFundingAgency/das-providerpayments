using System;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Utilities;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Infrastructure.Data.Repositories.PriceEpisodeRepository
{
    public class WhenGetProviderPriceEpisodesCalledDuringAnIlrPeriodEnd
    {
        private readonly long _ukprn = 10007459;
        private readonly long _ukprnNoLearners = 10007458;

        private IPriceEpisodeRepository _priceEpisodeRepository;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            _priceEpisodeRepository = new DataLock.Infrastructure.Data.Repositories.PriceEpisodeRepository(GlobalTestContext.Instance.PeriodEndConnectionString);
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
            TestDataHelper.PeriodEndExecuteScript("PeriodEndMultipleLearningDeliveries.sql");

            // Act
            var priceEpisodes = _priceEpisodeRepository.GetProviderPriceEpisodes(_ukprnNoLearners);

            // Assert
            Assert.IsNotNull(priceEpisodes);
            Assert.AreEqual(0, priceEpisodes.Length);
        }

        [Test]
        public void ThenPriceEpisodesReturnedForMultipleEntriesInTheDatabase()
        {
            // Arrange
            TestDataHelper.PeriodEndExecuteScript("PeriodEndMultipleLearningDeliveries.sql");
            TestDataHelper.PeriodEndCopyReferenceData();

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
            TestDataHelper.PeriodEndExecuteScript("PeriodEndLearnerChangesEmployers.sql");
            TestDataHelper.PeriodEndCopyReferenceData();

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
            TestDataHelper.PeriodEndExecuteScript("PeriodEndLearnerChangesEmployersDasToNonDas.sql");
            TestDataHelper.PeriodEndCopyReferenceData();

            // Act
            var priceEpisodes = _priceEpisodeRepository.GetProviderPriceEpisodes(_ukprn);

            // Assert
            Assert.IsNotNull(priceEpisodes);
            Assert.AreEqual(1, priceEpisodes.Length);
            Assert.AreEqual(1, priceEpisodes.Count(l => l.StartDate == new DateTime(2017, 8, 1)));
        }

        [Test]
        public void ThenFirstIncentiveThreshholdDateIsReturned()
        {
            // Arrange
            TestDataHelper.PeriodEndExecuteScript("PeriodEndLearnerFirstIncentiveThreshhold.sql");
            TestDataHelper.PeriodEndCopyReferenceData();

            // Act
            var priceEpisodes = _priceEpisodeRepository.GetProviderPriceEpisodes(_ukprn);

            // Assert
            Assert.IsNotNull(priceEpisodes);
            Assert.AreEqual(1, priceEpisodes.Length);
            Assert.AreEqual(priceEpisodes.Single().FirstAdditionalPaymentThresholdDate,new DateTime(2017,11,04));
        }


        [Test]
        public void ThenSecondIncentiveThreshholdDateIsReturned()
        {
            // Arrange
            TestDataHelper.PeriodEndExecuteScript("PeriodEndLearnerSecondIncentiveThreshhold.sql");
            TestDataHelper.PeriodEndCopyReferenceData();

            // Act
            var priceEpisodes = _priceEpisodeRepository.GetProviderPriceEpisodes(_ukprn);

            // Assert
            Assert.IsNotNull(priceEpisodes);
            Assert.AreEqual(1, priceEpisodes.Length);
            Assert.AreEqual(priceEpisodes.Single().SecondAdditionalPaymentThresholdDate, new DateTime(2018, 08, 04));
        }
    }
}