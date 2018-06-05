namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities
{
    public class CollectionPeriodEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AcademicYear { get; set; }
        public string CollectionPeriodName => $"{AcademicYear}-{Name}";
        public int Month { get; set; }
        public int Year { get; set; }
        public bool Open { get; set; }
    }
}