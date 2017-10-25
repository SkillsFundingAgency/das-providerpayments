namespace ProviderPayments.TestStack.Domain
{
    public class Account
    {
        public long Id { get; set; }
        public string HashId { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
    }
}