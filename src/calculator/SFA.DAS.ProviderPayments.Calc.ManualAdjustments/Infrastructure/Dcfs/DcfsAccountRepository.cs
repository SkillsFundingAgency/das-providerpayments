using SFA.DAS.Payments.DCFS.Infrastructure.Data;

namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments.Infrastructure.Dcfs
{
    public class DcfsAccountRepository : DcfsRepository, IAccountRepository
    {
        public DcfsAccountRepository(string connectionString) 
            : base(connectionString)
        {
        }

        public void AdjustAccountBalance(string accountId, decimal amountToAdjustBy)
        {
            amountToAdjustBy *= -1;
            Execute("EXEC LevyPayments.UpdateAccountLevySpend @accountId, @amountToAdjustBy",
                new { accountId, amountToAdjustBy });
        }
    }
}