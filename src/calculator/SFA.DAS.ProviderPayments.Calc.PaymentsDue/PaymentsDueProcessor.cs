using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using NLog;
using SFA.DAS.ProviderPayments.Calc.Common.Tools.Extensions;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.CollectionPeriods;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.CollectionPeriods.GetCurrentCollectionPeriodQuery;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Earnings;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Earnings.GetProviderEarningsQuery;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Providers.GetProvidersQuery;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments.AddRequiredPaymentsCommand;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue
{
    public class PaymentsDueProcessor
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public PaymentsDueProcessor(ILogger logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        protected PaymentsDueProcessor()
        {
            // So we can mock
        }

        public virtual void Process()
        {
            _logger.Info("Started Payment Schedule Processor.");

            var collectionPeriod = _mediator.Send(new GetCurrentCollectionPeriodQueryRequest());

            if (!collectionPeriod.IsValid)
            {
                throw new PaymentsDueProcessorException(PaymentsDueProcessorException.ErrorReadingCollectionPeriodMessage, collectionPeriod.Exception);
            }

            if (collectionPeriod.Period == null)
            {
                throw new PaymentsDueProcessorException(PaymentsDueProcessorException.ErrorNoCollectionPeriodMessage);
            }

            var providers = _mediator.Send(new GetProvidersQueryRequest());

            if (!providers.IsValid)
            {
                throw new PaymentsDueProcessorException(PaymentsDueProcessorException.ErrorReadingProvidersMessage, providers.Exception);
            }

            if (providers.Items != null && providers.Items.Any())
            {
                foreach (var provider in providers.Items)
                {
                    _logger.Info($"Processing provider with ukprn {provider.Ukprn}.");

                    var providerPayments = new List<RequiredPayment>();

                    var providerEarnings = _mediator.Send(new GetProviderEarningsQueryRequest {Ukprn = provider.Ukprn});

                    if (!providerEarnings.IsValid)
                    {
                        throw new PaymentsDueProcessorException(PaymentsDueProcessorException.ErrorReadingProviderEarningsMessage, providerEarnings.Exception);
                    }

                    if (providerEarnings.Items == null || !providerEarnings.Items.Any())
                    {
                        _logger.Info($"No earnings found for provider with ukprn {provider.Ukprn}.");
                        continue;
                    }

                    foreach (var earning in providerEarnings.Items)
                    {
                        if (ShouldSchedulePayment(collectionPeriod.Period, earning))
                        {
                            var isComplete = earning.LearningActualEndDate.HasValue;
                            var isCompleteOnCensusDate = HasCompletedOnCensusDate(earning);

                            if (!isComplete || isCompleteOnCensusDate)
                            {
                                providerPayments.Add(SchedulePayment(collectionPeriod.Period, earning, earning.MonthlyInstallment, TransactionType.Learning));
                            }
                            if (isComplete)
                            {
                                providerPayments.Add(SchedulePayment(collectionPeriod.Period, earning, earning.CompletionPayment, TransactionType.Completion));
                            }
                        }
                    }

                    _logger.Info($"Writing {providerPayments.Count} scheduled payments for provider with ukprn {provider.Ukprn}.");

                    var writeRequiredPaymentsResponse = _mediator.Send(new AddRequiredPaymentsCommandRequest {Payments = providerPayments.ToArray()});

                    if (!writeRequiredPaymentsResponse.IsValid)
                    {
                        throw new PaymentsDueProcessorException(PaymentsDueProcessorException.ErrorWritingRequiredProviderPaymentsMessage, writeRequiredPaymentsResponse.Exception);
                    }

                    _logger.Info($"Finished processing provider with ukprn {provider.Ukprn}.");
                }
            }
            else
            {
                _logger.Info("No providers found to process.");
            }

            _logger.Info("Finished Payment Schedule Processor.");
        }

        private bool ShouldSchedulePayment(CollectionPeriod period, Earning earning)
        {
            var periodDate = new DateTime(period.Year, period.Month, 1).LastDayOfMonth();

            return earning.LearningStartDate <= periodDate &&
                periodDate <= earning.LearningPlannedEndDate;
        }

        private bool HasCompletedOnCensusDate(Earning earning)
        {
            if (!earning.LearningActualEndDate.HasValue)
            {
                return false;
            }

            return earning.LearningActualEndDate.Value == earning.LearningActualEndDate.Value.LastDayOfMonth();
        }

        private RequiredPayment SchedulePayment(CollectionPeriod period, Earning earning, decimal amount, TransactionType transactionType)
        {
            _logger.Info($"Scheduling a payment of {amount} for provider with ukprn {earning.Ukprn} to pay for {transactionType} on learner {earning.LearnerRefNumber} / {earning.AimSequenceNumber} in period {period.Month} / {period.Year}.");

            return new RequiredPayment
            {
                LearnerRefNumber = earning.LearnerRefNumber,
                AimSequenceNumber = earning.AimSequenceNumber,
                Ukprn = earning.Ukprn,
                DeliveryMonth = period.Month,
                DeliveryYear = period.Year,
                TransactionType = transactionType,
                AmountDue = amount
            };
        }
    }
}
