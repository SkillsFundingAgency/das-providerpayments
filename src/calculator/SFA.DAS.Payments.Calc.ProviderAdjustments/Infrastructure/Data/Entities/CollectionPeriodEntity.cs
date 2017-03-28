namespace SFA.DAS.Payments.Calc.ProviderAdjustments.Infrastructure.Data.Entities
{
    public class CollectionPeriodEntity
    {
        public int PeriodId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string Name { get; set; }
    }
}