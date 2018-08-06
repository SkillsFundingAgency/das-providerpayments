namespace SFA.DAS.ProviderPayments.Calc.Datalock.Shared.Infrastructure.Data.Entities
{
    public class IncentiveEarningsEntity
    {
        public long Ukprn { get; set; }
        public string LearnRefNumber { get; set; }
        public int Period { get; set; }
        public decimal PriceEpisodeFirstEmp1618Pay { get; set; }
        public decimal PriceEpisodeSecondEmp1618Pay { get; set; }
        public string PriceEpisodeIdentifier { get; set; }
    }
}
