using MediatR;
using SFA.DAS.Payments.Reference.Accounts.Infrastructure.Data;
using SFA.DAS.Payments.Reference.Accounts.Infrastructure.Data.Entities;

namespace SFA.DAS.Payments.Reference.Accounts.Application.AddOrUpdateAccountCommand
{
    public class AddOrUpdateAccountCommandHandler : IRequestHandler<AddOrUpdateAccountCommandRequest, Unit>
    {
        private readonly IAccountRepository _accountRepository;

        public AddOrUpdateAccountCommandHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public Unit Handle(AddOrUpdateAccountCommandRequest message)
        {
            var exists = _accountRepository.GetById(message.Account.AccountId) != null;
            var entity = new AccountEntity
            {
                AccountId = message.Account.AccountId,
                AccountHashId = message.Account.AccountHashId,
                AccountName = message.Account.AccountName,
                Balance = message.Account.Balance,
                VersionId = message.CorrelationDate.ToString("yyyyMMdd"),
                IsLevyPayer = message.Account.IsLevyPayer
            };

            if (exists)
            {
                _accountRepository.Update(entity);
            }
            else
            {
                _accountRepository.Insert(entity);
            }

            return Unit.Value;
        }
    }
}