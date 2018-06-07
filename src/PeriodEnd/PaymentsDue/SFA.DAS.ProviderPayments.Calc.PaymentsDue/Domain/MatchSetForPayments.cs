namespace SFA.DAS.ProviderPayments.Calc.PaymentsDue.Domain
{
    class MatchSetForPayments
    {
        public int StandardCode { get; set; }
        public int FrameworkCode { get; set; }
        public int ProgrammeType { get; set; }
        public int PathwayCode { get; set; }
        public int ApprenticeshipContractType { get; set; }
        public int TransactionType { get; set; }
        public decimal SfaContributionPercentage { get; set; }
        public string LearnAimRef { get; set; }
        public string FundingLineType { get; set; }
        public int DeliveryYear { get; set; }
        public int DeliveryMonth { get; set; }
        public long? AccountId { get; set; }
        public long? CommitmentId { get; set; }
    }
}