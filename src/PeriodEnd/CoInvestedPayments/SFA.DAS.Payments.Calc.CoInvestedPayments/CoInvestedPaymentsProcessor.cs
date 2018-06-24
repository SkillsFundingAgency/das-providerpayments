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
using SFA.DAS.ProviderPayments.Calc.CoInvestedPayments.Application.Payments.GetCoInvestedPaymentsHistoryQuery;

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
                        if (paymentDue.AmountDue > 0)
                        {
                            AddCoInvestedPaymentsForLearner(learnerLevelPaymentsForProvider, collectionPeriod, paymentDue);
                        }
                        else if (paymentDue.AmountDue < 0)
                        {
                            MakeCoInvestedRefundsForLearner(learnerLevelPaymentsForProvider, collectionPeriod, paymentDue);

                        }
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
                case TransactionType.LearningSupport:
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

                    var coInvestedSfaAmount = DetermineCoInvestedAmount(FundingSource.CoInvestedSfa, paymentDue.SfaContributionPercentage, paymentDue.AmountDue);
                    if (coInvestedSfaAmount != 0)
                    {
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
                            Amount = coInvestedSfaAmount
                        });
                    }

                    var coInvestedEmployerAmount = DetermineCoInvestedAmount(FundingSource.CoInvestedEmployer, paymentDue.SfaContributionPercentage, paymentDue.AmountDue);
                    if (coInvestedEmployerAmount != 0)
                    {
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
                                Amount = coInvestedEmployerAmount
                            });
                    }
                    break;
            }
        }

        private void MakeCoInvestedRefundsForLearner(ICollection<Payment> payments, CollectionPeriod currentPeriod, PaymentDue paymentDue)
        {
            _logger.Info($"Making a co invested refund payment of {paymentDue.AmountDue} for delivery month/year {paymentDue.DeliveryMonth} / {paymentDue.DeliveryYear}, to pay for {paymentDue.TransactionType}  / {paymentDue.Ukprn}");

            var year = paymentDue.DeliveryYear;
            var month = paymentDue.DeliveryMonth;
            var refunded = 0m;

            while (paymentDue.AmountDue < refunded)
            {
                refunded += MakeCoinvestedRefund(payments, currentPeriod, paymentDue, year, month, paymentDue.AmountDue - refunded);

                month--;
                if (month == 0)
                {
                    year--;
                    month = 12;
                }

                if (month == 7)
                {
                    // Previous academic year
                    break;
                }
            }
        }

        private decimal MakeCoinvestedRefund(ICollection<Payment> payments, CollectionPeriod period, PaymentDue paymentDue, int year, int month,
            decimal amount)
        {
            var refunded = 0m;

            var historyPayments = _mediator.Send(new GetCoInvestedPaymentsHistoryQueryRequest
            {
                DeliveryYear = year,
                DeliveryMonth = month,
                TransactionType = (int)paymentDue.TransactionType,
                AimSequenceNumber = paymentDue.AimSequenceNumber,
                Ukprn = paymentDue.Ukprn,
                FrameworkCode = paymentDue.FrameworkCode,
                PathwayCode = paymentDue.PathwayCode,
                ProgrammeType = paymentDue.ProgrammeType,
                StandardCode = paymentDue.StandardCode,
                Uln = paymentDue.Uln
            });

            if (!historyPayments.IsValid)
            {
                throw new CoInvestedPaymentsProcessorException(CoInvestedPaymentsProcessorException.ErrorReadingPaymentsHistoryMessage, historyPayments.Exception);
            }

            var historicalPayments = historyPayments.Items.ToList();
            var coinvestedFundingSources = new[]
                {FundingSource.FullyFundedSfa, FundingSource.CoInvestedSfa, FundingSource.CoInvestedEmployer};

            var totalPaidInPeriod = historicalPayments.Where(x => coinvestedFundingSources.Contains(x.FundingSource)).Sum(x => x.Amount);
            amount = Math.Min(amount, totalPaidInPeriod);

            refunded += AddRefundPayment(payments, historicalPayments, FundingSource.FullyFundedSfa, paymentDue, period, amount);
            refunded += AddRefundPayment(payments, historicalPayments, FundingSource.CoInvestedSfa, paymentDue, period, amount);
            refunded += AddRefundPayment(payments, historicalPayments, FundingSource.CoInvestedEmployer, paymentDue, period, amount);

            return refunded;
        }

        private decimal AddRefundPayment(ICollection<Payment> payments,
            List<PaymentHistory> historyPayments,
            FundingSource fundingSource,
            PaymentDue paymentDue,
            CollectionPeriod currentPeriod,
            decimal refund)
        {
            var amountPaidTotal = historyPayments.Sum(x => x.Amount);
            if (amountPaidTotal < 0.005m)
            {
                return 0m;
            }

            var amountPaidFromSource = historyPayments.Where(x => x.FundingSource == fundingSource).Sum(x => x.Amount);

            var percentagePaidFromSource = amountPaidFromSource / amountPaidTotal;
            var amountToRefund = refund * percentagePaidFromSource;
            if (amountToRefund < 0)
            {
                payments.Add(
                    new Payment
                    {
                        RequiredPaymentId = paymentDue.Id,
                        DeliveryMonth = paymentDue.DeliveryMonth,
                        DeliveryYear = paymentDue.DeliveryYear,
                        CollectionPeriodName = $"{_yearOfCollection}-{currentPeriod.Name}",
                        CollectionPeriodMonth = currentPeriod.Month,
                        CollectionPeriodYear = currentPeriod.Year,
                        FundingSource = fundingSource,
                        TransactionType = paymentDue.TransactionType,
                        Amount = amountToRefund
                    });
            }

            return amountToRefund;
        }

        private static decimal DetermineCoInvestedAmount(FundingSource fundingSource, decimal sfaContributionPercentage, decimal amountToPay)
        {
            decimal result;

            switch (fundingSource)
            {
                case FundingSource.CoInvestedSfa:
                    {
                        result = amountToPay * sfaContributionPercentage;
                        break;
                    }
                case FundingSource.CoInvestedEmployer:
                    {
                        result = amountToPay * (1 - sfaContributionPercentage);
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