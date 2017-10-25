using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.PriceEpisode.GetProviderPriceEpisodesQuery;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.UnitTests.Tools.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.PriceEpisode.GetProviderPriceEpisodesQuery.GetProviderPriceEpisodesQueryHandler
{
    public class WhenHandling
    {
        private readonly long _ukprn = 10007459;

        private Mock<IPriceEpisodeRepository> _priceEpisodeRepository;

        private GetProviderPriceEpisodesQueryRequest _request;
        private CollectionEarnings.DataLock.Application.PriceEpisode.GetProviderPriceEpisodesQuery.GetProviderPriceEpisodesQueryHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _priceEpisodeRepository = new Mock<IPriceEpisodeRepository>();

            _request = new GetProviderPriceEpisodesQueryRequest
            {
                Ukprn = _ukprn
            };

            _handler = new CollectionEarnings.DataLock.Application.PriceEpisode.GetProviderPriceEpisodesQuery.GetProviderPriceEpisodesQueryHandler(_priceEpisodeRepository.Object);
        }

        [Test]
        public void ThenValidResponseReturnedForValidRepositoryResponse()
        {
            // Arrange
            _priceEpisodeRepository
                .Setup(dlr => dlr.GetProviderPriceEpisodes(_ukprn))
                .Returns(new[] {new PriceEpisodeEntityBuilder().Build()});

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.IsValid);
            Assert.IsNull(response.Exception);
            Assert.AreEqual(1, response.Items.Length);
        }

        [Test]
        public void ThenInvalidResponseReturnedForInvalidRepositoryResponse()
        {
            // Arrange
            _priceEpisodeRepository
                .Setup(dlr => dlr.GetProviderPriceEpisodes(_ukprn))
                .Throws<Exception>();

            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsFalse(response.IsValid);
            Assert.IsNotNull(response.Exception);
            Assert.IsNull(response.Items);
        }
    }
}