using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Domain
{
    public class ProviderCommitments
    {
        private Dictionary<long, LearnerCommitments> CommitmentsByLearner { get; } = new Dictionary<long, LearnerCommitments>();
        
        public ProviderCommitments(IEnumerable<CommitmentEntity> commitments)
        {
            if (commitments == null)
            {
                throw new ArgumentNullException(nameof(commitments));
            }

            var sortedCommitments = commitments.ToLookup(x => x.Uln);

            foreach (var sortedCommitment in sortedCommitments)
            {
                var learnerCommitments = new LearnerCommitments(sortedCommitment.ToList());
                CommitmentsByLearner.Add(sortedCommitment.Key, learnerCommitments);
            }
        }

        public LearnerCommitments CommitmentsForLearner(long uln)
        {
            if (!CommitmentsByLearner.ContainsKey(uln))
            {
                CommitmentsByLearner.Add(uln, new LearnerCommitments(new List<CommitmentEntity>()));
            }
            return CommitmentsByLearner[uln];
        }

        public IEnumerable<long> AllUlns()
        {
            return CommitmentsByLearner.Keys;
        }
    }
}
