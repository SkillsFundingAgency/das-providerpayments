using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.DataLock.Application.PriceEpisodePeriodMatch.AddPriceEpisodePeriodMatchesCommand;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.UnitTests.Application.PriceEpisodePeriodMatch.AddPriceEpisodePeriodMatchesCommand.AddPriceEpisodePeriodMatchesCommandHandler
{
    public class WhenHandling
    {
        private static readonly CollectionEarnings.DataLock.Application.PriceEpisodePeriodMatch.PriceEpisodePeriodMatch[] PeriodMatches =
        {
            new CollectionEarnings.DataLock.Application.PriceEpisodePeriodMatch.PriceEpisodePeriodMatch
            {
                Ukprn = 10007459,
                PriceEpisodeIdentifier = "27-25-2016-09-01",
                LearnerReferenceNumber = "Lrn001",
                AimSequenceNumber = 1,
                CommitmentId = 10,
                CommitmentVersionId = "10-100",
                Period = 6,
                Payable = true
            },
            new CollectionEarnings.DataLock.Application.PriceEpisodePeriodMatch.PriceEpisodePeriodMatch
            {
                Ukprn = 10007459,
                PriceEpisodeIdentifier = "27-25-2017-09-01",
                LearnerReferenceNumber = "Lrn002",
                AimSequenceNumber = 9,
                CommitmentId = 7,
                CommitmentVersionId = "7-24",
                Period = 1,
                Payable = false
            }
        };

        private Mock<IPriceEpisodePeriodMatchRepository> _repository;

        private AddPriceEpisodePeriodMatchesCommandRequest _request;
        private CollectionEarnings.DataLock.Application.PriceEpisodePeriodMatch.AddPriceEpisodePeriodMatchesCommand.AddPriceEpisodePeriodMatchesCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _repository = new Mock<IPriceEpisodePeriodMatchRepository>();

            _request = new AddPriceEpisodePeriodMatchesCommandRequest
            {
                PriceEpisodePeriodMatches = PeriodMatches
            };

            _handler = new CollectionEarnings.DataLock.Application.PriceEpisodePeriodMatch.AddPriceEpisodePeriodMatchesCommand.AddPriceEpisodePeriodMatchesCommandHandler(_repository.Object);
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
        public void ThenItShouldWriteThePriceEpisodePeriodMatchesToTheRepository()
        {
            // Act
            _handler.Handle(_request);

            // Assert
            _repository.Verify(r => r.AddPriceEpisodePeriodMatches(It.Is<PriceEpisodePeriodMatchEntity[]>(l => PriceEpisodePeriodMatchesBatchesMatch(l, PeriodMatches))), Times.Once);
        }

        [Test]
        public void ThenExceptionIsThrownForInvalidRepositoryResponse()
        {
            // Arrange
            _repository
                .Setup(ver => ver.AddPriceEpisodePeriodMatches(It.IsAny<PriceEpisodePeriodMatchEntity[]>()))
                .Throws<Exception>();

            // Assert
            Assert.Throws<Exception>(() => _handler.Handle(_request));
        }

        private bool PriceEpisodePeriodMatchesBatchesMatch(PriceEpisodePeriodMatchEntity[] entities, CollectionEarnings.DataLock.Application.PriceEpisodePeriodMatch.PriceEpisodePeriodMatch[] matches)
        {
            if (entities.Length != matches.Length)
            {
                return false;
            }

            for (var x = 0; x < entities.Length; x++)
            {
                if (!PeriodMatchesMatch(entities[x], matches[x]))
                {
                    return false;
                }
            }

            return true;
        }

        private bool PeriodMatchesMatch(PriceEpisodePeriodMatchEntity entity, CollectionEarnings.DataLock.Application.PriceEpisodePeriodMatch.PriceEpisodePeriodMatch periodMatch)
        {
            return entity.Ukprn == periodMatch.Ukprn
                   && entity.PriceEpisodeIdentifier == periodMatch.PriceEpisodeIdentifier
                   && entity.LearnRefNumber == periodMatch.LearnerReferenceNumber
                   && entity.AimSeqNumber == periodMatch.AimSequenceNumber
                   && entity.CommitmentId == periodMatch.CommitmentId
                   && entity.VersionId == periodMatch.CommitmentVersionId
                   && entity.Period == periodMatch.Period
                   && entity.Payable == periodMatch.Payable;
        }
    }
}