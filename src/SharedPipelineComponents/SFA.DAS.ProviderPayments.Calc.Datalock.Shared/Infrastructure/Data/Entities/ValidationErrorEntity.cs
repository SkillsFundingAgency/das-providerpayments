namespace SFA.DAS.ProviderPayments.Calc.Datalock.Shared.Infrastructure.Data.Entities
{
    public class ValidationErrorEntity
    {
        public long Ukprn { get; set; }
        public string LearnRefNumber { get; set; }
        public long? AimSeqNumber { get; set; }
        public string RuleId { get; set; }
        public string PriceEpisodeIdentifier { get; set; }
    }
}
