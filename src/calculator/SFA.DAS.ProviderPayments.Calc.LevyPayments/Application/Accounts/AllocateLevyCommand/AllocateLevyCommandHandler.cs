using MediatR;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Accounts.AllocateLevyCommand
{
    public class AllocateLevyCommandHandler : IRequestHandler<AllocateLevyCommandRequest, AllocateLevyCommandResponse>
    {
        private readonly IAccountRepository _accountRepository;

        public AllocateLevyCommandHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public AllocateLevyCommandResponse Handle(AllocateLevyCommandRequest message)
        {
            long accountId;
            if (!long.TryParse(message.Account.Id, out accountId))
            {
                throw new InvalidRequestException($"Invalid account id. {message.Account.Id} is not a valid number");
            }

            var account = _accountRepository.GetAccountById(accountId);
            var amountToSpend = message.AmountRequested > account.Balance ? account.Balance : message.AmountRequested;
            if (amountToSpend < 0)
            {
                amountToSpend = 0;
            }

            _accountRepository.SpendLevy(accountId, amountToSpend);

            return new AllocateLevyCommandResponse
            {
                AmountAllocated = amountToSpend
            };
        }
    }
}