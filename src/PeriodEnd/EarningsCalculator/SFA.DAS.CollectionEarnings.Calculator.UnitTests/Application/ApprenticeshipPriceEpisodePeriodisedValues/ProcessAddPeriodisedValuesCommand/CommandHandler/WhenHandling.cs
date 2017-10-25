using System;
using Moq;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.Application.ApprenticeshipPriceEpisodePeriodisedValues;
using SFA.DAS.CollectionEarnings.Calculator.Application.ApprenticeshipPriceEpisodePeriodisedValues.ProcessAddPeriodisedValuesCommand;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.Calculator.UnitTests.Application.ApprenticeshipPriceEpisodePeriodisedValues.ProcessAddPeriodisedValuesCommand.CommandHandler
{
    public class WhenHandling
    {
        private static readonly Calculator.Application.ApprenticeshipPriceEpisodePeriodisedValues.ApprenticeshipPriceEpisodePeriodisedValues[] PeriodisedValues = new []
        {
            new Calculator.Application.ApprenticeshipPriceEpisodePeriodisedValues.ApprenticeshipPriceEpisodePeriodisedValues
            {
                PriceEpisodeId = "550-20-6-2016-09-01",
                LearnerReferenceNumber = "Lrn001",
                AimSequenceNumber = 1,
                PriceEpisodeStartDate = new DateTime(2016, 9, 1),
                AttributeName = AttributeNames.PriceEpisodeOnProgPayment,
                Period1Amount = 1000m,
                Period2Amount = 1000m,
                Period3Amount = 1000m,
                Period4Amount = 1000m,
                Period5Amount = 1000m,
                Period6Amount = 1000m,
                Period7Amount = 1000m,
                Period8Amount = 1000m,
                Period9Amount = 1000m,
                Period10Amount = 1000m,
                Period11Amount = 1000m,
                Period12Amount = 1000m
            },
            new Calculator.Application.ApprenticeshipPriceEpisodePeriodisedValues.ApprenticeshipPriceEpisodePeriodisedValues
            {
                PriceEpisodeId = "550-20-6-2016-09-01",
                LearnerReferenceNumber = "Lrn001",
                AimSequenceNumber = 2,
                PriceEpisodeStartDate = new DateTime(2016, 9, 1),
                AttributeName = AttributeNames.PriceEpisodeOnProgPayment,
                Period1Amount = 1000m,
                Period2Amount = 1000m,
                Period3Amount = 1000m,
                Period4Amount = 1000m,
                Period5Amount = 1000m,
                Period6Amount = 1000m,
                Period7Amount = 1000m,
                Period8Amount = 1000m,
                Period9Amount = 1000m,
                Period10Amount = 1000m,
                Period11Amount = 1000m,
                Period12Amount = 1000m
            },
            new Calculator.Application.ApprenticeshipPriceEpisodePeriodisedValues.ApprenticeshipPriceEpisodePeriodisedValues
            {
                PriceEpisodeId = "550-20-6-2016-09-01",
                LearnerReferenceNumber = "Lrn001",
                AimSequenceNumber = 3,
                PriceEpisodeStartDate = new DateTime(2016, 9, 1),
                AttributeName = AttributeNames.PriceEpisodeOnProgPayment,
                Period1Amount = 1000m,
                Period2Amount = 1000m,
                Period3Amount = 1000m,
                Period4Amount = 1000m,
                Period5Amount = 1000m,
                Period6Amount = 1000m,
                Period7Amount = 1000m,
                Period8Amount = 1000m,
                Period9Amount = 1000m,
                Period10Amount = 1000m,
                Period11Amount = 1000m,
                Period12Amount = 1000m
            },
            new Calculator.Application.ApprenticeshipPriceEpisodePeriodisedValues.ApprenticeshipPriceEpisodePeriodisedValues
            {
                PriceEpisodeId = "550-20-6-2016-09-01",
                LearnerReferenceNumber = "Lrn001",
                AimSequenceNumber = 4,
                PriceEpisodeStartDate = new DateTime(2016, 9, 1),
                AttributeName = AttributeNames.PriceEpisodeOnProgPayment,
                Period1Amount = 1000m,
                Period2Amount = 1000m,
                Period3Amount = 1000m,
                Period4Amount = 1000m,
                Period5Amount = 1000m,
                Period6Amount = 1000m,
                Period7Amount = 1000m,
                Period8Amount = 1000m,
                Period9Amount = 1000m,
                Period10Amount = 1000m,
                Period11Amount = 1000m,
                Period12Amount = 1000m
            },
            new Calculator.Application.ApprenticeshipPriceEpisodePeriodisedValues.ApprenticeshipPriceEpisodePeriodisedValues
            {
                PriceEpisodeId = "550-20-6-2016-09-01",
                LearnerReferenceNumber = "Lrn001",
                AimSequenceNumber = 5,
                PriceEpisodeStartDate = new DateTime(2016, 9, 1),
                AttributeName = AttributeNames.PriceEpisodeOnProgPayment,
                Period1Amount = 1000m,
                Period2Amount = 1000m,
                Period3Amount = 1000m,
                Period4Amount = 1000m,
                Period5Amount = 1000m,
                Period6Amount = 1000m,
                Period7Amount = 1000m,
                Period8Amount = 1000m,
                Period9Amount = 1000m,
                Period10Amount = 1000m,
                Period11Amount = 1000m,
                Period12Amount = 1000m
            }
        };

        private Mock<IApprenticeshipPriceEpisodePeriodisedValuesRepository> _repository;

        private ProcessAddPeriodisedValuesCommandRequest _request;
        private ProcessAddPeriodisedValuesCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _repository = new Mock<IApprenticeshipPriceEpisodePeriodisedValuesRepository>();

            _request = new ProcessAddPeriodisedValuesCommandRequest
            {
                PeriodisedValues = PeriodisedValues
            };

            _handler = new ProcessAddPeriodisedValuesCommandHandler(_repository.Object);
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
        public void ThenItShouldWriteThePeriodisedValuesToTheRepository()
        {
            // Act
            _handler.Handle(_request);

            // Assert
            _repository.Verify(r => r.AddApprenticeshipPriceEpisodePeriodisedValues(It.Is<ApprenticeshipPriceEpisodePeriodisedValuesEntity[]>(l => PeriodisedValuesBatchesMatch(l, PeriodisedValues))), Times.Once);
        }

        [Test]
        public void ThenInvalidResponseReturnedForInvalidRepositoryResponse()
        {
            // Arrange
            _repository
                .Setup(r => r.AddApprenticeshipPriceEpisodePeriodisedValues(It.IsAny<ApprenticeshipPriceEpisodePeriodisedValuesEntity[]>()))
                .Throws<Exception>();

            // Assert
            Assert.Throws<Exception>(() => _handler.Handle(_request));
        }

        private bool PeriodisedValuesBatchesMatch(ApprenticeshipPriceEpisodePeriodisedValuesEntity[] entities, Calculator.Application.ApprenticeshipPriceEpisodePeriodisedValues.ApprenticeshipPriceEpisodePeriodisedValues[] periodisedValues)
        {
            if (entities.Length != periodisedValues.Length)
            {
                return false;
            }

            for (var x = 0; x < entities.Length; x++)
            {
                if (!PeriodisedValuesMatch(entities[x], periodisedValues[x]))
                {
                    return false;
                }
            }

            return true;
        }

        private bool PeriodisedValuesMatch(ApprenticeshipPriceEpisodePeriodisedValuesEntity entity, Calculator.Application.ApprenticeshipPriceEpisodePeriodisedValues.ApprenticeshipPriceEpisodePeriodisedValues periodisedValues)
        {
            return entity.PriceEpisodeIdentifier == periodisedValues.PriceEpisodeId &&
                   entity.LearnRefNumber == periodisedValues.LearnerReferenceNumber &&
                   entity.AttributeName == periodisedValues.AttributeName.ToString() &&
                   entity.Period_1 == periodisedValues.Period1Amount &&
                   entity.Period_2 == periodisedValues.Period2Amount &&
                   entity.Period_3 == periodisedValues.Period3Amount &&
                   entity.Period_4 == periodisedValues.Period4Amount &&
                   entity.Period_5 == periodisedValues.Period5Amount &&
                   entity.Period_6 == periodisedValues.Period6Amount &&
                   entity.Period_7 == periodisedValues.Period7Amount &&
                   entity.Period_8 == periodisedValues.Period8Amount &&
                   entity.Period_9 == periodisedValues.Period9Amount &&
                   entity.Period_10 == periodisedValues.Period10Amount &&
                   entity.Period_11 == periodisedValues.Period11Amount &&
                   entity.Period_12 == periodisedValues.Period12Amount;
        }
    }
}