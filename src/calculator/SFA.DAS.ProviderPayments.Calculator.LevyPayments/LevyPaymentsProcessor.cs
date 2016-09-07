using MediatR;
using NLog;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Accounts;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Accounts.AllocateLevyCommand;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Accounts.GetNextAccountQuery;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Accounts.MarkAccountAsProcessedCommand;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Earnings;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Earnings.GetEarningForCommitmentQuery;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Payments;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Payments.ProcessPaymentCommand;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments
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
            while ((account = _mediator.Send(new GetNextAccountQueryRequest())?.Account) != null)
            {
                foreach (var commitment in account.Commitments)
                {
                    var earning = _mediator.Send(new GetEarningForCommitmentQueryRequest { CommitmentId = commitment.Id })?.Earning;
                    if (earning == null || earning.MonthlyInstallmentCapped <= 0)
                    {
                        continue;
                    }

                    var isComplete = earning.LearningActualEndDate.HasValue;
                    var isCompleteOnCensusDate = isComplete && earning.LearningActualEndDate.Value.Month != earning.LearningActualEndDate.Value.AddDays(1).Month;

                    if (!isComplete || isCompleteOnCensusDate)
                    {
                        MakeLevyPayment(account, commitment, earning, earning.MonthlyInstallmentCapped, TransactionType.Learning);
                    }
                    if (isComplete)
                    {
                        MakeLevyPayment(account, commitment, earning, earning.CompletionPaymentCapped, TransactionType.Completion);
                    }

                    //if (earning.LearningActualEndDate.HasValue)
                    //{
                    //    MakeLevyPayment(account, commitment, earning, earning.CompletionPaymentCapped, TransactionType.Completion);
                    //}
                    //else if (earning.MonthlyInstallmentCapped > 0)
                    //{
                    //    MakeLevyPayment(account, commitment, earning, earning.MonthlyInstallmentCapped, TransactionType.Learning);
                    //}
                }

                _mediator.Send(new MarkAccountAsProcessedCommandRequest {AccountId = account.Id});
            }
        }

        private void MakeLevyPayment(Account account, Commitment commitment, PeriodEarning earning, decimal amount, TransactionType transactionType)
        {
            var levyAllocation = _mediator.Send(new AllocateLevyCommandRequest
            {
                Account = account,
                AmountRequested = amount
            })?.AmountAllocated ?? 0;

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
