using ProviderPayments.TestStack.Domain.Data.Entities;

namespace ProviderPayments.TestStack.Domain.Data
{
    public interface IProcessRepository : IRepository<ProcessStatusEntity, string>
    {
    }
}
