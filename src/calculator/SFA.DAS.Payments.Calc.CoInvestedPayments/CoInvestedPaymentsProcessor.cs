using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using NLog;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.CollectionPeriods;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.CollectionPeriods.GetCurrentCollectionPeriodQuery;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.Earnings;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.Earnings.GetProviderEarningsQuery;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.Payments;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.Payments.ProcessPaymentCommand;
using SFA.DAS.Payments.Calc.CoInvestedPayments.Application.Providers.GetProvidersQuery;
using SFA.DAS.ProviderPayments.Calc.Common.Application;
using SFA.DAS.ProviderPayments.Calc.Common.Tools.Extensions;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments
{
    public class CoInvestedPaymentsProcessor
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

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

            //var period = GetCurrentCollectionPeriod();

            // For each DAS entry in the ILR that has passed Data Lock, for this employer then split it out in to SFA and E lines in the Paymemts table...

            // Foreach Provider...

            //
            var collectionPeriod = _mediator.Send(new GetCurrentCollectionPeriodQueryRequest());

            if (!collectionPeriod.IsValid)
            {
                throw new CoInvestedPaymentsProcessorException(CoInvestedPaymentsProcessorException.NotDefined, collectionPeriod.Exception);
            }

            if (collectionPeriod.Period == null)
            {
                throw new CoInvestedPaymentsProcessorException(CoInvestedPaymentsProcessorException.NotDefined);
            }

            var providers = _mediator.Send(new GetProvidersQueryRequest());

            if (!providers.IsValid)
            {
                throw new CoInvestedPaymentsProcessorException(CoInvestedPaymentsProcessorException.NotDefined, providers.Exception);
            }

            if (providers.Items != null && providers.Items.Any())
            {
                foreach (var provider in providers.Items)
                {
                    _logger.Info($"Processing co-invested payments for provider with ukprn {provider.Ukprn}.");

                    //var providerDuePayments = new List<RequiredPayment>();

                    var providerEarnings = _mediator.Send(new GetProviderEarningsQueryRequest { Ukprn = provider.Ukprn });

                    if (!providerEarnings.IsValid)
                    {
                        throw new CoInvestedPaymentsProcessorException(CoInvestedPaymentsProcessorException.NotDefined, providerEarnings.Exception);
                    }

                    if (!providerEarnings.DoesHaveEarnings())
                    {
                        _logger.Info($"No earnings found for provider with ukprn {provider.Ukprn}.");
                        continue;
                    }

                    var learnerLevelPaymentsForProvider = new List<Payment>();

                    foreach (var earning in providerEarnings.Items)
                    {
                        if (ShouldPayProviderUsingCoInvested(collectionPeriod.Period, earning))
                        {
                            AddCoInvestedPaymentsForLearner(learnerLevelPaymentsForProvider, collectionPeriod.Period, earning);  // Add to learnerLevelPaymentsForProvider
                        }
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

            _logger.Info($"Finished writing learner co-invested payment entries for provider with ukprn {ukprn}.");
        }

        private void AddCoInvestedPaymentsForLearner(ICollection<Payment> payments, CollectionPeriod period, Earning earning)
        {
            var isComplete = earning.LearningActualEndDate.HasValue;
            var isCompleteOnCensusDate = HasCompletedOnCensusDate(earning);

            //if (!isComplete || isCompleteOnCensusDate)
            //{
            //    providerDuePayments.Add(DuePayment(collectionPeriod.Period, earning, earning.MonthlyInstallment, TransactionType.Learning));
            //}
            //if (isComplete)
            //{
            //    providerDuePayments.Add(DuePayment(collectionPeriod.Period, earning, earning.CompletionPayment, TransactionType.Completion));
            //}

            _logger.Info($"WIP");
            payments.Add(
                new Payment
                {
                    CommitmentId = earning.CommitmentId,
                    LearnerRefNumber = earning.LearnerRefNumber,
                    AimSequenceNumber = earning.AimSequenceNumber,
                    Ukprn = earning.Ukprn,
                    //DeliveryMonth = paymentDue.DeliveryMonth,
                    //DeliveryYear = paymentDue.DeliveryYear,
                    CollectionPeriodMonth = period.Month,
                    CollectionPeriodYear = period.Year,
                    Source = FundingSource.CoInvestedEmployer,
                    TransactionType = TransactionType.Learning,
                    Amount = 10000000
                }
                );
        }

        private bool ShouldPayProviderUsingCoInvested(CollectionPeriod period, Earning earning)
        {
            return true;
        }

        private void MakeCoInvestedPayment()
        {
            

        }

        private CollectionPeriod GetCurrentCollectionPeriod()
        {
            var collectionPeriod = _mediator.Send(new GetCurrentCollectionPeriodQueryRequest());

            if (!collectionPeriod.IsValid)
            {
                //throw new CoInvestedPaymentsProcessorException(CoInvestedPaymentsProcessorException.ErrorReadingCollectionPeriodMessage, collectionPeriod.Exception);
            }

            if (collectionPeriod.Period == null)
            {
                //throw new CoInvestedPaymentsProcessorException(CoInvestedPaymentsProcessorException.ErrorNoCollectionPeriodMessage);
            }

            return collectionPeriod.Period;
        }

        //private void MarkAccountAsProcessed(string accountId)
        //{
        //    _mediator.Send(new MarkAccountAsProcessedCommandRequest { AccountId = accountId });
        //}

        //private PaymentDue[] GetPaymentsDueForCommitment(string commitmentId)
        //{
        //    var paymentsDue = _mediator.Send(new GetPaymentsDueForCommitmentQueryRequest { CommitmentId = commitmentId });

        //    if (!paymentsDue.IsValid)
        //    {
        //        throw new LevyPaymentsProcessorException(LevyPaymentsProcessorException.ErrorReadingPaymentsDueForCommitmentMessage, paymentsDue.Exception);
        //    }

        //    return paymentsDue.Items;
        //}

        //private Account GetNextAccountRequiringProcessing()
        //{
        //    return _mediator.Send(new GetNextAccountQueryRequest())?.Account;
        //}

        //private decimal GetLevyAllocation(Account account, decimal amountRequested)
        //{
        //    return _mediator.Send(new AllocateLevyCommandRequest
        //    {
        //        Account = account,
        //        AmountRequested = amountRequested
        //    })?.AmountAllocated ?? 0;
        //}

        //private void MakeLevyPayment(Commitment commitment, CollectionPeriod period, PaymentDue paymentDue, decimal levyAllocation)
        //{
        //    _logger.Info($"Making a levy payment of {levyAllocation} for commitment {commitment.Id}, to pay for {paymentDue.TransactionType} on {paymentDue.LearnerRefNumber} / {paymentDue.AimSequenceNumber} / {paymentDue.Ukprn}");

        //    _mediator.Send(new ProcessPaymentCommandRequest
        //    {
        //        Payment = new Payment
        //        {
        //            CommitmentId = commitment.Id,
        //            LearnerRefNumber = paymentDue.LearnerRefNumber,
        //            AimSequenceNumber = paymentDue.AimSequenceNumber,
        //            Ukprn = paymentDue.Ukprn,
        //            DeliveryMonth = paymentDue.DeliveryMonth,
        //            DeliveryYear = paymentDue.DeliveryYear,
        //            CollectionPeriodMonth = period.Month,
        //            CollectionPeriodYear = period.Year,
        //            Source = FundingSource.Levy,
        //            TransactionType = paymentDue.TransactionType,
        //            Amount = levyAllocation
        //        }
        //    });
        //}
        private bool HasCompletedOnCensusDate(Earning earning)
        {
            if (!earning.LearningActualEndDate.HasValue)
            {
                return false;
            }

            return earning.LearningActualEndDate.Value == earning.LearningActualEndDate.Value.LastDayOfMonth();
        }
    }

    public class CoInvestedPaymentsProcessorException : Exception
    {
        public static string NotDefined = "NotDefined";

        public CoInvestedPaymentsProcessorException(string message)
            : base(message)
        {
        }

        public CoInvestedPaymentsProcessorException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}