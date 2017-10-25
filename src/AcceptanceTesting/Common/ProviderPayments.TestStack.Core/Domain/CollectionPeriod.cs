namespace ProviderPayments.TestStack.Core.Domain
{
    public class CollectionPeriod
    {
        public int PeriodId { get; set; }
        public string Period { get; set; }
        public int CalendarMonth { get; set; }
        public int CalendarYear { get; set; }
        public byte CollectionOpen { get; set; }
        public string ActualsSchemaPeriod { get; set; }
    }
}