using System.Threading.Tasks;
using ProviderPayments.TestStack.Domain.Data.Entities;

namespace ProviderPayments.TestStack.Domain.Data
{
    public interface ICommitmentRepository : IRepository<CommitmentEntity, CommitmentEntityId>, IWritableRepository<CommitmentEntity, CommitmentEntityId>
    {
        Task UpdateEventStreamPointer();
    }
}
