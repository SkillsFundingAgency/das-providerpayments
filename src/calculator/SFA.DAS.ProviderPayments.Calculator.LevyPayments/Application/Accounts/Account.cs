namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.Application.Accounts
{
    public class Account
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Commitment[] Commitments { get; set; }
    }
}
