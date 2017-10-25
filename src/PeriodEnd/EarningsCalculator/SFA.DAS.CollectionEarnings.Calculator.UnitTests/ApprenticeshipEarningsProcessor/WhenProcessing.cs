using System;
using MediatR;
using Moq;
using NLog;
using NUnit.Framework;
using SFA.DAS.CollectionEarnings.Calculator.Application.ApprenticeshipPriceEpisode;
using SFA.DAS.CollectionEarnings.Calculator.Application.ApprenticeshipPriceEpisode.ProcessAddPriceEpisodesCommand;
using SFA.DAS.CollectionEarnings.Calculator.Application.ApprenticeshipPriceEpisodePeriod;
using SFA.DAS.CollectionEarnings.Calculator.Application.ApprenticeshipPriceEpisodePeriod.AddPriceEpisodePeriodCommand;
using SFA.DAS.CollectionEarnings.Calculator.Application.ApprenticeshipPriceEpisodePeriodisedValues;
using SFA.DAS.CollectionEarnings.Calculator.Application.ApprenticeshipPriceEpisodePeriodisedValues.ProcessAddPeriodisedValuesCommand;
using SFA.DAS.CollectionEarnings.Calculator.Application.EarningsCalculation.GetLearningDeliveriesEarningsQuery;
using SFA.DAS.CollectionEarnings.Calculator.Application.LearningDeliveryToProcess;
using SFA.DAS.CollectionEarnings.Calculator.Application.LearningDeliveryToProcess.GetAllLearningDeliveriesToProcessQuery;
using SFA.DAS.CollectionEarnings.Calculator.UnitTests.Tools.Builders;

namespace SFA.DAS.CollectionEarnings.Calculator.UnitTests.ApprenticeshipEarningsProcessor
{
    public class WhenProcessing
    {
        private static readonly object[] EmptyItems =
        {
            new object[] {null},
            new object[] {new LearningDelivery[] {}}
        };

        private Calculator.ApprenticeshipEarningsProcessor _processor;
        private Mock<ILogger> _logger;
        private Mock<IMediator> _mediator;

        [SetUp]
        public void Arrange()
        {
            _logger = new Mock<ILogger>();
            _mediator = new Mock<IMediator>();

            _processor = new Calculator.ApprenticeshipEarningsProcessor(_logger.Object, _mediator.Object);

            InitialMockSetup();
        }

        private void InitialMockSetup()
        {
            // Mediator
            _mediator
                .Setup(m => m.Send(It.IsAny<GetAllLearningDeliveriesToProcessQueryRequest>()))
                .Returns(new GetAllLearningDeliveriesToProcessQueryResponse
                    {
                        IsValid = true,
                        Items = new[]
                                {
                                    new LearningDeliveryBuilder().Build()
                                }
                    }
                );

            _mediator
                .Setup(m => m.Send(It.IsAny<GetLearningDeliveriesEarningsQueryRequest>()))
                .Returns(new GetLearningDeliveriesEarningsQueryResponse
                    {
                        IsValid = true,
                        PriceEpisodes = new [] { new ApprenticeshipPriceEpisode() },
                        PriceEpisodesPeriodisedValues = new [] { new ApprenticeshipPriceEpisodePeriodisedValues() },
                        PriceEpisodesPeriodsEarnings = new [] { new ApprenticeshipPriceEpisodePeriod() }
                    }
                );

            _mediator
                .Setup(m => m.Send(It.IsAny<ProcessAddPriceEpisodesCommandRequest>()))
                .Returns(Unit.Value);

            _mediator
                .Setup(m => m.Send(It.IsAny<ProcessAddPeriodisedValuesCommandRequest>()))
                .Returns(Unit.Value);

            _mediator
                .Setup(m => m.Send(It.IsAny<AddPriceEpisodePeriodCommandRequest>()))
                .Returns(Unit.Value);
        }

        [Test]
        public void ThenExpectingExceptionForGetAllLearningDeliveriesToProcessQueryFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetAllLearningDeliveriesToProcessQueryRequest>()))
                .Returns(new GetAllLearningDeliveriesToProcessQueryResponse
                    {
                        IsValid = false,
                        Exception = new Exception(EarningsCalculatorException.ErrorReadingLearningDeliveriesToProcessMessage)
                    }
                );

            // Assert
            var ex = Assert.Throws<EarningsCalculatorException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(EarningsCalculatorException.ErrorReadingLearningDeliveriesToProcessMessage));
        }

        [Test]
        public void ThenExpectingExceptionForGetLearningDeliveriesEarningsQueryFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetLearningDeliveriesEarningsQueryRequest>()))
                .Returns(new GetLearningDeliveriesEarningsQueryResponse
                    {
                        IsValid = false,
                        Exception = new Exception(EarningsCalculatorException.ErrorCalculatingEarningsForTheLearningDeliveriesMessage)
                    }
                );

            // Assert
            var ex = Assert.Throws<EarningsCalculatorException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(EarningsCalculatorException.ErrorCalculatingEarningsForTheLearningDeliveriesMessage));
        }

        [Test]
        public void ThenExpectingExceptionForAddProcessedLearningDeliveriesCommandFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<ProcessAddPriceEpisodesCommandRequest>()))
                .Throws<Exception>();

            // Assert
            var ex = Assert.Throws<EarningsCalculatorException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(EarningsCalculatorException.ErrorWritingProcessedLearningDeliveriesMessage));
        }

        [Test]
        public void ThenExpectingExceptionForAddProcessedLearningDeliveryPeriodisedValuesCommandFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<ProcessAddPeriodisedValuesCommandRequest>()))
                .Throws<Exception>();

            // Assert
            var ex = Assert.Throws<EarningsCalculatorException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(EarningsCalculatorException.ErrorWritingProcessedLearningDeliveryPeriodisedValuesMessage));
        }

        [Test]

        [TestCaseSource(nameof(EmptyItems))]
        public void ThenExpectingLogEntryForGetAllLearningDeliveriesToProcessQueryNoItems(LearningDelivery[] items)
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<GetAllLearningDeliveriesToProcessQueryRequest>()))
                .Returns(new GetAllLearningDeliveriesToProcessQueryResponse
                    {
                        IsValid = true,
                        Items = items
                    }
                );

            // Act
            _processor.Process();

            // Assert
            _logger.Verify(l => l.Info(It.Is<string>(p => p.Equals("Not found any learning deliveries to process."))), Times.Once);
        }

        [Test]
        public void ThenExpectingExceptionForAddProcessedLearningDeliveryPeriodCommandFailure()
        {
            // Arrange
            _mediator
                .Setup(m => m.Send(It.IsAny<AddPriceEpisodePeriodCommandRequest>()))
                .Throws<Exception>();

            // Assert
            var ex = Assert.Throws<EarningsCalculatorException>(() => _processor.Process());
            Assert.IsTrue(ex.Message.Contains(EarningsCalculatorException.ErrorWritingLearningDeliveriesPeriodEarningsMessage));
        }
    }
}