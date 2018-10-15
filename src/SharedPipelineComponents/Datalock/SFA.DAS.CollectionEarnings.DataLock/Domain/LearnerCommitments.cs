using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.CollectionEarnings.DataLock.Domain.Extensions;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;
using SFA.DAS.Payments.DCFS.Domain;
using SFA.DAS.Payments.DCFS.Extensions;

namespace SFA.DAS.CollectionEarnings.DataLock.Domain
{
    public class LearnerCommitments
    {
        public List<Commitment> Commitments { get; }
        
        public LearnerCommitments(IEnumerable<CommitmentEntity> commitments)
        {
            Commitments = new List<Commitment>(commitments.Select(x => new Commitment(x)));
            var commitmentsById = Commitments.ToLookup(x => x.CommitmentId).OrderBy(x => x.Key);

            foreach (var commitmentsForId in commitmentsById)
            {
                var commitmentList = commitmentsForId.ToList();

                if (commitmentList.Count == 1)
                {
                    // No 'versions'
                    commitmentList[0].EffectiveStartDate = commitmentList[0].StartDate.LastDayOfMonth();
                    if (commitmentList[0].PaymentStatus == (int) PaymentStatus.Cancelled)
                    {
                        commitmentList[0].EffectiveEndDate = commitmentList[0].WithdrawnOnDate;
                    }
                    else
                    {
                        commitmentList[0].EffectiveEndDate = commitmentList[0].EndDate.LastDayOfMonth().AddDays(-1);
                    }
                }
                else
                {
                    foreach (var commitment in commitmentList)
                    {
                        commitment.EffectiveStartDate = commitment.EffectiveFrom;

                        if (commitment.PaymentStatus == (int) PaymentStatus.Cancelled &&
                            commitment.WithdrawnOnDate < commitment.EffectiveTo)
                        {
                            commitment.EffectiveEndDate = commitment.WithdrawnOnDate;
                        }
                        else
                        {
                            commitment.EffectiveEndDate = commitment.EffectiveTo;
                        }
                        
                        commitment.IsVersioned = true;
                    }

                    var commitmentsToRemove = new List<Commitment>();
                    foreach (var commitment in commitmentList)
                    {
                        if (commitment.EffectiveEndDate < commitment.EffectiveStartDate)
                        {
                            commitmentsToRemove.Add(commitment);
                        }
                    }

                    foreach (var commitment in commitmentsToRemove)
                    {
                        Commitments.Remove(commitment);
                    }
                }
            }

            for (var i = 0; i < Commitments.Count - 1; i++) // not looking at the last one
            {
                if (Commitments[i].EffectiveEndDate < Commitments[i + 1].EffectiveStartDate)
                {
                    Commitments[i].EffectiveEndDate = Commitments[i + 1].EffectiveStartDate.AddDays(-1);
                }
            }

            var lastCommitment = Commitments
                .OrderByDescending(x => x.EffectiveStartDate)
                .ThenByDescending(x => x.CommitmentId)
                .FirstOrDefault();
            if (lastCommitment != null && lastCommitment.PaymentStatus == (int) PaymentStatus.Active)
            {
                lastCommitment.EffectiveEndDate = new DateTime(2099, 01, 01);
            }
        }

        public IReadOnlyList<Commitment> ActiveCommitmentsForDate(DateTime date)
        {
            return Commitments.Where(x => x.EffectiveStartDate <= date &&

                                          (x.EffectiveEndDate == null ||
                                          x.EffectiveEndDate >= date) &&

                                          (x.PaymentStatus == (int)PaymentStatus.Active || 
                                              (x.PaymentStatus == (int)PaymentStatus.Cancelled &&
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
