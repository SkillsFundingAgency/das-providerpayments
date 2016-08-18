namespace SFA.DAS.ProviderPayments.Api.Dto
{
    public class PaymentDto
    {
        public AccountDto Account { get; set; }
        public ProviderDto Provider { get; set; }
        public ApprenticeshipDto Apprenticeship { get; set; }
        public PeriodDto ReportedPeriod { get; set; }
        public PeriodDto DeliveryPeriod { get; set; }
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public FundingType FundingType { get; set; }
    }
}
