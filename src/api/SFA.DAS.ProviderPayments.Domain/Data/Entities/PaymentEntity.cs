namespace SFA.DAS.ProviderPayments.Domain.Data.Entities
{
    public class PaymentEntity
    {
        public string AccountId { get; set; }
        public string Ukprn { get; set; }
        public long Uln { get; set; }
        public long StandardCode { get; set; }
        public int PathwayCode { get; set; }
        public int FrameworkCode { get; set; }
        public int ProgrammeType { get; set; }
        public string ReportedPeriodCode { get; set; }
        public string DeliveryPeriodCode { get; set; }
        public decimal Amount { get; set; }
        public int TransactionType { get; set; }
        public int FundingType { get; set; }
    }
}
