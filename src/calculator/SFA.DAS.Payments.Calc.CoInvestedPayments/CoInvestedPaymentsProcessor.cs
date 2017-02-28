using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using NLog;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.CollectionPeriods;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.CollectionPeriods.GetCurrentCollectionPeriodQuery;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.Payments;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.Payments.ProcessPaymentsCommand;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.PaymentsDue;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.PaymentsDue.GetPaymentsDueForUkprnQuery;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.Providers.GetProvidersQuery;
using SFA.DAS.Payments.DCFS.Domain;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments
{
    public class CoInvestedPaymentsProcessor
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly string _yearOfCollection;

        public CoInvestedPaymentsProcessor(ILogger logger, IMediator mediator, string yearOfCollection)
        {
            _logger = logger;
            _mediator = mediator;
            _yearOfCollection = yearOfCollection;
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

                    WriteCoInvestedPaymentsForProviderOrThrow(provider.Ukprn, learnerLevelPaymentsForProvider);
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

        private void WriteCoInvestedPaymentsForProviderOrThrow(long ukprn, IReadOnlyCollection<Payment> payments)
        {
            _logger.Info($"Writing {payments.Count} learner co-invested payment entries for provider with ukprn {ukprn}.");

            var writeCommandResult = _mediator.Send(
                new ProcessPaymentsCommandRequest
                {
                    Payments = payments.ToArray()
                });

            if (!writeCommandResult.IsValid)
            {
                throw new CoInvestedPaymentsProcessorException(CoInvestedPaymentsProcessorException.ErrorWritingPaymentsForUkprn, writeCommandResult.Exception);
            }
        }

        private void AddCoInvestedPaymentsForLearner(ICollection<Payment> payments, CollectionPeriod currentPeriod, PaymentDue paymentDue)
        {
            switch (paymentDue.TransactionType)
            {
                case TransactionType.First16To18EmployerIncentive:
                case TransactionType.First16To18ProviderIncentive:
                case TransactionType.Second16To18EmployerIncentive:
                case TransactionType.Second16To18ProviderIncentive:
                case TransactionType.Balancing16To18FrameworkUplift:
                case TransactionType.Completion16To18FrameworkUplift:
                case TransactionType.OnProgramme16To18FrameworkUplift:
                case TransactionType.FirstDisadvantagePayment:
                case TransactionType.SecondDisadvantagePayment:
                case TransactionType.OnProgrammeMathsAndEnglish:
                case TransactionType.BalancingMathsAndEnglish:
                    payments.Add(
                        new Payment
                        {
                            RequiredPaymentId = paymentDue.Id,
                            DeliveryMonth = paymentDue.DeliveryMonth,
                            DeliveryYear = paymentDue.DeliveryYear,
                            CollectionPeriodName = $"{_yearOfCollection}-{currentPeriod.Name}",
                            CollectionPeriodMonth = currentPeriod.Month,
                            CollectionPeriodYear = currentPeriod.Year,
                            FundingSource = FundingSource.FullyFundedSfa,
                            TransactionType = paymentDue.TransactionType,
                            Amount = paymentDue.AmountDue
                        });
                    break;
                default:
                    payments.Add(
                    new Payment
                    {
                        RequiredPaymentId = paymentDue.Id,
                        DeliveryMonth = paymentDue.DeliveryMonth,
                        DeliveryYear = paymentDue.DeliveryYear,
                        CollectionPeriodName = $"{_yearOfCollection}-{currentPeriod.Name}",
                        CollectionPeriodMonth = currentPeriod.Month,
                        CollectionPeriodYear = currentPeriod.Year,
                        FundingSource = FundingSource.CoInvestedSfa,
                        TransactionType = paymentDue.TransactionType,
                        Amount = DetermineCoInvestedAmount(FundingSource.CoInvestedSfa, paymentDue.SfaContributionPercentage, paymentDue.AmountDue)
                    });

                    payments.Add(
                        new Payment
                        {
                            RequiredPaymentId = paymentDue.Id,
                            DeliveryMonth = paymentDue.DeliveryMonth,
                            DeliveryYear = paymentDue.DeliveryYear,
                            CollectionPeriodName = $"{_yearOfCollection}-{currentPeriod.Name}",
                            CollectionPeriodMonth = currentPeriod.Month,
                            CollectionPeriodYear = currentPeriod.Year,
                            FundingSource = FundingSource.CoInvestedEmployer,
                            TransactionType = paymentDue.TransactionType,
                            Amount = DetermineCoInvestedAmount(FundingSource.CoInvestedEmployer, paymentDue.SfaContributionPercentage, paymentDue.AmountDue)
                        });
                    break;
            }
        }

        private static decimal DetermineCoInvestedAmount(FundingSource fundingSource, decimal sfaContributionPercentage, decimal amountToPay)
        {
            decimal result;

            switch (fundingSource)
            {
                case FundingSource.CoInvestedSfa:
                {
                    result =  amountToPay * sfaContributionPercentage;
                    break;
                }
                case FundingSource.CoInvestedEmployer:
                {
                    result =  amountToPay * (1 - sfaContributionPercentage);
                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException(nameof(fundingSource), fundingSource.ToString());
                }
            }

            return decimal.Round(result, 5);
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
    }
}