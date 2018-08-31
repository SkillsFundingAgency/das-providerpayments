namespace SFA.DAS.ProviderPayments.Calc.Shared.Interfaces.Payments
{
    public interface IIdentifyCommitments
    {
        string LearnRefNumber { get; set; }
        int AimSeqNumber { get; set; }
        string PriceEpisodeIdentifier { get; set; }
        long Ukprn { get; set; }
    }
}