using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Infrastructure.Data.Entities
{
    public class CollectionPeriodEntity
    {
        [Range(1, 14)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string AcademicYear { get; set; }
        public string CollectionPeriodName => $"{AcademicYear}-{Name}";
        [Range(1, 12)]
        public int Month { get; set; }
        [Range(2017, 2019)]
        public int Year { get; set; }
        public bool Open { get; set; }
    }
}