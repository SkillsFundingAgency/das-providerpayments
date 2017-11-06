using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Utilities;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Infrastructure.Data.Repositories.PriceEpisodeMatchRepository
{
    public class WhenAddPriceEpisodeMatchesCalledDuringAnIlrPeriodEnd
    {
        private IPriceEpisodeMatchRepository _priceEpisodeMatchRepository;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            _priceEpisodeMatchRepository = new DataLock.Infrastructure.Data.Repositories.PriceEpisodeMatchRepository(GlobalTestContext.Instance.PeriodEndConnectionString);
        }

        [Test]
        public void ThenPriceEpisodeMatchesAreAddedSuccessfully()
        {
            // Arrange
            var matches = new[]
            {
                new PriceEpisodeMatchEntityBuilder().Build(),
                new PriceEpisodeMatchEntityBuilder().WithLearnRefNumber("Lrn002").WithCommitmentId(2).Build(),
                new PriceEpisodeMatchEntityBuilder().WithLearnRefNumber("Lrn003").WithAimSeqNumber(2).WithCommitmentId(2).Build()
            };

            // Act
            _priceEpisodeMatchRepository.AddPriceEpisodeMatches(matches);

            // Assert
            var matchesEntities = TestDataHelper.PeriodEndGetPriceEpisodeMatches();

            Assert.IsNotNull(matchesEntities);
            Assert.AreEqual(3, matchesEntities.Length);
        }
    }
}