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
            long accountId;
            if (!long.TryParse(message.AccountId, out accountId))
            {
                throw new InvalidRequestException($"Invalid account id. {message.AccountId} is not a valid number");
            }

            _accountRepository.MarkAccountAsProcessed(accountId);
            return Unit.Value;
        }
    }
}