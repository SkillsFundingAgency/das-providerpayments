using System;
using System.Collections.Generic;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Domain
{
    public class LearnerCommitments
    {
        public List<CommitmentEntity> Commitments { get; set; }

        public LearnerCommitments(long key, IEnumerable<CommitmentEntity> commitments)
        {
            Commitments = new List<CommitmentEntity>(commitments);    
        }

        public CommitmentEntity CommitmentForDate(DateTime date)
        {
            return new CommitmentEntity();
        }
    }
}
