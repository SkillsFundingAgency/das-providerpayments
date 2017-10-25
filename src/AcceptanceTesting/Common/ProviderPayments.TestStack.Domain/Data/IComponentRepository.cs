using System.Collections.Generic;
using System.Threading.Tasks;
using ProviderPayments.TestStack.Domain.Data.Entities;

namespace ProviderPayments.TestStack.Domain.Data
{
    public interface IComponentRepository : IRepository
    {
        Task<IEnumerable<ComponentEntity>> All();
        Task UpdateComponent(int componentType, string version, byte[] componentBuffer);
    }
}
