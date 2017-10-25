using ProviderPayments.TestStack.Domain.Data.Entities;

namespace ProviderPayments.TestStack.Domain.Data
{
    public interface IProviderRepository : IRepository<ProviderEntity, long>
    {
    }
}
