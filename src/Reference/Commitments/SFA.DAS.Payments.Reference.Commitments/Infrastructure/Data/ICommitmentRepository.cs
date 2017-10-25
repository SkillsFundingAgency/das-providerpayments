using SFA.DAS.Payments.Reference.Commitments.Infrastructure.Data.Entities;

namespace SFA.DAS.Payments.Reference.Commitments.Infrastructure.Data
{
    public interface ICommitmentRepository
    {
        CommitmentEntity GetById(long commitmentId);
        bool CommitmentExists(CommitmentEntity commitment);

        void Insert(CommitmentEntity commitment);
        void InsertHistory(CommitmentEntity commitment);
        void Update(CommitmentEntity commitment);
        void Delete(long commitmentId);
    }
}