using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using NLog;
using SFA.DAS.ProviderPayments.Calc.Common.Tools.Extensions;
using SFA.DAS.ProviderPayments.Calc.PaymentSchedule.Application.CollectionPeriods;
using SFA.DAS.ProviderPayments.Calc.PaymentSchedule.Application.CollectionPeriods.GetCurrentCollectionPeriodQuery;
using SFA.DAS.ProviderPayments.Calc.PaymentSchedule.Application.Earnings.GetProviderEarningsQuery;
using SFA.DAS.ProviderPayments.Calc.PaymentSchedule.Application.Providers.GetProvidersQuery;
using SFA.DAS.ProviderPayments.Calc.PaymentSchedule.Application.Earnings;
using SFA.DAS.ProviderPayments.Calc.PaymentSchedule.Application.RequiredPayments;
using SFA.DAS.ProviderPayments.Calc.PaymentSchedule.Application.RequiredPayments.AddRequiredPaymentsCommand;

namespace SFA.DAS.ProviderPayments.Calc.PaymentSchedule
{
    public class PaymentScheduleProcessor
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public PaymentScheduleProcessor(ILogger logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        protected PaymentScheduleProcessor()
        {
            // So we can mock
        }

        public virtual void Process()
        {
            _logger.Info("Started Payment Schedule Processor.");

            var collectionPeriod = _mediator.Send(new GetCurrentCollectionPeriodQueryRequest());

            if (!collectionPeriod.IsValid)
            {
                // TODO: Investigate if we want to throw the exception or not
                throw new PaymentScheduleProcessorException(PaymentScheduleProcessorException.ErrorReadingCollectionPeriodMessage, collectionPeriod.Exception);
            }

            if (collectionPeriod.Period == null)
            {
                // TODO: Investigate if we want to throw the exception or not
                throw new PaymentScheduleProcessorException(PaymentScheduleProcessorException.ErrorNoCollectionPeriodMessage);
            }

            var providers = _mediator.Send(new GetProvidersQueryRequest());

            if (!providers.IsValid)
            {
                // TODO: Investigate if we want to throw the exception or not
                throw new PaymentScheduleProcessorException(PaymentScheduleProcessorException.ErrorReadingProvidersMessage, providers.Exception);
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
                        // TODO: Investigate if we want to throw the exception or not
                        throw new PaymentScheduleProcessorException(PaymentScheduleProcessorException.ErrorReadingProviderEarningsMessage, providerEarnings.Exception);
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
                        // TODO: Investigate if we want to throw the exception or not
                        throw new PaymentScheduleProcessorException(PaymentScheduleProcessorException.ErrorWritingRequiredProviderPaymentsMessage, writeRequiredPaymentsResponse.Exception);
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

        private bool ShouldSchedulePayment(CollectionPeriod period, PeriodEarning earning)
        {
            var periodDate = new DateTime(period.Year, period.Month, 1).LastDayOfMonth();

            return earning.LearningStartDate <= periodDate &&
                periodDate <= earning.LearningPlannedEndDate;
        }

        private bool HasCompletedOnCensusDate(PeriodEarning earning)
        {
            if (!earning.LearningActualEndDate.HasValue)
            {
                return false;
            }

            return earning.LearningActualEndDate.Value == earning.LearningActualEndDate.Value.LastDayOfMonth();
        }

        private RequiredPayment SchedulePayment(CollectionPeriod period, PeriodEarning earning, decimal amount, TransactionType transactionType)
        {
            _logger.Info($"Scheduling a payment of {amount} for provider with ukprn {earning.Ukprn} to pay for {transactionType} on learner {earning.LearnerRefNumber} / {earning.AimSequenceNumber} in period {period.Month} / {period.Year}.");

            return new RequiredPayment
            {
                LearnerRefNumber = earning.LearnerRefNumber,
                AimSequenceNumber = earning.AimSequenceNumber,
                Ukprn = earning.Ukprn,
                DeliveryMonth = period.Month,
                DeliveryYear = period.Year,
                TransactionType = (int) transactionType,
                AmountDue = amount
            };
        }
    }
}
