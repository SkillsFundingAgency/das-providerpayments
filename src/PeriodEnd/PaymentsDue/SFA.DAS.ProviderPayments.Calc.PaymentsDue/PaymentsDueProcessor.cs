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
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.Earnings;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application.RequiredPayments.GetPaymentHistoryWhereNoEarningQuery;
using System;
using System.Diagnostics;

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
                var paymentsDue = new List<RequiredPayment>();

                GetPaymentsDueForPaymentsWithoutEarnings(collectionPeriod.Period, paymentsDue);
                SavePaymentsDue(paymentsDue);

                foreach (var provider in providers.Items)
                {
                    ProcessProvider(provider, collectionPeriod.Period);
                    _logger.Info($"Finished processing provider with ukprn {provider.Ukprn}.");
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

            var earningResponse = _mediator.Send(new GetProviderEarningsQueryRequest
            {
                Ukprn = provider.Ukprn,
                AcademicYear = _context.GetPropertyValue(Common.Context.PaymentsContextPropertyKeys.YearOfCollection)
            });

            if (!earningResponse.IsValid)
            {
                throw new PaymentsDueProcessorException(PaymentsDueProcessorException.ErrorReadingProviderEarningsMessage, earningResponse.Exception);
            }

            var paymentsDue = new List<RequiredPayment>();
            GetPaymentsDue(provider, currentPeriod, earningResponse, paymentsDue);

            SavePaymentsDue(paymentsDue);

        }

        private void SavePaymentsDue(List<RequiredPayment> paymentsDue)
        {
            if (paymentsDue.Any())
            {
                var addPaymentsDueResponse = _mediator.Send(new AddRequiredPaymentsCommandRequest { Payments = paymentsDue.ToArray() });
                if (!addPaymentsDueResponse.IsValid)
                {
                    throw new PaymentsDueProcessorException(PaymentsDueProcessorException.ErrorWritingRequiredProviderPaymentsMessage, addPaymentsDueResponse.Exception);
                }
            }
        }

        private void GetPaymentsDueForPaymentsWithoutEarnings(CollectionPeriod currentPeriod, List<RequiredPayment> paymentsDue)
        {
            var historicalPaymentsResponse = _mediator.Send(new GetPaymentHistoryWhereNoEarningQueryRequest());
            if (!historicalPaymentsResponse.IsValid)
            {
                throw new PaymentsDueProcessorException(PaymentsDueProcessorException.ErrorReadingPaymentHistoryWithoutEarningsMessage, historicalPaymentsResponse.Exception);
            }

            foreach (var historicalPayment in historicalPaymentsResponse.Items)
            {
                paymentsDue.Add(new RequiredPayment
                {
                    CommitmentId = historicalPayment.CommitmentId,
                    CommitmentVersionId = historicalPayment.CommitmentVersionId,
                    AccountId = historicalPayment.AccountId,
                    AccountVersionId = historicalPayment.AccountVersionId,
                    Uln = historicalPayment.Uln,
                    IlrSubmissionDateTime = historicalPayment.IlrSubmissionDateTime,
                    Ukprn = historicalPayment.Ukprn,
                    LearnerRefNumber = historicalPayment.LearnerRefNumber,
                    AimSequenceNumber = historicalPayment.AimSequenceNumber,
                    DeliveryMonth = historicalPayment.DeliveryMonth,
                    DeliveryYear = historicalPayment.DeliveryYear,
                    AmountDue = -historicalPayment.AmountDue,
                    TransactionType = historicalPayment.TransactionType,
                    StandardCode = historicalPayment.StandardCode,
                    FrameworkCode = historicalPayment.FrameworkCode,
                    ProgrammeType = historicalPayment.ProgrammeType,
                    PathwayCode = historicalPayment.PathwayCode,
                    ApprenticeshipContractType = historicalPayment.ApprenticeshipContractType,
                    PriceEpisodeIdentifier = historicalPayment.PriceEpisodeIdentifier,
                    SfaContributionPercentage = historicalPayment.SfaContributionPercentage,
                    FundingLineType = historicalPayment.FundingLineType,
                    UseLevyBalance = historicalPayment.UseLevyBalance,
                    LearnAimRef = historicalPayment.LearnAimRef,
                    LearningStartDate = historicalPayment.LearningStartDate
                });
            }
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
                        e.LearnerReferenceNumber
                    })
                .Distinct()
                .ToArray();

            foreach (var earningItem in earningsData)
            {
                var historyResponse = _mediator.Send(new GetPaymentHistoryQueryRequest
                {
                    Ukprn = provider.Ukprn,
                    LearnRefNumber = earningItem.LearnerReferenceNumber
                });
                if (!historyResponse.IsValid)
                {
                    throw new PaymentsDueProcessorException(PaymentsDueProcessorException.ErrorReadingPaymentHistoryMessage, historyResponse.Exception);
                }
                paymentHistory.AddRange(historyResponse.Items);
            }

            foreach (var earning in earningResponse.Items)
            {
                var amountEarned = earning.EarnedValue;

                // If this is a 0 earning but there is another equivilant earning with earning then ignore this one
                if (amountEarned == 0 && PayableItemExists(earningResponse.Items, earning))
                {
                    continue;
                }

                if (earning.CalendarYear > currentPeriod.Year
                    || earning.CalendarYear == currentPeriod.Year && earning.CalendarMonth > currentPeriod.Month)
                {
                    continue;
                }

                var historicalAllPayments = paymentHistory
                    .Where(p => p.Ukprn == earning.Ukprn &&
                                p.LearnerRefNumber == earning.LearnerReferenceNumber &&
                                p.StandardCode == earning.StandardCode &&
                                p.FrameworkCode == earning.FrameworkCode &&
                                p.PathwayCode == earning.PathwayCode &&
                                p.ProgrammeType == earning.ProgrammeType &&
                                p.TransactionType == earning.Type &&
                                p.LearnAimRef == earning.LearnAimRef &&
                                p.LearningStartDate.Month == earning.LearningStartDate.Month &&
                                p.LearningStartDate.Year == earning.LearningStartDate.Year);

                ProcessContractTypeChanges(historicalAllPayments, earning, provider, paymentsDue);

                var alreadyPaidItems = historicalAllPayments.Where(p => p.DeliveryMonth == earning.CalendarMonth &&
                                                                    p.DeliveryYear == earning.CalendarYear).ToArray();

                var amountDue = amountEarned - alreadyPaidItems.Sum(p => p.AmountDue);


                var isPayble = false;
                if (EarningIsPayableDasEarning(earning) || EarningIsPayableNonDasEarning(earning))
                {
                    isPayble = true;
                }
                else if (amountEarned <= 0)
                {
                    isPayble = true;

                    var oldCommitment = alreadyPaidItems.FirstOrDefault();
                    if (oldCommitment != null)
                    {
                        earning.CommitmentId = oldCommitment.CommitmentId;
                        earning.AccountId = oldCommitment.AccountId;
                        earning.AccountVersionId = oldCommitment.AccountVersionId;
                        earning.CommitmentVersionId = oldCommitment.CommitmentVersionId;
                    }
                }

                if (amountDue != 0 && isPayble)
                {
                    if (amountDue >= 0)
                    {
                        AddPaymentsDue(provider, paymentsDue, earning, amountDue);
                    }
                    else
                    {
                        ApportionPaymentDuesWithHistoricalPayments(provider, paymentsDue, earning, historicalAllPayments.ToArray(), amountDue, currentPeriod);
                    }
                }
            }
        }

        private void ProcessContractTypeChanges(IEnumerable<RequiredPayment> historicalAllPayments, PeriodEarning earning, Provider provider, List<RequiredPayment> paymentsDue)
        {
            var contractTypeChangePayments = historicalAllPayments.Where(h => h.DeliveryMonth == earning.CalendarMonth &&
                                                                       h.DeliveryYear == earning.CalendarYear &&
                                                                       h.ApprenticeshipContractType != earning.ApprenticeshipContractType &&
                                                                       earning.ApprenticeshipContractTypeStartDate.HasValue &&
                                                                       new DateTime(h.DeliveryYear, h.DeliveryMonth, 1) <= new DateTime(earning.ApprenticeshipContractTypeStartDate.Value.Year, earning.ApprenticeshipContractTypeStartDate.Value.Month, 1));

            if (historicalAllPayments.Count() == 1 && historicalAllPayments.First().AmountDue > 0)
            {
                foreach (var payment in contractTypeChangePayments)
                {
                    AddPaymentsDue(paymentsDue, payment, -payment.AmountDue, earning, provider);
                    AddPaymentsDue(provider, paymentsDue, earning, earning.EarnedValue);
                }
            }
        }

        private bool EarningIsPayableDasEarning(PeriodEarning earning)
        {
            return earning.EarnedValue > 0 && earning.ApprenticeshipContractType == 1 && earning.Payable && earning.IsSuccess;
        }
        private bool EarningIsPayableNonDasEarning(PeriodEarning earning)
        {
            return earning.EarnedValue > 0 && earning.ApprenticeshipContractType == 2;
        }

        private bool PayableItemExists(PeriodEarning[] earnings, PeriodEarning currentEarning)
        {
            return earnings.Any(p => p.Ukprn == currentEarning.Ukprn &&
                               p.Uln == currentEarning.Uln &&
                               p.StandardCode == currentEarning.StandardCode &&
                               p.FrameworkCode == currentEarning.FrameworkCode &&
                               p.PathwayCode == currentEarning.PathwayCode &&
                               p.ProgrammeType == currentEarning.ProgrammeType &&
                               p.CalendarMonth == currentEarning.CalendarMonth &&
                               p.CalendarYear == currentEarning.CalendarYear &&
                               p.Type == currentEarning.Type &&
                               p.EarnedValue != 0 &&
                               ((p.ApprenticeshipContractType == 1 && p.IsSuccess && p.Payable) || p.ApprenticeshipContractType == 2));
        }

        private void ApportionPaymentDuesOverPreviousPeriods(Provider provider, List<RequiredPayment> paymentsDue, PeriodEarning earning, RequiredPayment[] paymentHistory, decimal amountDue)
        {
            var refundablePeriods = paymentHistory.Where(x => x.LearnerRefNumber == earning.LearnerReferenceNumber)
                                                   .GroupBy(x => new { x.DeliveryMonth, x.DeliveryYear })
                                                  .Select(x => new { x.Key.DeliveryMonth, x.Key.DeliveryYear, AmountDue = x.Sum(y => y.AmountDue) })
                                                  .OrderByDescending(x => x.DeliveryYear)
                                                  .ThenByDescending(x => x.DeliveryMonth)
                                                  .ToArray();

            var originalAmountDue = amountDue;
            // if there are no historical payments then just skip the execution
            if (refundablePeriods.Any())
            {
                var refundPeriodIndex = 0;
                while (amountDue < 0)
                {
                    if (refundPeriodIndex >= refundablePeriods.Length)
                    {
                        _logger.Error($"ERROR TRYING TO Refund.\n UKPRN: {provider.Ukprn}\n ILR Submission Date: {provider.IlrSubmissionDateTime}\n" +
                                      $"Learner reference number: {earning.LearnerReferenceNumber}\n ULN: {earning.Uln}\n" +
                                      $"Amount to refund: {originalAmountDue}\n Current amount to refund: {amountDue}\n");
                        break;
                    }

                    var period = refundablePeriods[refundPeriodIndex];

                    // Attempt to get refund from payments due first
                    var paymentsDueInPeriod = paymentsDue.Where(x => x.LearnerRefNumber == earning.LearnerReferenceNumber && x.DeliveryMonth == period.DeliveryMonth && x.DeliveryYear == period.DeliveryYear && x.TransactionType == earning.Type).ToArray();
                    foreach (var paymentDue in paymentsDueInPeriod)
                    {
                        amountDue -= -paymentDue.AmountDue;
                        paymentsDue.Remove(paymentDue);
                    }

                    // Attempt to get any remaining amount from previous payments
                    var refundedInPeriod = period.AmountDue >= -amountDue ? amountDue : -period.AmountDue;
                    if (refundedInPeriod != 0)
                    {
                        AddPaymentsDue(provider, paymentsDue, earning, refundedInPeriod, period.DeliveryMonth, period.DeliveryYear);
                        amountDue -= refundedInPeriod;
                    }

                    refundPeriodIndex++;
                }
            }
            else
            {
                AddPaymentsDue(provider, paymentsDue, earning, earning.EarnedValue, earning.CalendarMonth, earning.CalendarYear);
            }
        }

        private void ApportionPaymentDuesWithHistoricalPayments(Provider provider,
                                                                List<RequiredPayment> paymentsDue,
                                                                PeriodEarning earning,
                                                                RequiredPayment[] paymentHistory,
                                                                decimal amountDue,
                                                                CollectionPeriod currentPeriod)
        {

            var refundAdded = false;
            var pastTransaction = paymentHistory.Where(x => x.DeliveryMonth == earning.CalendarMonth && x.DeliveryYear == earning.CalendarYear);

            if (pastTransaction.Count() == 1)
            {
                var transaction = pastTransaction.First();
                if (transaction.AmountDue == -amountDue)
                {
                    AddRefundPaymentDue(provider, paymentsDue, new DateTime(transaction.CollectionPeriodYear, transaction.CollectionPeriodMonth, 1), transaction.AmountDue, transaction.CommitmentId, earning, paymentHistory, amountDue);
                    refundAdded = true;
                }

            }
            if (!refundAdded)
            {

                var refundablePeriods = paymentHistory.Where(x => x.DeliveryMonth == earning.CalendarMonth
                                                                && x.DeliveryYear == earning.CalendarYear
                                                                && x.AmountDue > 0
                                                                && new DateTime(x.CollectionPeriodYear, x.CollectionPeriodMonth, 1) !=
                                                                    new DateTime(currentPeriod.Year, currentPeriod.Month, 1))
                                                                .OrderByDescending(x => new DateTime(x.CollectionPeriodYear, x.CollectionPeriodMonth, 1))
                                                               .ToArray();

                // if there are no historical payments then just skip the execution
                if (refundablePeriods.Any())
                {

                    var refundPeriodIndex = 0;
                    while (amountDue < 0)
                    {

                        //We should not get this, but added here otherwise it could well stuck into infinite loop
                        if (refundablePeriods.Length == refundPeriodIndex)
                        {
                            break;
                        }
                        var period = refundablePeriods[refundPeriodIndex];

                        amountDue -= AddRefundPaymentDue(provider, paymentsDue,
                                                        new DateTime(period.CollectionPeriodYear, period.CollectionPeriodMonth, 1),
                                                        period.AmountDue,
                                                        period.CommitmentId,
                                                        earning,
                                                        paymentHistory,
                                                        amountDue);
                        refundAdded = true;
                        refundPeriodIndex++;
                    }
                }
            }
            if (!refundAdded)
            {
                ApportionPaymentDuesOverPreviousPeriods(provider, paymentsDue, earning, paymentHistory, amountDue);
            }
        }
        private decimal AddRefundPaymentDue(Provider provider,
                                        List<RequiredPayment> paymentsDue,
                                        DateTime paymentDate,
                                        decimal alreadyPaidAmount,
                                        long? commitmentId,
                                        PeriodEarning earning,
                                        RequiredPayment[] paymentHistory,
                                        decimal amountDue)
        {

            // Attempt to get refund from payments due first
            var paymentsDueInPeriod = paymentsDue.Where(x => x.LearnerRefNumber == earning.LearnerReferenceNumber && x.DeliveryMonth == paymentDate.Month
                                                        && x.DeliveryYear == paymentDate.Year
                                                        && alreadyPaidAmount > 0
                                                        && x.TransactionType == earning.Type).ToArray();
            foreach (var paymentDue in paymentsDueInPeriod)
            {
                alreadyPaidAmount -= -paymentDue.AmountDue;
                paymentsDue.Remove(paymentDue);
            }

            // Attempt to get any remaining amount from previous payments
            var refundedInPeriod = alreadyPaidAmount >= -amountDue ? amountDue : -alreadyPaidAmount;
            if (refundedInPeriod != 0)
            {
                UpdateCommitmentForRefund(paymentHistory, earning, commitmentId, paymentDate);

                AddPaymentsDue(provider, paymentsDue, earning, refundedInPeriod);

            }
            return refundedInPeriod;
        }

        private void UpdateCommitmentForRefund(RequiredPayment[] paymentHistory, PeriodEarning earning, long? historicalCommitmentId, DateTime historicalCollectionPeriodDate)
        {
            if (historicalCommitmentId.HasValue && earning.CommitmentId.HasValue)
            {
                var historicalPayment = paymentHistory.FirstOrDefault(x => x.CommitmentId == historicalCommitmentId
                                                    && x.DeliveryMonth == earning.CalendarMonth
                                                    && x.DeliveryYear == earning.CalendarYear
                                                    && x.CollectionPeriodMonth == historicalCollectionPeriodDate.Month
                                                    && x.CollectionPeriodYear == historicalCollectionPeriodDate.Year);
                if (historicalPayment != null)
                {
                    earning.CommitmentId = historicalPayment.CommitmentId;
                    earning.CommitmentVersionId = historicalPayment.CommitmentVersionId;
                    earning.AccountId = historicalPayment.AccountId;
                    earning.AccountVersionId = historicalPayment.AccountVersionId;
                }
            }
        }

        private void AddPaymentsDue(Provider provider, List<RequiredPayment> paymentsDue, PeriodEarning earning, decimal amountDue, int deliveryMonth = 0, int deliveryYear = 0)
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
                DeliveryMonth = deliveryMonth > 0 ? deliveryMonth : earning.CalendarMonth,
                DeliveryYear = deliveryYear > 0 ? deliveryYear : earning.CalendarYear,
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
                UseLevyBalance = earning.UseLevyBalance,
                LearnAimRef = earning.LearnAimRef,
                LearningStartDate = earning.LearningStartDate
            });
        }

        private void AddPaymentsDue(List<RequiredPayment> paymentsDue, RequiredPayment payment, decimal amountDue,
            PeriodEarning earning, Provider provider)
        {
            payment.AmountDue = amountDue;
            payment.IlrSubmissionDateTime = provider.IlrSubmissionDateTime;

            paymentsDue.Add(payment);
        }
    }
}
