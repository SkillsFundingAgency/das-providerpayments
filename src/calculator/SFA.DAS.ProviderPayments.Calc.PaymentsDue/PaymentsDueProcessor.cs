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
            var period1Month = currentPeriod.Month - (periodNumber - 1);
            var period1Year = period1Month > 0 ? currentPeriod.Year : currentPeriod.Year - 1;
            if (period1Month < 1)
            {
                period1Month = period1Month + 12;
            }

            var earningResponse = _mediator.Send(new GetProviderEarningsQueryRequest
            {
                Ukprn = provider.Ukprn,
                Period1Month = period1Month,
                Period1Year = period1Year,
                AcademicYear = _context.GetPropertyValue(Common.Context.PaymentsContextPropertyKeys.YearOfCollection)
            });

            if (!earningResponse.IsValid)
            {
                throw new PaymentsDueProcessorException(PaymentsDueProcessorException.ErrorReadingProviderEarningsMessage, earningResponse.Exception);
            }

            var paymentsDue = new List<RequiredPayment>();

            GetPaymentsDue(provider, currentPeriod, earningResponse, paymentsDue);

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

        

        private void GetPaymentsDue(Provider provider, CollectionPeriod currentPeriod, 
                                    GetProviderEarningsQueryResponse earningResponse, List<RequiredPayment> paymentsDue)
        {
            var paymentHistory = new List<RequiredPayment>();
            var earningsData = earningResponse.Items
                .Select(e =>
                    new
                    {
                        e.Ukprn,
                        e.Uln
                    })
                .Distinct()
                .ToArray();

            foreach (var earningItem in earningsData)
            {
                var historyResponse = _mediator.Send(new GetPaymentHistoryQueryRequest
                {
                    Ukprn = provider.Ukprn,
                    Uln = earningItem.Uln
                });
                if (!historyResponse.IsValid)
                {
                    throw new PaymentsDueProcessorException(PaymentsDueProcessorException.ErrorReadingPaymentHistoryMessage, historyResponse.Exception);
                }
                paymentHistory.AddRange(historyResponse.Items);
            }


            foreach (var earning in earningResponse.Items)
            {
                if (earning.CalendarYear > currentPeriod.Year
                    || (earning.CalendarYear == currentPeriod.Year && earning.CalendarMonth > currentPeriod.Month))
                {
                    continue;
                }

                var amountEarned = earning.EarnedValue;
                var alreadyPaid = paymentHistory
                    .Where(p => p.Ukprn == earning.Ukprn &&
                                p.Uln == earning.Uln &&
                                p.StandardCode == earning.StandardCode &&
                                p.FrameworkCode == earning.FrameworkCode &&
                                p.PathwayCode == earning.PathwayCode &&
                                p.ProgrammeType == earning.ProgrammeType &&
                                p.DeliveryMonth == earning.CalendarMonth &&
                                p.DeliveryYear == earning.CalendarYear &&
                                p.TransactionType == earning.Type)
                    .Sum(p => p.AmountDue);
                var amountDue = amountEarned - alreadyPaid;

                if (amountDue != 0)
                {
                    AddPaymentsDue(provider, paymentsDue, earning, amountDue);
                }
            }
        }

        private void AddPaymentsDue(Provider provider, List<RequiredPayment> paymentsDue, 
                                    Application.Earnings.PeriodEarning earning, decimal amountDue)
        {
            paymentsDue.Add(new RequiredPayment
            {
                CommitmentId = earning.CommitmentId,
                CommitmentVersionId = earning.CommitmentVersionId,
                AccountId = earning.AccountId,
                AccountVersionId = earning.AccountVersionId,
                Uln = earning.Uln,
                IlrSubmissionDateTime = provider.IlrSubmissionDateTime, //Provider
                Ukprn = earning.Ukprn,
                LearnerRefNumber = earning.LearnerReferenceNumber,
                AimSequenceNumber = earning.AimSequenceNumber,
                DeliveryMonth = earning.CalendarMonth,
                DeliveryYear = earning.CalendarYear,
                AmountDue = amountDue,
                TransactionType = earning.Type,
                StandardCode = earning.StandardCode,
                FrameworkCode = earning.FrameworkCode,
                ProgrammeType = earning.ProgrammeType,
                PathwayCode = earning.PathwayCode,
                ApprenticeshipContractType = earning.ApprenticeshipContractType,
                PriceEpisodeIdentifier = earning.PriceEpisodeIdentifier,
                SfaContributionPercentage = earning.SfaContributionPercentage,
                FundingLineType = earning.FundingLineType,
                UseLevyBalance = earning.UseLevyBalance
            });
        }
    }
}
