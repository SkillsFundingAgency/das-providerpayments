using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.Application.ApprenticeshipPriceEpisode.ProcessAddPriceEpisodesCommand;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Entities;
using SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools.Builders;

namespace SFA.DAS.CollectionEarnings.Calculator.UnitTests.Application.ApprenticeshipPriceEpisode.ProcessAddPriceEpisodesCommand.ProcessAddPriceEpisodesCommandHandler
{
    public class WhenHandling
    {
        private static readonly Calculator.Application.ApprenticeshipPriceEpisode.ApprenticeshipPriceEpisode[] PriceEpisodes = 
        {
            new ApprenticeshipPriceEpisodeBuilder().Build(),
            new ApprenticeshipPriceEpisodeBuilder().WithAimSeqNumber(2).Build(),
            new ApprenticeshipPriceEpisodeBuilder().WithAimSeqNumber(3).Build(),
            new ApprenticeshipPriceEpisodeBuilder().WithAimSeqNumber(4).Build(),
            new ApprenticeshipPriceEpisodeBuilder().WithAimSeqNumber(5).Build()
        };

        private Mock<IApprenticeshipPriceEpisodeRepository> _repository;

        private ProcessAddPriceEpisodesCommandRequest _request;
        private Calculator.Application.ApprenticeshipPriceEpisode.ProcessAddPriceEpisodesCommand.ProcessAddPriceEpisodesCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _repository = new Mock<IApprenticeshipPriceEpisodeRepository>();

            _request = new ProcessAddPriceEpisodesCommandRequest
            {
                PriceEpisodes = PriceEpisodes
            };

            _handler = new Calculator.Application.ApprenticeshipPriceEpisode.ProcessAddPriceEpisodesCommand.ProcessAddPriceEpisodesCommandHandler(_repository.Object);
        }

        [Test]
        public void ThenValidResponseReturnedForValidRepositoryResponse()
        {
            // Act
            var response = _handler.Handle(_request);

            // Assert
            Assert.IsNotNull(response);
        }

        [Test]
        public void ThenItShouldWriteThePriceEpisodesToTheRepository()
        {
            // Act
            _handler.Handle(_request);

            // Assert
            _repository.Verify(r => r.AddApprenticeshipPriceEpisodes(It.Is<ApprenticeshipPriceEpisodeEntity[]>(l => PriceEpisodesBatchesMatch(l, PriceEpisodes))));
        }

        [Test]
        public void ThenInvalidResponseReturnedForInvalidRepositoryResponse()
        {
            // Arrange
            _repository
                .Setup(r => r.AddApprenticeshipPriceEpisodes(It.IsAny<ApprenticeshipPriceEpisodeEntity[]>()))
                .Throws<Exception>();

            // Assert
            Assert.Throws<Exception>(() => _handler.Handle(_request));
        }

        private bool PriceEpisodesBatchesMatch(ApprenticeshipPriceEpisodeEntity[] entities, Calculator.Application.ApprenticeshipPriceEpisode.ApprenticeshipPriceEpisode[] priceEpisodes)
        {
            if (entities.Length != priceEpisodes.Length)
            {
                return false;
            }

            for (var x = 0; x < entities.Length; x++)
            {
                if (!PriceEpisodesMatch(entities[x], priceEpisodes[x]))
                {
                    return false;
                }
            }

            return true;
        }

        private bool PriceEpisodesMatch(ApprenticeshipPriceEpisodeEntity entity, Calculator.Application.ApprenticeshipPriceEpisode.ApprenticeshipPriceEpisode priceEpisode)
        {
            return entity.PriceEpisodeIdentifier == priceEpisode.Id
                && entity.LearnRefNumber == priceEpisode.LearnerReferenceNumber
                && entity.EpisodeEffectiveTNPStartDate == priceEpisode.StartDate
                && entity.EpisodeStartDate == priceEpisode.StartDate
                && entity.PriceEpisodeActualEndDate == priceEpisode.ActualEndDate
                && entity.PriceEpisodeAimSeqNumber == priceEpisode.AimSequenceNumber
                && entity.PriceEpisodeCompletionElement == priceEpisode.CompletionAmount
                && entity.PriceEpisodeInstalmentValue == priceEpisode.MonthlyAmount
                && entity.PriceEpisodeTotalTNPPrice == priceEpisode.NegotiatedPrice
                && entity.TNP1 == priceEpisode.Tnp1
                && entity.TNP2 == priceEpisode.Tnp2
                && entity.TNP3 == priceEpisode.Tnp3
                && entity.TNP4 == priceEpisode.Tnp4;
        }
    }
}