using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.Application.ApprenticeshipPriceEpisodePeriod.AddPriceEpisodePeriodCommand;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.Calculator.UnitTests.Application.ApprenticeshipPriceEpisodePeriod.AddPriceEpisodePeriodCommand.AddPriceEpisodePeriodCommandHandler
{
    public class WhenHandling
    {
        private static readonly Calculator.Application.ApprenticeshipPriceEpisodePeriod.ApprenticeshipPriceEpisodePeriod[] PeriodEarnings =
        {
            new Calculator.Application.ApprenticeshipPriceEpisodePeriod.ApprenticeshipPriceEpisodePeriod
            {
                PriceEpisodeId = "550-20-6-2016-09-01",
                LearnerReferenceNumber = "Lrn001",
                AimSequenceNumber = 99,
                PriceEpisodeStartDate = new DateTime(2016, 9, 1),
                Period = 1,
                PriceEpisodeOnProgPayment = 1000.00m,
                PriceEpisodeBalancePayment = 0.00m,
                PriceEpisodeCompletionPayment = 0.00m
            },
            new Calculator.Application.ApprenticeshipPriceEpisodePeriod.ApprenticeshipPriceEpisodePeriod
            {
                PriceEpisodeId = "550-20-6-2016-09-01",
                LearnerReferenceNumber = "Lrn001",
                AimSequenceNumber = 99,
                PriceEpisodeStartDate = new DateTime(2016, 9, 1),
                Period = 2,
                PriceEpisodeOnProgPayment = 1000.00m,
                PriceEpisodeBalancePayment = 0.00m,
                PriceEpisodeCompletionPayment = 0.00m
            },
            new Calculator.Application.ApprenticeshipPriceEpisodePeriod.ApprenticeshipPriceEpisodePeriod
            {
                PriceEpisodeId = "550-20-6-2016-09-01",
                LearnerReferenceNumber = "Lrn001",
                AimSequenceNumber = 99,
                PriceEpisodeStartDate = new DateTime(2016, 9, 1),
                Period = 3,
                PriceEpisodeOnProgPayment = 1000.00m,
                PriceEpisodeBalancePayment = 0.00m,
                PriceEpisodeCompletionPayment = 0.00m
            }
        };

        private Mock<IApprenticeshipPriceEpisodePeriodRepository> _repository;

        private AddPriceEpisodePeriodCommandRequest _request;
        private Calculator.Application.ApprenticeshipPriceEpisodePeriod.AddPriceEpisodePeriodCommand.AddPriceEpisodePeriodCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _repository = new Mock<IApprenticeshipPriceEpisodePeriodRepository>();

            _request = new AddPriceEpisodePeriodCommandRequest
            {
                PeriodEarnings = PeriodEarnings
            };

            _handler = new Calculator.Application.ApprenticeshipPriceEpisodePeriod.AddPriceEpisodePeriodCommand.AddPriceEpisodePeriodCommandHandler(_repository.Object);
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
        public void ThenItShouldWriteTheLearningDeliveryPeriodEarningsToTheRepository()
        {
            // Act
            _handler.Handle(_request);

            // Assert
            _repository.Verify(r => r.AddApprenticeshipPriceEpisodePeriod(It.Is<ApprenticeshipPriceEpisodePeriodEntity[]>(l => PeriodEarningsBatchesMatch(l, PeriodEarnings))), Times.Once);
        }

        [Test]
        public void ThenExceptionIsThrownForInvalidRepositoryResponse()
        {
            // Arrange
            _repository
                .Setup(r => r.AddApprenticeshipPriceEpisodePeriod(It.IsAny<ApprenticeshipPriceEpisodePeriodEntity[]>()))
                .Throws<Exception>();

            // Assert
            Assert.Throws<Exception>(() => _handler.Handle(_request));
        }

        private bool PeriodEarningsBatchesMatch(ApprenticeshipPriceEpisodePeriodEntity[] entities, Calculator.Application.ApprenticeshipPriceEpisodePeriod.ApprenticeshipPriceEpisodePeriod[] periodEarnings)
        {
            if (entities.Length != periodEarnings.Length)
            {
                return false;
            }

            for (var x = 0; x < entities.Length; x++)
            {
                if (!PeriodEarningsMatch(entities[x], periodEarnings[x]))
                {
                    return false;
                }
            }

            return true;
        }

        private bool PeriodEarningsMatch(ApprenticeshipPriceEpisodePeriodEntity entity, Calculator.Application.ApprenticeshipPriceEpisodePeriod.ApprenticeshipPriceEpisodePeriod periodEarning)
        {
            return entity.PriceEpisodeIdentifier == periodEarning.PriceEpisodeId &&
                   entity.LearnRefNumber == periodEarning.LearnerReferenceNumber &&
                   entity.Period == periodEarning.Period &&
                   entity.PriceEpisodeOnProgPayment == periodEarning.PriceEpisodeOnProgPayment &&
                   entity.PriceEpisodeCompletionPayment == periodEarning.PriceEpisodeCompletionPayment &&
                   entity.PriceEpisodeBalancePayment == periodEarning.PriceEpisodeBalancePayment;
        }
    }
}