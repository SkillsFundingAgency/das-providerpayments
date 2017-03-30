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

            decimal amountToUpdate = 0;
            if (message.AmountRequested > 0)
            {
                var account = _accountRepository.GetAccountById(accountId);
                amountToUpdate = message.AmountRequested > account.Balance ? account.Balance : message.AmountRequested;
                if (amountToUpdate < 0)
                {
                    amountToUpdate = 0;
                }
            }
            else
            {
                amountToUpdate = message.AmountRequested;
            }
           
            _accountRepository.UpdateLevyBalance(accountId, amountToUpdate);

            return new AllocateLevyCommandResponse
            {
                AmountAllocated = amountToUpdate
            };
        }
    }
}