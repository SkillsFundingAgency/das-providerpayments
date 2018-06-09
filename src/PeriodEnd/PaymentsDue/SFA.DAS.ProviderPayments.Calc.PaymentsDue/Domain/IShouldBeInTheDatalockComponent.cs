using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Services;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain
{
    // ReSharper disable once InconsistentNaming
    public class IShouldBeInTheDatalockComponent : IIShouldBeInTheDataLockComponent
    {
        public List<PriceEpisode> ValidatePriceEpisodes(
            List<Commitment> commitments,
            List<DatalockOutput> dataLocks)
        {
            // ASSUMPTIONS from Looking at the live data.
            //  Datalocks are 'keyed' by UKPRN, LearnRefNumber, PriceEpisodeIdentifier and CommitmentId
            //  Where there are multiple commitmentids, one is payable and the rest are not
            //  Per unique key above, payable are either all true or all false

            var priceEpisodes = new List<PriceEpisode>();

            // Process dataLocks by price episode
            var dataLocksByPriceEpisode = dataLocks.ToLookup(x => x.PriceEpisodeIdentifier);

            foreach (var dataLockGroup in dataLocksByPriceEpisode)
            {
                // Break it down by commitment
                var priceEpisodesByCommitment = dataLockGroup.ToLookup(x => x.CommitmentId);
                foreach (var priceEpisodeGroup in priceEpisodesByCommitment)
                {
                    // Not sure about this one...
                    var commitment = commitments.FirstOrDefault(x => x.CommitmentId == priceEpisodeGroup.Key);
                    if (commitment == null || 
                        !commitment.IsLevyPayer || 
                        priceEpisodeGroup.All(x => !x.Payable))
                    {
                        var priceEpisode = new PriceEpisode(dataLockGroup.Key, false,
                            commitment?.CommitmentId ?? 0, commitment?.CommitmentVersionId,
                            commitment?.AccountId ?? 0, commitment?.AccountVersionId);
                        priceEpisodes.Add(priceEpisode);
                    }
                    else
                    {
                        var priceEpisode = new PriceEpisode(dataLockGroup.Key, true,
                            commitment.CommitmentId ?? 0, commitment.CommitmentVersionId,
                            commitment.AccountId ?? 0, commitment.AccountVersionId);
                        priceEpisodes.Add(priceEpisode);
                    }
                }
            }

            return priceEpisodes;
        }
    }
}
