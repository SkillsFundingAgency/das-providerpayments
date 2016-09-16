using SFA.DAS.ProviderPayments.Calc.Common.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.LevyPayments.Infrastructure.Data.Repositories
{
    public class DcfsAccountRepository : DcfsRepository, IAccountRepository
    {
        private const string OutstandingAccountsSource = "LevyPayments.vw_AccountsRequiringProcessing";
        private const string AccountsColumns = "AccountId [Id], AccountName [Name], Balance";
        private const string SelectNextAccountForProcessing = "SELECT TOP 1 " + AccountsColumns + " FROM " + OutstandingAccountsSource;
        private const string SelectAccountById = "SELECT " + AccountsColumns + " FROM " + OutstandingAccountsSource + " WHERE AccountId = @AccountId";
        private const string UpdateLevySpentCommand = "LevyPayments.UpdateAccountLevySpend @AccountId, @AmountToUpdateBy";
        private const string MarkAccountAsProcessedCommand = "LevyPayments.MarkAccountAsProcessed @AccountId";

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
            Execute(UpdateLevySpentCommand, new { AccountId = accountId, AmountToUpdateBy = amount });
        }

        public void MarkAccountAsProcessed(string accountId)
        {
            Execute(MarkAccountAsProcessedCommand, new { AccountId = accountId });
        }
    }
}
