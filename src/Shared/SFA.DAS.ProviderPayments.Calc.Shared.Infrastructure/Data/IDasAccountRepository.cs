namespace SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data
{
    public interface IDasAccountRepository
    {
        void AdjustBalance(long accountId, decimal balance);
    }
}