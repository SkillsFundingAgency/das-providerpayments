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

            foreach (var datalockByPriceEpisode in dataLocksByPriceEpisode)
            {
                // Break it down by commitment
                var priceEpisodesByCommitment = datalockByPriceEpisode.ToLookup(x => x.CommitmentId);
                foreach (var priceEpisodeByCommitment in priceEpisodesByCommitment)
                {
                    // Not sure about this one...
                    var commitment = commitments
                        .OrderByDescending(x => x.CommitmentVersionId)
                        .FirstOrDefault(x => x.CommitmentId == priceEpisodeByCommitment.Key);

                    if (commitment == null || 
                        !commitment.IsLevyPayer || 
                        priceEpisodeByCommitment.All(x => !x.Payable))
                    {
                        var priceEpisode = new PriceEpisode(datalockByPriceEpisode.Key, new  List<int>(), 
                            commitment?.CommitmentId ?? 0, commitment?.CommitmentVersionId,
                            commitment?.AccountId ?? 0, commitment?.AccountVersionId);
                        priceEpisodes.Add(priceEpisode);
                    }
                    else
                    {
                        var payablePeriods = priceEpisodeByCommitment
                            .Where(x => x.Payable)
                            .Select(x => x.Period)
                            .ToList();

                        var priceEpisode = new PriceEpisode(datalockByPriceEpisode.Key, payablePeriods,
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
