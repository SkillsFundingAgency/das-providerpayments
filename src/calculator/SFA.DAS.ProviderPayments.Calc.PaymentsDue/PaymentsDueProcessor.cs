using System.Collections.Generic;
using System.Linq;
using MediatR;
using NLog;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.CollectionPeriods;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.CollectionPeriods.GetCurrentCollectionPeriodQuery;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Earnings.GetProviderEarningsQuery;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Providers;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Providers.GetProvidersQuery;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments.AddRequiredPaymentsCommand;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments.GetPaymentHistoryQuery;

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

            var periodNumber = currentPeriod.PeriodNumber > 12 ? 12 : currentPeriod.PeriodNumber;
            int period1Month = currentPeriod.Month - (periodNumber - 1);
            int period1Year = period1Month > 0 ? currentPeriod.Year : currentPeriod.Year - 1;
            if (period1Month < 1)
            {
                period1Month = period1Month + 12;
            }

            var earningResponse = _mediator.Send(new GetProviderEarningsQueryRequest
            {
                Ukprn = provider.Ukprn,
                Period1Month = period1Month,
                Period1Year = period1Year,
                AcademicYear = _context.GetPropertyValue(Common.Context.ContextPropertyKeys.YearOfCollection)
            });
            if (!earningResponse.IsValid)
            {
                throw new PaymentsDueProcessorException(PaymentsDueProcessorException.ErrorReadingProviderEarningsMessage, earningResponse.Exception);
            }

            var paymentHistory = new List<RequiredPayment>();
            var commitmentIds = earningResponse.Items.Select(e => e.CommitmentId).Distinct().ToArray();
            foreach (var commitmentId in commitmentIds)
            {
                var historyResponse = _mediator.Send(new GetPaymentHistoryQueryRequest
                {
                    Ukprn = provider.Ukprn,
                    CommitmentId = commitmentId
                });
                if (!historyResponse.IsValid)
                {
                    throw new PaymentsDueProcessorException(PaymentsDueProcessorException.ErrorReadingPaymentHistoryMessage, historyResponse.Exception);
                }
                paymentHistory.AddRange(historyResponse.Items);
            }

            var paymentsDue = new List<RequiredPayment>();
            foreach (var earning in earningResponse.Items)
            {
                if (earning.CalendarYear > currentPeriod.Year
                    || (earning.CalendarYear == currentPeriod.Year && earning.CalendarMonth > currentPeriod.Month))
                {
                    continue;
                }

                var amountEarned = earning.EarnedValue;
                var alreadyPaid = paymentHistory
                    .Where(p => p.CommitmentId == earning.CommitmentId && p.DeliveryMonth == earning.CalendarMonth && p.DeliveryYear == earning.CalendarYear)
                    .Sum(p => p.AmountDue);
                var amountDue = amountEarned - alreadyPaid;

                if (amountDue > 0)
                {
                    paymentsDue.Add(new RequiredPayment
                    {
                        CommitmentId = earning.CommitmentId,
                        Ukprn = earning.Ukprn,
                        LearnerRefNumber = earning.LearnerReferenceNumber,
                        AimSequenceNumber = earning.AimSequenceNumber,
                        DeliveryMonth = earning.CalendarMonth,
                        DeliveryYear = earning.CalendarYear,
                        AmountDue = amountDue,
                        TransactionType = (TransactionType)(int)earning.Type
                    });
                }
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
    }
}
