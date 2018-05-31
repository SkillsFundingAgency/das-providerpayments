namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Application
{
    public class ComparableEarning
    {
        public string LearnRefNumber { get; set; }
        public long Ukprn { get; set; }
        public int PriceEpisodeAimSeqNumber { get; set; }
        public string PriceEpisodeIdentifier { get; set; }
        public long Uln { get; set; }
    }
}