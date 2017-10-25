using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProviderPayments.TestStack.Domain;
using ProviderPayments.TestStack.Domain.Data;
using ProviderPayments.TestStack.Domain.Data.Entities;
using ProviderPayments.TestStack.Domain.Mapping;

namespace ProviderPayments.TestStack.Application
{
    public interface IAccountService
    {
        Task<IEnumerable<Account>> GetAllAccounts();
        Task<Account> GetAccountById(long id);
        Task CreateAccount(Account account);
        Task UpdateAccount(Account account);
        Task DeleteAccount(long id);
    }

    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public AccountService(IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Account>> GetAllAccounts()
        {
            var entities = await _accountRepository.All();
            var accounts = _mapper.Map<IEnumerable<Account>>(entities);
            return accounts.OrderBy(a => a.Name);
        }

        public async Task<Account> GetAccountById(long id)
        {
            var entity = await _accountRepository.Single(id);
            if (entity == null)
            {
                return null;
            }
            var account = _mapper.Map<Account>(entity);

            return account;
        }

        public async Task CreateAccount(Account account)
        {
            ValidateAccount(account);

            var entity = _mapper.Map<AccountEntity>(account);
            entity.VersionId = DateTime.Now.ToString("yyyyMMddHHmmss");

            await _accountRepository.Create(entity);

            await _accountRepository.UpdateAudit();
        }

        public async Task UpdateAccount(Account account)
        {
            ValidateAccount(account);

            var entity = _mapper.Map<AccountEntity>(account);
            entity.VersionId = DateTime.Now.ToString("yyyyMMddHHmmss");

            await _accountRepository.Update(entity);

            await _accountRepository.UpdateAudit();
        }

        public async Task DeleteAccount(long id)
        {
            await _accountRepository.Delete(id);

            await _accountRepository.UpdateAudit();
        }

        private void ValidateAccount(Account account)
        {
            if (account.Id == 0)
            {
                throw new Exception("Id is required");
            }

            if (string.IsNullOrEmpty(account.Name))
            {
                throw new Exception("Name is required");
            }

            if (account.Balance < 0)
            {
                throw new Exception("Balance is required");
            }
        }
    }
}
