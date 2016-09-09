using MediatR;
using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Infrastructure.Data;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Accounts.AllocateLevyCommand
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
            var account = _accountRepository.GetAccountById(message.Account.Id);
            var amountToSpend = message.AmountRequested > account.Balance ? account.Balance : message.AmountRequested;
            if (amountToSpend < 0)
            {
                amountToSpend = 0;
            }

            _accountRepository.SpendLevy(message.Account.Id, amountToSpend);

            return new AllocateLevyCommandResponse
            {
                AmountAllocated = amountToSpend
            };
        }
    }
}