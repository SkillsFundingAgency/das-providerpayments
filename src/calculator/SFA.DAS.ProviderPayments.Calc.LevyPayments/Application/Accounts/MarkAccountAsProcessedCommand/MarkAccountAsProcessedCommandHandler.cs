using MediatR;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Accounts.MarkAccountAsProcessedCommand
{
    public class MarkAccountAsProcessedCommandHandler : IRequestHandler<MarkAccountAsProcessedCommandRequest, Unit>
    {
        private readonly IAccountRepository _accountRepository;

        public MarkAccountAsProcessedCommandHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public Unit Handle(MarkAccountAsProcessedCommandRequest message)
        {
            _accountRepository.MarkAccountAsProcessed(message.AccountId);
            return Unit.Value;
        }
    }
}