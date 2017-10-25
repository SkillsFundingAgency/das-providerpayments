using SFA.DAS.Payments.Reference.Accounts.Infrastructure.Data.Entities;

namespace SFA.DAS.Payments.Reference.Accounts.Infrastructure.Data
{
    public interface IAccountRepository
    {
        AccountEntity GetById(long id);

        void Insert(AccountEntity account);
        void Update(AccountEntity account);
    }
}