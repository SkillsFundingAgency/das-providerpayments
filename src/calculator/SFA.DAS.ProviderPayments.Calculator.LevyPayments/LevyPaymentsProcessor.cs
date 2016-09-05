using MediatR;
using NLog;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Accounts;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Accounts.AllocateLevyCommand;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Accounts.GetNextAccountQuery;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Earnings;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Earnings.GetEarningForCommitmentQuery;
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
                    var earning = _mediator.Send(new GetEarningForCommitmentQueryRequest { CommitmentId = commitment.Id });

                    if (earning.Earning.MonthlyInstallmentCapped > 0)
                    {
                        MakeLevyPayment(account, commitment, earning.Earning);
                    }
                }
            }
        }

        private void MakeLevyPayment(Account account, Commitment commitment, PeriodEarning earning)
        {
            var levyAllocation = _mediator.Send(new AllocateLevyCommandRequest
            {
                Account = account,
                AmountRequested = earning.MonthlyInstallmentCapped
            })?.AmountAllocated ?? 0;

            _mediator.Send(new ProcessPaymentCommandRequest
            {
                Payment = new Application.Payments.Payment
                {
                    CommitmentId = commitment.Id,
                    LearnerRefNumber = earning.LearnerRefNumber,
                    AimSequenceNumber = earning.AimSequenceNumber,
                    Ukprn = earning.Ukprn,
                    Source = Application.Payments.FundingSource.Levy,
                    Amount = levyAllocation
                }
            });
        }

    }
}
