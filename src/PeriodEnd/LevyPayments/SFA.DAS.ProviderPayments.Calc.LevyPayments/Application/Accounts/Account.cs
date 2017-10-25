namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Application.Accounts
{
    public class Account
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Commitment[] Commitments { get; set; }
    }
}
