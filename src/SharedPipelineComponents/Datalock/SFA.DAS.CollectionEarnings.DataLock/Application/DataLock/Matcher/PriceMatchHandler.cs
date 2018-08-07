﻿using System.Collections.Generic;
using System.Linq;
using SFA.DAS.CollectionEarnings.DataLock.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.DataLock.Application.DataLock.Matcher
{
    public class PriceMatchHandler : MatchHandler
    {
        public PriceMatchHandler(MatchHandler nextMatchHandler):
            base(nextMatchHandler)
        {

        }
        public override bool StopOnError
        {
            get
            {
                return false;
            }
        }
        public override MatchResult Match(List<CommitmentEntity> commitments, PriceEpisode.PriceEpisode priceEpisode, List<DasAccount.DasAccount> dasAccounts, MatchResult matchResult)
        {
            matchResult.Commitments = commitments.ToArray();

            var commitmentsToMatch = commitments.Where(c => priceEpisode.NegotiatedPrice.HasValue &&
                                                            (long) c.AgreedCost == priceEpisode.NegotiatedPrice.Value).ToList();

            if (!commitmentsToMatch.Any())
            {
               matchResult.ErrorCodes.Add(DataLockErrorCodes.MismatchingPrice);
            }
        
            else
            {
                matchResult.Commitments = commitmentsToMatch.ToArray();
            }

            return ExecuteNextHandler(commitments, priceEpisode,dasAccounts,matchResult);
        }
    }
}