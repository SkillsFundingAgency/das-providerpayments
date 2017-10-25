namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Infrastructure
{
    public interface IAccountRepository
    {
        void AdjustAccountBalance(string accountId, decimal amountToAdjustBy);
    }
}