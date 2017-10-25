using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProviderPayments.TestStack.Domain.Data
{
    public interface IRepository
    {
        
    }

    public interface IRepository<TEntity, TId> : IRepository
    {
        Task<IEnumerable<TEntity>> All();
        Task<TEntity> Single(TId id);
    }
}
