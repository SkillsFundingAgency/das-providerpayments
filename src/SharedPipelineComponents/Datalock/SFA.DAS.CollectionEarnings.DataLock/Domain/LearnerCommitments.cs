﻿using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Domain
{
    public class LearnerCommitments
    {
        public List<Commitment> Commitments { get; }
        
        public LearnerCommitments(long key, IEnumerable<CommitmentEntity> commitments)
        {
            Commitments = new List<Commitment>(commitments.Select(x => new Commitment(x)));
            var commitmentsById = Commitments.ToLookup(x => x.CommitmentId);

            foreach (var commitmentsForId in commitmentsById)
            {
                var commitmentList = commitmentsForId.ToList();

                if (commitmentList.Count == 1)
                {
                    // No 'versions'
                    commitmentList[0].EffectiveStartDate = commitmentList[0].StartDate;
                    commitmentList[0].EffectiveEndDate = commitmentList[0].EndDate;
                }
                else
                {
                    foreach (var commitment in commitmentList)
                    {
                        commitment.EffectiveStartDate = commitment.EffectiveFrom;
                        commitment.EffectiveEndDate = commitment.EffectiveTo;
                        commitment.IsVersioned = true;
                    }
                }

                foreach (var commitment in commitmentList)
                {
                    if (commitment.WithdrawnOnDate.HasValue)
                    {
                        commitment.EffectiveEndDate = commitment.WithdrawnOnDate;
                    }
                }
            }

            var lastCommitment = Commitments
                .OrderByDescending(x => x.EffectiveStartDate)
                .ThenByDescending(x => x.CommitmentId)
                .FirstOrDefault();
            if (lastCommitment != null)
            {
                lastCommitment.EffectiveEndDate = null;
            }
        }

        public IReadOnlyList<Commitment> ActiveCommitmentsForDate(DateTime date)
        {
            return Commitments.Where(x => x.EffectiveStartDate <= date &&

                                          (x.EffectiveEndDate == null ||
                                          x.EffectiveEndDate >= date) &&

                                          (x.PaymentStatus == 1 || 
                                              (x.PaymentStatus == 3 &&
                                               x.WithdrawnOnDate >= date)))
                .ToList();
        }

        public IReadOnlyList<CommitmentEntity> NonActiveCommitmentsForDate(DateTime date)
        {
            return Commitments.Where(x => x.EffectiveStartDate < date &&
                                          (x.WithdrawnOnDate.HasValue ||
                                          x.PausedOnDate.HasValue))
                .ToList();
        }
    }
}