using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using NLog;
using SFA.DAS.ProviderPayments.Calc.Common.Context;
using SFA.DAS.ProviderPayments.Calc.Common.Tools.Extensions;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.CollectionPeriods;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.CollectionPeriods.GetCurrentCollectionPeriodQuery;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Earnings;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Earnings.GetProviderEarningsQuery;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Providers;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Providers.GetProvidersQuery;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments.AddRequiredPaymentsCommand;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue
{
    public class PaymentsDueProcessor
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly ContextWrapper _context;

        public PaymentsDueProcessor(ILogger logger, IMediator mediator, ContextWrapper context)
        {
            _logger = logger;
            _mediator = mediator;
            _context = context;
        }
        protected PaymentsDueProcessor()
        {
            // So we can mock
        }

        public virtual void Process()
        {
            _logger.Info("Started Payments Due Processor.");

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
                    ProcessProvider(provider, collectionPeriod.Period);
                }
            }
            else
            {
                _logger.Info("No providers found to process.");
            }

            _logger.Info("Finished Payments Due Processor.");
        }




        private void ProcessProvider(Provider provider, CollectionPeriod currentPeriod)
        {
            _logger.Info($"Processing provider with ukprn {provider.Ukprn}.");

            int period1Month = currentPeriod.Month - (currentPeriod.PeriodNumber - 1);
            int period1Year = period1Month > 0 ? currentPeriod.Year : currentPeriod.Year - 1;
            if (period1Month < 1)
            {
                period1Month = 12 - period1Month;
            }

            var earningResponse = _mediator.Send(new GetProviderEarningsQueryRequest
            {
                Ukprn = provider.Ukprn,
                Period1Month = period1Month,
                Period1Year = period1Year,
                AcademicYear = _context.GetPropertyValue(ContextPropertyKeys.YearOfCollection)
            });
            if (!earningResponse.IsValid)
            {
                throw new PaymentsDueProcessorException(PaymentsDueProcessorException.ErrorReadingProviderEarningsMessage, earningResponse.Exception);
            }

            var paymentsDue = new List<RequiredPayment>();
            foreach (var earning in earningResponse.Items)
            {
                if (earning.CalendarYear > currentPeriod.Year
                    || (earning.CalendarYear == currentPeriod.Year && earning.CalendarMonth > currentPeriod.Month))
                {
                    continue;
                }

                paymentsDue.Add(new RequiredPayment
                {
                    CommitmentId = earning.CommitmentId,
                    Ukprn = earning.Ukprn,
                    LearnerRefNumber = earning.LearnerReferenceNumber,
                    AimSequenceNumber = earning.AimSequenceNumber,
                    DeliveryMonth = earning.CalendarMonth,
                    DeliveryYear = earning.CalendarYear,
                    AmountDue = earning.EarnedValue,
                    TransactionType = (TransactionType)(int)earning.Type
                });
            }

            if (paymentsDue.Any())
            {
                var addPaymentsDueResponse = _mediator.Send(new AddRequiredPaymentsCommandRequest { Payments = paymentsDue.ToArray() });
                if (!addPaymentsDueResponse.IsValid)
                {
                    throw new PaymentsDueProcessorException(PaymentsDueProcessorException.ErrorWritingRequiredProviderPaymentsMessage, addPaymentsDueResponse.Exception);
                }
            }

            _logger.Info($"Finished processing provider with ukprn {provider.Ukprn}.");
        }



        private bool SummarisationPeriodIsAfterLearningStartDate(CollectionPeriod period, DateTime learningStartDate)
        {
            var periodDate = new DateTime(period.Year, period.Month, 1).LastDayOfMonth();

            return learningStartDate <= periodDate;
        }

        private bool SummarisationPeriodIsBeforeLearningEndDate(CollectionPeriod period, DateTime learningEndDate)
        {
            var periodDate = new DateTime(period.Year, period.Month, 1);

            return periodDate <= learningEndDate;
        }
    }
}
