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
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Accounts.GetAccountAndPaymentInformationQuery;
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


                foreach (var refund in account.Refunds)
                {
                    _logger.Info($"Processing commitment {refund.CommitmentId} for account {account.Id} for refunds");

                        _logger.Info($"refund due of {refund.AmountDue} for commitment {refund.CommitmentId}, to credit for {refund.TransactionType} on {refund.LearnerRefNumber} / {refund.AimSequenceNumber} / {refund.Ukprn}");

                        var amountToRefund = MakeLevyRefund(period, refund);
                        if (amountToRefund < 0)
                        {
                            GetLevyAllocation(account, amountToRefund);
                            _logger.Info($"levy refund payment made of {refund.AmountDue} for commitment {refund.CommitmentId}, to credit for {refund.TransactionType} on {refund.LearnerRefNumber} / {refund.AimSequenceNumber} / {refund.Ukprn}");
                        }
                }

                foreach (var payment in account.Payments)
                {
                    _logger.Info($"Payment due of {payment.AmountDue} for commitment {payment.CommitmentId}, to pay for {payment.TransactionType} on {payment.LearnerRefNumber} / {payment.AimSequenceNumber} / {payment.Ukprn}");
                    var levyAllocation = GetLevyAllocation(account, payment.AmountDue);

                    if (levyAllocation == 0)
                    {
                        _logger.Info($"No mode levy in the account to pay for {payment.TransactionType} on {payment.LearnerRefNumber} / {payment.AimSequenceNumber} / {payment.Ukprn}");
                        break;
                    }

                    MakeLevyPayment(period, payment, levyAllocation);

                    _logger.Info($"Finished processing commitment {payment.CommitmentId} for account {account.Id}");
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
            return _mediator.Send(new GetAccountAndPaymentQueryRequest())?.Account;
        }

        private decimal GetLevyAllocation(Account account, decimal amountRequested)
        {
            return _mediator.Send(new AllocateLevyCommandRequest
            {
                Account = account,
                AmountRequested = amountRequested
            })?.AmountAllocated ?? 0;
        }

        private void MakeLevyPayment(CollectionPeriod period, PaymentDue paymentDue, decimal levyAllocation)
        {
            _logger.Info($"Making a levy payment of {levyAllocation} for commitment {paymentDue.CommitmentId}, to pay for {paymentDue.TransactionType} on {paymentDue.LearnerRefNumber} / {paymentDue.AimSequenceNumber} / {paymentDue.Ukprn}");

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


        private decimal MakeLevyRefund(CollectionPeriod period, PaymentDue paymentDue)
        {
            _logger.Info($"Making a levy refund payment of {paymentDue.AmountDue} for delivery month/year {paymentDue.DeliveryMonth} / {paymentDue.DeliveryYear}, to pay for {paymentDue.TransactionType} on {paymentDue.LearnerRefNumber} / {paymentDue.AimSequenceNumber} / {paymentDue.Ukprn}");
            decimal amountToRefund = 0;

            var historyPaymentsResponse = _mediator.Send(new GetLevyPaymentsHistoryQueryRequest
            {
                DeliveryYear = paymentDue.DeliveryYear,
                DeliveryMonth = paymentDue.DeliveryMonth,
                TransactionType = (int)paymentDue.TransactionType,
                CommitmentId = paymentDue.CommitmentId
            });

            if (!historyPaymentsResponse.IsValid)
            {
                throw new LevyPaymentsProcessorException(LevyPaymentsProcessorException.ErrorReadingPaymentsDueForCommitmentMessage, historyPaymentsResponse.Exception);
            }

            var historyPayments = historyPaymentsResponse.Items;
            var totalAmountPaidInPeriod = historyPayments.Sum(x => x.Amount);
            if (totalAmountPaidInPeriod == 0)
            {
                return 0m;
            }

            var totalLevyPaidInPeriod = historyPayments.Where(x => x.FundingSource == FundingSource.Levy).Sum(x => x.Amount);
            var percentagePaidByLevyInPeriod = totalLevyPaidInPeriod / totalAmountPaidInPeriod;

            amountToRefund = paymentDue.AmountDue * percentagePaidByLevyInPeriod; // historyPayments.Items.Sum(x => x.Amount) * -1;
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
