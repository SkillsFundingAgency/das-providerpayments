using System.Collections.Generic;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain
{
    public class PriceEpisode : IHoldCommitmentInformation
    {
        public string PriceEpisodeIdentifier { get; set; }
        public List<int> PayablePeriods { get; set; }
        public long? CommitmentId { get; set; }
        public string CommitmentVersionId { get; set; }
        public long? AccountId { get; set; }
        public string AccountVersionId { get; set; }

        public PriceEpisode(string priceEpisodeIdentifier,
            List<int> payablePeriods, 
            long commitmentId, 
            string commitmentVersionId, 
            long accountId, 
            string accountVersionId)
        {
            PriceEpisodeIdentifier = priceEpisodeIdentifier;
            PayablePeriods = payablePeriods;
            CommitmentId = commitmentId;
            CommitmentVersionId = commitmentVersionId;
            AccountId = accountId;
            AccountVersionId = accountVersionId;
        }
    }
}
