namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain
{
    public class PriceEpisode
    {
        public string PriceEpisodeIdentifier { get; set; }
        public bool Payable { get; set; }
        public long CommitmentId { get; set; }
        public string CommitmentVersionId { get; set; }
        public long AccountId { get; set; }
        public string AccountVersionId { get; set; }

        public PriceEpisode(string priceEpisodeIdentifier,
            bool payable, 
            long commitmentId, 
            string commitmentVersionId, 
            long accountId, 
            string accountVersionId)
        {
            PriceEpisodeIdentifier = priceEpisodeIdentifier;
            Payable = payable;
            CommitmentId = commitmentId;
            CommitmentVersionId = commitmentVersionId;
            AccountId = accountId;
            AccountVersionId = accountVersionId;
        }
    }
}
