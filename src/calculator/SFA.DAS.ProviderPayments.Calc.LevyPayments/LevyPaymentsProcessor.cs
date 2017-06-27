using System.Linq;
using MediatR;
using NLog;

using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Accounts;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Accounts.AllocateLevyCommand;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Accounts.GetNextAccountQuery;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Accounts.MarkAccountAsProcessedCommand;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.CollectionPeriods;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.CollectionPeriods.GetCurrentCollectionPeriodQuery;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Payments;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Payments.GetPaymentsDueForCommitmentQuery;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Payments.ProcessPaymentCommand;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Payments.GetLevyPaymentsHistoryQuery;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments
{
    public class LevyPaymentsProcessor
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly string _yearOfCollection;

        public LevyPaymentsProcessor(ILogger logger, IMediator mediator, string yearOfCollection)
        {
            _logger = logger;
            _mediator = mediator;
            _yearOfCollection = yearOfCollection;
        }
        protected LevyPaymentsProcessor()
        {
            // So we can mock
        }


        public virtual void Process()
        {
            _logger.Info("Started Levy Payments Processor.");

            var period = GetCurrentCollectionPeriod();

            Account account;

            while ((account = GetNextAccountRequiringProcessing()) != null)
            {
                _logger.Info($"Processing account {account.Id}");

                //process refunds first
                foreach (var commitment in account.Commitments)
                {
                    _logger.Info($"Processing commitment {commitment.Id} for account {account.Id} for refunds");

                    var paymentsDue = GetPaymentsDueForCommitment(commitment.Id, true);

                    if (paymentsDue == null || !paymentsDue.Any())
                    {
                        continue;
                    }

                    foreach (var paymentDue in paymentsDue)
                    {
                        _logger.Info($"refund due of {paymentDue.AmountDue} for commitment {commitment.Id}, to credit for {paymentDue.TransactionType} on {paymentDue.LearnerRefNumber} / {paymentDue.AimSequenceNumber} / {paymentDue.Ukprn}");

                        var amountToRefund = MakeLevyRefund(commitment, period, paymentDue);
                        if (amountToRefund < 0)
                        {
                            GetLevyAllocation(account, amountToRefund);
                            _logger.Info($"levy refund payment made of {paymentDue.AmountDue} for commitment {commitment.Id}, to credit for {paymentDue.TransactionType} on {paymentDue.LearnerRefNumber} / {paymentDue.AimSequenceNumber} / {paymentDue.Ukprn}");

                        }
                    }

                }

                //process payments now
                var accountHasFundsForLevy = true;
                foreach (var commitment in account.Commitments)
                {
                    _logger.Info($"Processing commitment {commitment.Id} for account {account.Id} for payments");

                    var paymentsDue = GetPaymentsDueForCommitment(commitment.Id, false);

                    if (paymentsDue == null || !paymentsDue.Any())
                    {
                        continue;
                    }
                    
                    foreach (var paymentDue in paymentsDue.Where(x => x.AmountDue > 0))
                    {
                        _logger.Info($"Payment due of {paymentDue.AmountDue} for commitment {commitment.Id}, to pay for {paymentDue.TransactionType} on {paymentDue.LearnerRefNumber} / {paymentDue.AimSequenceNumber} / {paymentDue.Ukprn}");
                        var levyAllocation = GetLevyAllocation(account, paymentDue.AmountDue);

                        if (levyAllocation == 0)
                        {
                            _logger.Info($"No mode levy in the account to pay for {paymentDue.TransactionType} on {paymentDue.LearnerRefNumber} / {paymentDue.AimSequenceNumber} / {paymentDue.Ukprn}");
                            accountHasFundsForLevy = false;
                            break;
                        }

                        MakeLevyPayment(commitment, period, paymentDue, levyAllocation);

                    }

                    _logger.Info($"Finished processing commitment {commitment.Id} for account {account.Id}");

                    if (!accountHasFundsForLevy)
                    {
                        break;
                    }
                }

                MarkAccountAsProcessed(account.Id);

                _logger.Info($"Finished processing account {account.Id}");
            }

        }

        private CollectionPeriod GetCurrentCollectionPeriod()
        {
            var collectionPeriod = _mediator.Send(new GetCurrentCollectionPeriodQueryRequest());

            if (!collectionPeriod.IsValid)
            {
                throw new LevyPaymentsProcessorException(LevyPaymentsProcessorException.ErrorReadingCollectionPeriodMessage, collectionPeriod.Exception);
            }

            if (collectionPeriod.Period == null)
            {
                throw new LevyPaymentsProcessorException(LevyPaymentsProcessorException.ErrorNoCollectionPeriodMessage);
            }

            return collectionPeriod.Period;
        }

        private void MarkAccountAsProcessed(string accountId)
        {
            _mediator.Send(new MarkAccountAsProcessedCommandRequest { AccountId = accountId });
        }

        private PaymentDue[] GetPaymentsDueForCommitment(long commitmentId, bool refundPayments)
        {
            var paymentsDue = _mediator.Send(new GetPaymentsDueForCommitmentQueryRequest
            {
                CommitmentId = commitmentId,
                RefundPayments = refundPayments
            });

            if (!paymentsDue.IsValid)
            {
                throw new LevyPaymentsProcessorException(LevyPaymentsProcessorException.ErrorReadingPaymentsDueForCommitmentMessage, paymentsDue.Exception);
            }

            return paymentsDue.Items;
        }

        private Account GetNextAccountRequiringProcessing()
        {
            return _mediator.Send(new GetNextAccountQueryRequest())?.Account;
        }

        private decimal GetLevyAllocation(Account account, decimal amountRequested)
        {
            return _mediator.Send(new AllocateLevyCommandRequest
            {
                Account = account,
                AmountRequested = amountRequested
            })?.AmountAllocated ?? 0;
        }

        private void MakeLevyPayment(Commitment commitment, CollectionPeriod period, PaymentDue paymentDue, decimal levyAllocation)
        {
            _logger.Info($"Making a levy payment of {levyAllocation} for commitment {commitment.Id}, to pay for {paymentDue.TransactionType} on {paymentDue.LearnerRefNumber} / {paymentDue.AimSequenceNumber} / {paymentDue.Ukprn}");

            _mediator.Send(new ProcessPaymentCommandRequest
            {
                Payment = new Payment
                {
                    RequiredPaymentId = paymentDue.Id,
                    DeliveryMonth = paymentDue.DeliveryMonth,
                    DeliveryYear = paymentDue.DeliveryYear,
                    CollectionPeriodName = $"{_yearOfCollection}-{period.Name}",
                    CollectionPeriodMonth = period.Month,
                    CollectionPeriodYear = period.Year,
                    FundingSource = FundingSource.Levy,
                    TransactionType = paymentDue.TransactionType,
                    Amount = levyAllocation
                }
            });
        }


        private decimal MakeLevyRefund(Commitment commitment, CollectionPeriod period, PaymentDue paymentDue)
        {
            _logger.Info($"Making a levy refund payment of {paymentDue.AmountDue} for delivery month/year {paymentDue.DeliveryMonth} / {paymentDue.DeliveryYear}, to pay for {paymentDue.TransactionType} on {paymentDue.LearnerRefNumber} / {paymentDue.AimSequenceNumber} / {paymentDue.Ukprn}");
            decimal amountToRefund = 0;

            var historyPayments = _mediator.Send(new GetLevyPaymentsHistoryQueryRequest
            {
                DeliveryYear = paymentDue.DeliveryYear,
                DeliveryMonth = paymentDue.DeliveryMonth,
                TransactionType = (int)paymentDue.TransactionType,
                CommitmentId = paymentDue.CommitmentId
            });

            if (!historyPayments.IsValid)
            {
                throw new LevyPaymentsProcessorException(LevyPaymentsProcessorException.ErrorReadingPaymentsDueForCommitmentMessage, historyPayments.Exception);
            }

            amountToRefund = paymentDue.AmountDue; // historyPayments.Items.Sum(x => x.Amount) * -1;
            if (amountToRefund < 0)
            {
                _mediator.Send(new ProcessPaymentCommandRequest
                {
                    Payment = new Payment
                    {
                        RequiredPaymentId = paymentDue.Id,
                        DeliveryMonth = paymentDue.DeliveryMonth,
                        DeliveryYear = paymentDue.DeliveryYear,
                        CollectionPeriodName = $"{_yearOfCollection}-{period.Name}",
                        CollectionPeriodMonth = period.Month,
                        CollectionPeriodYear = period.Year,
                        FundingSource = FundingSource.Levy,
                        TransactionType = paymentDue.TransactionType,
                        Amount = amountToRefund
                    }
                });
            }

            return amountToRefund;
        }

    }
}
