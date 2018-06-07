using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain
{
    // ReSharper disable once InconsistentNaming
    public class IShouldBeInTheDatalockComponent
    {
        public IShouldBeInTheDatalockComponent(
            List<Commitment> commitments,
            List<DataLockPriceEpisodePeriodMatchEntity> datalocks)
        {
            _commitments = commitments;
            _datalocks = datalocks;
            _priceEpisdodes = new List<PriceEpisode>();
        }

        private readonly List<Commitment> _commitments;
        private readonly List<DataLockPriceEpisodePeriodMatchEntity> _datalocks;

        public IReadOnlyList<PriceEpisode> PriceEpisodes { get; private set; }
        private readonly List<PriceEpisode> _priceEpisdodes;

        public void ValidatePriceEpisodes()
        {
            // ASSUMPTIONS from Looking at the live data.
            //  Datalocks are 'keyed' by UKPRN, LearnRefNumber, PriceEpisodeIdentifier and CommitmentId
            //  Where there are multiple commitmentids, one is payable and the rest are not
            //  Per unique key above, payable are either all true or all false

            // Process datalocks by price episode
            var datalocksByPriceEpisode = _datalocks.ToLookup(x => x.PriceEpisodeIdentifier);
            foreach (var datalocks in datalocksByPriceEpisode)
            {
                // Break it down by commitment
                var priceEpisodesByCommitment = datalocks.ToLookup(x => x.CommitmentId);
                foreach (var priceEpisodes in priceEpisodesByCommitment)
                {
                    var commitment = _commitments.First(x => x.CommitmentId == priceEpisodes.Key);
                    if (priceEpisodes.Any(x => x.Payable))
                    {
                        var priceEpisode = new PriceEpisode(datalocks.Key, true,
                            commitment.CommitmentId ?? 0, commitment.CommitmentVersionId,
                            commitment.AccountId ?? 0, commitment.AccountVersionId);
                        _priceEpisdodes.Add(priceEpisode);
                    }
                    else
                    {
                        var priceEpisode = new PriceEpisode(datalocks.Key, false,
                            commitment.CommitmentId ?? 0, commitment.CommitmentVersionId,
                            commitment.AccountId ?? 0, commitment.AccountVersionId);
                        _priceEpisdodes.Add(priceEpisode);
                    }
                }
            }

            PriceEpisodes = _priceEpisdodes;
        }
    }
}
