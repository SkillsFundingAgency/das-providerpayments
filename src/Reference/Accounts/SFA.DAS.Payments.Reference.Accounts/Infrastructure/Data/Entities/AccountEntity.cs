namespace SFA.DAS.Payments.Reference.Accounts.Infrastructure.Data.Entities
{
    public class AccountEntity
    {
        public long AccountId { get; set; }
        public string AccountHashId { get; set; }
        public string AccountName { get; set; }
        public decimal Balance { get; set; }
        public string VersionId { get; set; }
        public bool IsLevyPayer { get; set; }

        public decimal? TransferAllowance { get; set; }
    }
}
