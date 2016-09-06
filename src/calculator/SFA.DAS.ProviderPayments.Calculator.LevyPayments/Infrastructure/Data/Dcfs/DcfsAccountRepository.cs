using SFA.DAS.ProviderPayments.Calculator.LevyPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.Infrastructure.Data.Dcfs
{
    public class DcfsAccountRepository : DcfsRepository, IAccountRepository
    {
        private const string OutstandingAccountsSource = "LevyPayments.vw_AccountsRequiringProcessing";
        private const string AccountsColumns = "AccountId, AccountName, Balance";
        private const string SelectNextAccountForProcessing = "SELECT TOP 1 " + AccountsColumns + " FROM " + OutstandingAccountsSource;
        private const string SelectAccountById = "SELECT " + AccountsColumns + " FROM " + OutstandingAccountsSource + " WHERE AccountId = @AccountId";
        private const string UpdateLevySpent = "LevyPayments.UpdateAccountLevySpend";

        public DcfsAccountRepository(string connectionString)
            : base(connectionString)
        {
        }

        public AccountEntity GetNextAccountRequiringProcessing()
        {
            return QuerySingle<AccountEntity>(SelectNextAccountForProcessing);
        }

        public AccountEntity GetAccountById(string id)
        {
            return QuerySingle<AccountEntity>(SelectAccountById, new { AccountId = id });
        }

        public void SpendLevy(string accountId, decimal amount)
        {
            Execute(UpdateLevySpent, new { AccountId = accountId, AmountToUpdateBy = amount });
        }
    }
}
