using System.Threading.Tasks;

namespace ProviderPayments.TestStack.Domain.Data
{
    public interface IWritableRepository<TEntity, TId> : IRepository
    {
        Task Create(TEntity entity);
        Task Update(TEntity entity);
        Task Delete(TId id);
    }
}
