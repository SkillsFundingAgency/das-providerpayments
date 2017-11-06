using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Utilities;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests.Infrastructure.Data.Repositories.PriceEpisodePeriodMatchRepository
{
    public class WhenAddPriceEpisodePeriodMatchesCalledDuringAnIlrPeriodEnd
    {
        private IPriceEpisodePeriodMatchRepository _priceEpisodePeriodMatchRepository;

        [SetUp]
        public void Arrange()
        {
            TestDataHelper.Clean();

            _priceEpisodePeriodMatchRepository = new DataLock.Infrastructure.Data.Repositories.PriceEpisodePeriodMatchRepository(GlobalTestContext.Instance.PeriodEndConnectionString);
        }

        [Test]
        public void ThenPriceEpisodePeriodMatchesAreAddedSuccessfully()
        {
            // Arrange
            var matches = new[]
            {
                new PriceEpisodePeriodMatchEntityBuilder().Build(),
                new PriceEpisodePeriodMatchEntityBuilder().WithPeriod(2).Build(),
                new PriceEpisodePeriodMatchEntityBuilder().WithPeriod(3).Build()
            };

            // Act
            _priceEpisodePeriodMatchRepository.AddPriceEpisodePeriodMatches(matches);

            // Assert
            var matchEntities = TestDataHelper.PeriodEndGetPriceEpisodePeriodMatches();

            Assert.IsNotNull(matchEntities);
            Assert.AreEqual(3, matchEntities.Length);
        }
    }
}