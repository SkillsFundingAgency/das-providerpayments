using System;
using System.Collections.Generic;
using MediatR;
using NLog;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.CollectionPeriods;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.CollectionPeriods.GetCurrentCollectionPeriodQuery;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.Earnings;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.Earnings.GetProviderEarningsQuery;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.Payments;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.Payments.ProcessPaymentCommand;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.PaymentsDue;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.PaymentsDue.GetPaymentsDueForUkprnQuery;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.Providers.GetProvidersQuery;
using SFA.DAS.ProviderPayments.Calc.Common.Application;
using SFA.DAS.ProviderPayments.Calc.Common.Tools.Extensions;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments
{
    public class CoInvestedPaymentsProcessor
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private const decimal CoInvestedSfaRatio = 0.9m;


        public CoInvestedPaymentsProcessor(ILogger logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        protected CoInvestedPaymentsProcessor()
        {
            // So we can mock
        }


        public virtual void Process()
        {
            _logger.Info("Started Co-Invested Payments Processor.");

            var collectionPeriod = ReturnCurrentCollectionPeriodOrThrow();
            var providersQueryResponse = ReturnValidGetProvidersQueryResponseOrThrow();

            if (providersQueryResponse.HasAnyItems())
            {
                foreach (var provider in providersQueryResponse.Items)
                {
                    _logger.Info($"Processing co-invested payments for provider with ukprn {provider.Ukprn}.");

                    var providerPaymentsDueQueryResponse = ReturnValidGetPaymentsDueForUkprnQueryResponseOrThrow(provider.Ukprn);

                    if (!providerPaymentsDueQueryResponse.DoesHavePaymentsDue())
                    {
                        _logger.Info($"No payments due for found for provider with ukprn {provider.Ukprn}.");
                        continue;
                    }

                    var learnerLevelPaymentsForProvider = new List<Payment>();

                    _logger.Info($"Provider {provider.Ukprn} has {providerPaymentsDueQueryResponse.Items.Length} payments due.");

                    _logger.Info($"Building payments from payments due for {provider.Ukprn}.");

                    foreach (var paymentDue in providerPaymentsDueQueryResponse.Items)
                    {
                        AddCoInvestedPaymentsForLearner(learnerLevelPaymentsForProvider, collectionPeriod, paymentDue);
                    }

                    WriteCoInvestedPaymentsForProvider(provider.Ukprn, learnerLevelPaymentsForProvider);
                }
            }
            else
            {
                _logger.Info("No providers found to process.");
            }

            _logger.Info("Finished Co-Invested Payments Processor.");
        }

        private GetPaymentsDueForUkprnQueryResponse ReturnValidGetPaymentsDueForUkprnQueryResponseOrThrow(long ukprn)
        {
            var response = _mediator.Send(new GetPaymentsDueForUkprnQueryRequest { Ukprn = ukprn });

            if (!response.IsValid)
            {
                throw new CoInvestedPaymentsProcessorException(
                    CoInvestedPaymentsProcessorException.ErrorReadingPaymentsDueForUkprn,
                    response.Exception);
            }
            return response;
        }

        private GetProvidersQueryResponse ReturnValidGetProvidersQueryResponseOrThrow()
        {
            var providersQueryResponse = _mediator.Send(new GetProvidersQueryRequest());

            if (!providersQueryResponse.IsValid)
            {
                throw new CoInvestedPaymentsProcessorException(
                    CoInvestedPaymentsProcessorException.ErrorReadingProviders,
                    providersQueryResponse.Exception);
            }
            return providersQueryResponse;
        }

        private void WriteCoInvestedPaymentsForProvider(long ukprn, IReadOnlyCollection<Payment> payments)
        {
            _logger.Info($"Writing {payments.Count} learner co-invested payment entries for provider with ukprn {ukprn}.");

            foreach (var payment in payments)
            {
                _mediator.Send(
                    new ProcessPaymentCommandRequest
                    {
                        Payment = payment
                    });
            }

        }

        private void AddCoInvestedPaymentsForLearner(ICollection<Payment> payments, CollectionPeriod currentPeriod, PaymentDue paymentDue)
        {
            payments.Add(
                new Payment
                {
                    CommitmentId = paymentDue.CommitmentId,
                    LearnerRefNumber = paymentDue.LearnerRefNumber,
                    AimSequenceNumber = paymentDue.AimSequenceNumber,
                    Ukprn = paymentDue.Ukprn,
                    DeliveryMonth = paymentDue.DeliveryMonth,
                    DeliveryYear = paymentDue.DeliveryYear,
                    CollectionPeriodMonth = currentPeriod.Month,
                    CollectionPeriodYear = currentPeriod.Year,
                    FundingSource = FundingSource.CoInvestedSfa,
                    TransactionType = paymentDue.TransactionType,
                    Amount = DetermineCoInvestedAmount(FundingSource.CoInvestedSfa, paymentDue.AmountDue)
                }
                );

            payments.Add(
                new Payment
                {
                    CommitmentId = paymentDue.CommitmentId,
                    LearnerRefNumber = paymentDue.LearnerRefNumber,
                    AimSequenceNumber = paymentDue.AimSequenceNumber,
                    Ukprn = paymentDue.Ukprn,
                    DeliveryMonth = paymentDue.DeliveryMonth,
                    DeliveryYear = paymentDue.DeliveryYear,
                    CollectionPeriodMonth = currentPeriod.Month,
                    CollectionPeriodYear = currentPeriod.Year,
                    FundingSource = FundingSource.CoInvestedEmployer,
                    TransactionType = paymentDue.TransactionType,
                    Amount = DetermineCoInvestedAmount(FundingSource.CoInvestedEmployer, paymentDue.AmountDue)
                }
                );
        }

        private static decimal DetermineCoInvestedAmount(FundingSource fundingSource, decimal amountToPay)
        {
            switch (fundingSource)
            {
                case FundingSource.CoInvestedSfa:
                {
                    return amountToPay * CoInvestedSfaRatio;
                }
                case FundingSource.CoInvestedEmployer:
                {
                    return amountToPay * (1 - CoInvestedSfaRatio);
                }
                default:
                {
                    throw new ArgumentOutOfRangeException(nameof(fundingSource), fundingSource.ToString());
                }
            }
        }

        private CollectionPeriod ReturnCurrentCollectionPeriodOrThrow()
        {
            var collectionPeriod = _mediator.Send(new GetCurrentCollectionPeriodQueryRequest());

            if (!collectionPeriod.IsValid)
            {
                throw new CoInvestedPaymentsProcessorException(CoInvestedPaymentsProcessorException.ErrorReadingCollectionPeriodMessage, collectionPeriod.Exception);
            }

            if (collectionPeriod.Period == null)
            {
                throw new CoInvestedPaymentsProcessorException(CoInvestedPaymentsProcessorException.ErrorNoCollectionPeriodMessage);
            }

            return collectionPeriod.Period;
        }

        private static bool HasCompletedOnCensusDate(Earning earning)
        {
            if (!earning.LearningActualEndDate.HasValue)
            {
                return false;
            }

            return earning.LearningActualEndDate.Value == earning.LearningActualEndDate.Value.LastDayOfMonth();
        }
    }
}