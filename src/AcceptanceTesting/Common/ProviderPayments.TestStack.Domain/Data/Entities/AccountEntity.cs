namespace ProviderPayments.TestStack.Domain.Data.Entities
{
    public class AccountEntity
    {
        public long Id { get; set; }
        public string HashId { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public string VersionId { get; set; }
    }
}
