using System;
using MediatR;
using NLog;
using SFA.DAS.CollectionEarnings.Calculator.Application.ApprenticeshipPriceEpisode;
using SFA.DAS.CollectionEarnings.Calculator.Application.ApprenticeshipPriceEpisode.ProcessAddPriceEpisodesCommand;
using SFA.DAS.CollectionEarnings.Calculator.Application.ApprenticeshipPriceEpisodePeriod;
using SFA.DAS.CollectionEarnings.Calculator.Application.ApprenticeshipPriceEpisodePeriod.AddPriceEpisodePeriodCommand;
using SFA.DAS.CollectionEarnings.Calculator.Application.ApprenticeshipPriceEpisodePeriodisedValues;
using SFA.DAS.CollectionEarnings.Calculator.Application.ApprenticeshipPriceEpisodePeriodisedValues.ProcessAddPeriodisedValuesCommand;
using SFA.DAS.CollectionEarnings.Calculator.Application.EarningsCalculation.GetLearningDeliveriesEarningsQuery;
using SFA.DAS.CollectionEarnings.Calculator.Application.LearningDeliveryToProcess;
using SFA.DAS.CollectionEarnings.Calculator.Application.LearningDeliveryToProcess.GetAllLearningDeliveriesToProcessQuery;

namespace SFA.DAS.CollectionEarnings.Calculator
{
    public class ApprenticeshipEarningsProcessor
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public ApprenticeshipEarningsProcessor(ILogger logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        protected ApprenticeshipEarningsProcessor()
        {
        }

        public virtual void Process()
        {
            _logger.Info("Started Apprenticeship Earnings Processor.");

            var learningDeliveriesToProcess = ReturnLearningDeliveriesToProcessOrThrow();

            if (learningDeliveriesToProcess.HasAnyItems())
            {
                var processedEarnings = CalculateEarningsOrThrow(learningDeliveriesToProcess.Items);

                WriteApprenticeshipPriceEpisodesOrThrow(processedEarnings.PriceEpisodes);
                WriteApprenticeshipPriceEpisodePeriodisedValuesOrThrow(processedEarnings.PriceEpisodesPeriodisedValues);
                WriteApprenticeshipPriceEpisodePeriodEarningsOrThrow(processedEarnings.PriceEpisodesPeriodsEarnings);
            }
            else
            {
                _logger.Info("Not found any learning deliveries to process.");
            }

            _logger.Info("Finished Apprenticeship Earnings Processor.");
        }

        private GetAllLearningDeliveriesToProcessQueryResponse ReturnLearningDeliveriesToProcessOrThrow()
        {
            _logger.Debug("Reading learning deliveries to process.");

            var learningDeliveries = _mediator.Send(new GetAllLearningDeliveriesToProcessQueryRequest());

            if (!learningDeliveries.IsValid)
            {
                throw new EarningsCalculatorException(
                    EarningsCalculatorException.ErrorReadingLearningDeliveriesToProcessMessage, learningDeliveries.Exception);
            }

            return learningDeliveries;
        }

        private GetLearningDeliveriesEarningsQueryResponse CalculateEarningsOrThrow(LearningDelivery[] learningDeliveriesToProcess)
        {
            _logger.Debug("Started calculating the earnings for the found learning deliveries.");

            var earnings = _mediator.Send(new GetLearningDeliveriesEarningsQueryRequest
            {
                LearningDeliveries = learningDeliveriesToProcess
            });

            _logger.Debug("Finished calculating the earnings for the found learning deliveries.");

            if (!earnings.IsValid)
            {
                throw new EarningsCalculatorException(EarningsCalculatorException.ErrorCalculatingEarningsForTheLearningDeliveriesMessage, earnings.Exception);
            }

            return earnings;
        }

        private void WriteApprenticeshipPriceEpisodesOrThrow(ApprenticeshipPriceEpisode[] priceEpisodes)
        {
            _logger.Debug("Started writing processed learning deliveries.");

            try
            {
                _mediator.Send(new ProcessAddPriceEpisodesCommandRequest
                {
                    PriceEpisodes = priceEpisodes
                });
            }
            catch (Exception ex)
            {
                throw new EarningsCalculatorException(EarningsCalculatorException.ErrorWritingProcessedLearningDeliveriesMessage, ex);
            }

            _logger.Debug("Finished writing processed learning deliveries.");
        }

        private void WriteApprenticeshipPriceEpisodePeriodisedValuesOrThrow(ApprenticeshipPriceEpisodePeriodisedValues[] periodisedValues)
        {
            _logger.Debug("Started writing processed learning deliveries periodised values.");

            try
            {
                _mediator.Send(new ProcessAddPeriodisedValuesCommandRequest
                {
                    PeriodisedValues = periodisedValues
                });
            }
            catch (Exception ex)
            {
                throw new EarningsCalculatorException(EarningsCalculatorException.ErrorWritingProcessedLearningDeliveryPeriodisedValuesMessage, ex);
            }

            _logger.Debug("Finished writing processed learning deliveries periodised values.");
        }

        private void WriteApprenticeshipPriceEpisodePeriodEarningsOrThrow(ApprenticeshipPriceEpisodePeriod[] periodEarnings)
        {
            _logger.Debug("Started writing learning deliveries period earnings.");

            try
            {
                _mediator.Send(new AddPriceEpisodePeriodCommandRequest
                {
                    PeriodEarnings = periodEarnings
                });
            }
            catch (Exception ex)
            {
                throw new EarningsCalculatorException(EarningsCalculatorException.ErrorWritingLearningDeliveriesPeriodEarningsMessage, ex);
            }

            _logger.Debug("Finished writing learning deliveries period earnings.");
        }
    }
}