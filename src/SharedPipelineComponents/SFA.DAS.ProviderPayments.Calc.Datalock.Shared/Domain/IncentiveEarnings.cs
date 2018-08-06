namespace SFA.DAS.ProviderPayments.Calc.Datalock.Shared.Domain
{
    public class IncentiveEarnings
    {
        public long Ukprn { get; set; }
        public string LearnRefNumber { get; set; }
        public int Period { get; set; }
        public decimal First16To18EmployerIncentive { get; set; }
        public decimal Second16To18EmployerIncentive { get; set; }
        public string PriceEpisodeIdentifier { get; set; }
    }
}
