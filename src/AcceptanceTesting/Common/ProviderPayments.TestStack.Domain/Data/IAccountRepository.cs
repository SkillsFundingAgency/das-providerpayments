using System.Threading.Tasks;
using ProviderPayments.TestStack.Domain.Data.Entities;

namespace ProviderPayments.TestStack.Domain.Data
{
    public interface IAccountRepository : IRepository<AccountEntity, long>, IWritableRepository<AccountEntity, long>
    {
        Task UpdateAudit();
    }
}
