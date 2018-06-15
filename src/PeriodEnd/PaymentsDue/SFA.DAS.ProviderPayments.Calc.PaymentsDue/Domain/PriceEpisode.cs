using System.Collections.Generic;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain
{
    public class PriceEpisode : IHoldCommitmentInformation
    {
        public string PriceEpisodeIdentifier { get; set; }
        public List<int> PeriodsToIgnore { get; set; }
        public long? CommitmentId { get; set; }
        public string CommitmentVersionId { get; set; }
        public long? AccountId { get; set; }
        public string AccountVersionId { get; set; }
        public bool MustRefundPriceEpisode { get; set; }
        public bool SuccesfulDatalock { get; set; }

        public PriceEpisode(string priceEpisodeIdentifier,
            List<int> periodsToIgnore, 
            long commitmentId, 
            string commitmentVersionId, 
            long accountId, 
            string accountVersionId,
            bool successfulDatalock,
            bool mustRefundEpisode = false)
        {
            PriceEpisodeIdentifier = priceEpisodeIdentifier;
            PeriodsToIgnore = periodsToIgnore;
            CommitmentId = commitmentId;
            CommitmentVersionId = commitmentVersionId;
            AccountId = accountId;
            AccountVersionId = accountVersionId;
            MustRefundPriceEpisode = mustRefundEpisode;
            SuccesfulDatalock = successfulDatalock;
        }
    }
}
