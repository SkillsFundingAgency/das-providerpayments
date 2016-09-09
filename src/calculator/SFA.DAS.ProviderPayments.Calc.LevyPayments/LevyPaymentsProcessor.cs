using MediatR;
using NLog;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Accounts;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Accounts.AllocateLevyCommand;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Accounts.GetNextAccountQuery;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Accounts.MarkAccountAsProcessedCommand;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Earnings;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Earnings.GetEarningForCommitmentQuery;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Payments;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Payments.ProcessPaymentCommand;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments
{
    public class LevyPaymentsProcessor
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public LevyPaymentsProcessor(ILogger logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        protected LevyPaymentsProcessor()
        {
            // So we can mock
        }


        public virtual void Process()
        {
            _logger.Info("Started Levy Payments Processor.");

            Account account;
            while ((account = GetNextAccountRequiringProcessing()) != null)
            {
                _logger.Info($"Processing account {account.Id}");

                foreach (var commitment in account.Commitments)
                {
                    _logger.Info($"Processing commitment {commitment.Id} for account {account.Id}");

                    var earning = GetEarningForCommitment(commitment.Id);
                    if (earning == null || earning.MonthlyInstallmentCapped <= 0)
                    {
                        continue;
                    }

                    var isComplete = earning.LearningActualEndDate.HasValue;
                    var isCompleteOnCensusDate = HasCompletedOnCensusDate(earning);

                    if (!isComplete || isCompleteOnCensusDate)
                    {
                        MakeLevyPayment(account, commitment, earning, earning.MonthlyInstallmentCapped, TransactionType.Learning);
                    }
                    if (isComplete)
                    {
                        MakeLevyPayment(account, commitment, earning, earning.CompletionPaymentCapped, TransactionType.Completion);
                    }

                    _logger.Info($"Finished processing commitment {commitment.Id} for account {account.Id}");
                }

                MarkAccountAsProcessed(account.Id);
                _logger.Info($"Finished processing account {account.Id}");
            }
        }


        private void MarkAccountAsProcessed(string accountId)
        {
            _mediator.Send(new MarkAccountAsProcessedCommandRequest { AccountId = accountId });
        }
        private bool HasCompletedOnCensusDate(PeriodEarning earning)
        {
            if (!earning.LearningActualEndDate.HasValue)
            {
                return false;
            }
            return earning.LearningActualEndDate.Value.Month != earning.LearningActualEndDate.Value.AddDays(1).Month;
        }
        private PeriodEarning GetEarningForCommitment(string commitmentId)
        {
            return _mediator.Send(new GetEarningForCommitmentQueryRequest { CommitmentId = commitmentId })?.Earning;
        }
        private Account GetNextAccountRequiringProcessing()
        {
            return _mediator.Send(new GetNextAccountQueryRequest())?.Account;
        }

        private void MakeLevyPayment(Account account, Commitment commitment, PeriodEarning earning, decimal amount, TransactionType transactionType)
        {
            _logger.Info($"Levy payment of {amount} for commitment {commitment.Id}, to pay for {transactionType} on {earning.LearnerRefNumber} / {earning.AimSequenceNumber} / {earning.Ukprn}");

            var levyAllocation = _mediator.Send(new AllocateLevyCommandRequest
            {
                Account = account,
                AmountRequested = amount
            })?.AmountAllocated ?? 0;

            _logger.Info($"Making a levy payment of {levyAllocation} for commitment {commitment.Id}, to pay for {transactionType} on {earning.LearnerRefNumber} / {earning.AimSequenceNumber} / {earning.Ukprn}");

            _mediator.Send(new ProcessPaymentCommandRequest
            {
                Payment = new Payment
                {
                    CommitmentId = commitment.Id,
                    LearnerRefNumber = earning.LearnerRefNumber,
                    AimSequenceNumber = earning.AimSequenceNumber,
                    Ukprn = earning.Ukprn,
                    Source = FundingSource.Levy,
                    TransactionType = transactionType,
                    Amount = levyAllocation
                }
            });
        }

    }
}
