namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain
{
    /// <summary>
    /// Value object for determining whether two completion payments should be
    /// </summary>
    public sealed class CompletionPaymentGroup : BaseEquatableGroup<CompletionPaymentGroup>
    {
        public CompletionPaymentGroup(decimal priceEpisodeCumulativePmrs,
            int priceEpisodeCompExemCode)
        {
            PriceEpisodeCumulativePmrs = priceEpisodeCumulativePmrs;
            PriceEpisodeCompExemCode = priceEpisodeCompExemCode;
        }

        public decimal PriceEpisodeCumulativePmrs { get; }
        public int PriceEpisodeCompExemCode { get; }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = 31 * hash + PriceEpisodeCumulativePmrs.GetHashCode();
                hash = 31 * hash + PriceEpisodeCompExemCode.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(CompletionPaymentGroup test)
        {
            if (base.Equals(test))
                return true;

            if (test == null)
                return false;

            return PriceEpisodeCumulativePmrs == test.PriceEpisodeCumulativePmrs &&
                   PriceEpisodeCompExemCode == test.PriceEpisodeCompExemCode;
        }
    }
}