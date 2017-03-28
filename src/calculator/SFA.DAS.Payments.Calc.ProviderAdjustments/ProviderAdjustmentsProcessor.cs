using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using NLog;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Application.Adjustments;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Application.Adjustments.GetCurrentAdjustmentsQuery;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Application.Adjustments.GetPreviousAdjustmentsQuery;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Application.CollectionPeriods;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Application.CollectionPeriods.GetCurrentCollectionPeriodQuery;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Application.Payments;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Application.Payments.AddPaymentsCommand;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Application.Providers;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Application.Providers.GetProvidersQuery;

namespace SFA.DAS.Payments.Calc.ProviderAdjustments
{
    public class ProviderAdjustmentsProcessor
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly string _yearOfCollection;

        public ProviderAdjustmentsProcessor(ILogger logger, IMediator mediator, string yearOfCollection)
        {
            _logger = logger;
            _mediator = mediator;
            _yearOfCollection = yearOfCollection;
        }
        protected ProviderAdjustmentsProcessor()
        {
            // So we can mock
        }

        public virtual void Process()
        {
            _logger.Info("Started the Provider Adjustments Processor.");

            var collectionPeriod = ReturnValidGetCurrentCollectionPeriodQueryResponseOrThrow();
            var providersQueryResponse = ReturnValidGetProvidersQueryResponseOrThrow();

            if (providersQueryResponse.HasAnyItems())
            {
                foreach (var provider in providersQueryResponse.Items)
                {
                    _logger.Info($"Processing adjustments for provider with ukprn {provider.Ukprn}.");

                    var currentAdjustmentsResponse = ReturnValidGetCurrentAdjustmentsQueryResponseOrThrow(provider.Ukprn);
                    var previousPaymentsResponse = ReturnValidGetPreviousAdjustmentsQueryResponseOrThrow(provider.Ukprn);

                    ProcessAdjustmentsForProvider(provider, currentAdjustmentsResponse.Items, previousPaymentsResponse.Items, collectionPeriod.Period);

                    _logger.Info($"Finished processing adjustments for provider with ukprn {provider.Ukprn}.");
                }
            }
            else
            {
                _logger.Info("No providers found to process.");
            }

            _logger.Info("Finished the Provider Adjustments Processor.");
        }

        private GetCurrentCollectionPeriodQueryResponse ReturnValidGetCurrentCollectionPeriodQueryResponseOrThrow()
        {
            _logger.Info("Reading current collection period.");

            var response = _mediator.Send(new GetCurrentCollectionPeriodQueryRequest());

            if (!response.IsValid)
            {
                throw new ProviderAdjustmentsProcessorException(ProviderAdjustmentsProcessorException.ErrorReadingCollectionPeriodMessage, response.Exception);
            }

            if (response.Period == null)
            {
                throw new ProviderAdjustmentsProcessorException(ProviderAdjustmentsProcessorException.ErrorNoCollectionPeriodMessage);
            }

            return response;
        }

        private GetProvidersQueryResponse ReturnValidGetProvidersQueryResponseOrThrow()
        {
            _logger.Info("Reading list of providers to process.");

            var response = _mediator.Send(new GetProvidersQueryRequest());

            if (!response.IsValid)
            {
                throw new ProviderAdjustmentsProcessorException(ProviderAdjustmentsProcessorException.ErrorReadingProvidersMessage, response.Exception);
            }

            return response;
        }

        private GetCurrentAdjustmentsQueryResponse ReturnValidGetCurrentAdjustmentsQueryResponseOrThrow(long ukprn)
        {
            _logger.Info($"Reading current adjustments for provider with ukprn {ukprn}.");

            var response = _mediator.Send(new GetCurrentAdjustmentsQueryRequest
            {
                Ukprn = ukprn
            });

            if (!response.IsValid)
            {
                throw new ProviderAdjustmentsProcessorException(ProviderAdjustmentsProcessorException.ErrorReadingCurrentAdjustmentsMessage, response.Exception);
            }

            return response;
        }

        private GetPreviousAdjustmentsQueryResponse ReturnValidGetPreviousAdjustmentsQueryResponseOrThrow(long ukprn)
        {
            _logger.Info($"Reading previous adjustments for provider with ukprn {ukprn}.");

            var response = _mediator.Send(new GetPreviousAdjustmentsQueryRequest
            {
                Ukprn = ukprn
            });

            if (!response.IsValid)
            {
                throw new ProviderAdjustmentsProcessorException(ProviderAdjustmentsProcessorException.ErrorReadingPreviousAdjustmentsMessage, response.Exception);
            }

            return response;
        }

        private void ProcessAdjustmentsForProvider(Provider provider, Adjustment[] currentAdjustments, Adjustment[] previousAdjustments, CollectionPeriod period)
        {
            _logger.Info($"Calculating new adjustments for provider with ukprn {provider.Ukprn} for period {_yearOfCollection}-{period.Name}.");

            var payments = new List<Payment>();

            if (currentAdjustments != null && currentAdjustments.Length > 0)
            {
                CalculatePaymentsFromCurrentAndPreviousAdjustments(payments, provider, currentAdjustments, previousAdjustments, period);
            }
            else
            {
                _logger.Info($"No current adjustments for provider with ukprn {provider.Ukprn} found.");
            }

            _logger.Info($"Started writing adjustments for provider with ukprn {provider.Ukprn} for period {_yearOfCollection}-{period.Name}.");

            WriteProviderAdjustmentsOrThrow(payments.ToArray());

            _logger.Info($"Finished writing adjustments for provider with ukprn {provider.Ukprn} for period {_yearOfCollection}-{period.Name}.");
        }

        private void CalculatePaymentsFromCurrentAndPreviousAdjustments(List<Payment> payments, Provider provider,
            Adjustment[] currentAdjustments, Adjustment[] previousAdjustments, CollectionPeriod period)
        {
            foreach (var current in currentAdjustments)
            {
                var previous = previousAdjustments?
                    .Where(p =>
                        p.Ukprn == current.Ukprn &&
                        p.SubmissionCollectionPeriod == current.SubmissionCollectionPeriod &&
                        p.PaymentType == current.PaymentType)
                    .ToArray();

                var previousAmount = previous != null && previous.Length > 0
                    ? previous.Sum(p => p.Amount)
                    : 0.00m;

                var paymentAmount = current.Amount - previousAmount;

                payments.Add(new Payment
                {
                    Ukprn = provider.Ukprn,
                    SubmissionId = current.SubmissionId,
                    SubmissionCollectionPeriod = current.SubmissionCollectionPeriod,
                    SubmissionAcademicYear = int.Parse(_yearOfCollection),
                    PaymentType = current.PaymentType,
                    PaymentTypeName = current.PaymentTypeName,
                    Amount = paymentAmount,
                    CollectionPeriodName = $"{_yearOfCollection}-{period.Name}",
                    CollectionPeriodMonth = period.Month,
                    CollectionPeriodYear = period.Year
                });
            }
        }

        private void WriteProviderAdjustmentsOrThrow(Payment[] payments)
        {
            try
            {
                _mediator.Send(new AddPaymentsCommandRequest
                {
                    Payments = payments
                });
            }
            catch (Exception ex)
            {
                throw new ProviderAdjustmentsProcessorException(ProviderAdjustmentsProcessorException.ErrorWritingAdjustmentsMessage, ex);
            }
        }
    }
}