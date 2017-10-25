using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.PriceEpisodeMatch.AddPriceEpisodeMatchesCommand;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.PriceEpisodeMatch.AddPriceEpisodeMatchesCommand.AddPriceEpisodeMatchesCommandHandler
{
    public class WhenHandling
    {
        private static readonly CollectionEarnings.DataLock.Application.PriceEpisodeMatch.PriceEpisodeMatch[] PriceEpisodeMatches = 
        {
            new CollectionEarnings.DataLock.Application.PriceEpisodeMatch.PriceEpisodeMatch
            {
                Ukprn = 10007459,
                LearnerReferenceNumber = "Lrn001",
                AimSequenceNumber = 1,
                CommitmentId = 1,
                PriceEpisodeIdentifier = "27-25-2016-09-01"
            },
            new CollectionEarnings.DataLock.Application.PriceEpisodeMatch.PriceEpisodeMatch
            {
                Ukprn = 10007459,
                LearnerReferenceNumber = "Lrn002",
                AimSequenceNumber = 9,
                CommitmentId = 2,
                PriceEpisodeIdentifier = "27-25-2016-10-15"
            }
        };

        private Mock<IPriceEpisodeMatchRepository> _repository;

        private AddPriceEpisodeMatchesCommandRequest _request;
        private CollectionEarnings.DataLock.Application.PriceEpisodeMatch.AddPriceEpisodeMatchesCommand.AddPriceEpisodeMatchesCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _repository = new Mock<IPriceEpisodeMatchRepository>();

            _request = new AddPriceEpisodeMatchesCommandRequest
            {
                PriceEpisodeMatches = PriceEpisodeMatches
            };

            _handler = new CollectionEarnings.DataLock.Application.PriceEpisodeMatch.AddPriceEpisodeMatchesCommand.AddPriceEpisodeMatchesCommandHandler(_repository.Object);
        }

        [Test]
        public void ThenSuccessfullForValidRepositoryResponse()
        {
            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
        }

        [Test]
        public void ThenItShouldWriteThePriceEpisodeMatchesToTheRepository()
        {
            // Act
            _handler.Handle(_request);

            // Assert
            _repository.Verify(r => r.AddPriceEpisodeMatches(It.Is<PriceEpisodeMatchEntity[]>(l => BatchesMatch(l, PriceEpisodeMatches))), Times.Once);
        }

        [Test]
        public void ThenExceptionIsThrownForInvalidRepositoryResponse()
        {
            // Arrange
            _repository
                .Setup(ver => ver.AddPriceEpisodeMatches(It.IsAny<PriceEpisodeMatchEntity[]>()))
                .Throws<Exception>();

            // Assert
            Assert.Throws<Exception>(() => _handler.Handle(_request));
        }

        private bool BatchesMatch(PriceEpisodeMatchEntity[] entities, CollectionEarnings.DataLock.Application.PriceEpisodeMatch.PriceEpisodeMatch[] priceEpisodeMatches)
        {
            if (entities.Length != priceEpisodeMatches.Length)
            {
                return false;
            }

            for (var x = 0; x < entities.Length; x++)
            {
                if (!MatchesMatch(entities[x], priceEpisodeMatches[x]))
                {
                    return false;
                }
            }

            return true;
        }

        private bool MatchesMatch(PriceEpisodeMatchEntity entity, CollectionEarnings.DataLock.Application.PriceEpisodeMatch.PriceEpisodeMatch priceEpisodeMatch)
        {
            return entity.Ukprn == priceEpisodeMatch.Ukprn
                   && entity.LearnRefNumber == priceEpisodeMatch.LearnerReferenceNumber
                   && entity.AimSeqNumber == priceEpisodeMatch.AimSequenceNumber
                   && entity.CommitmentId == priceEpisodeMatch.CommitmentId
                   && entity.PriceEpisodeIdentifier == priceEpisodeMatch.PriceEpisodeIdentifier;
        }
    }
}