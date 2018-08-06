using System.Collections.Generic;
using SFA.DAS.Payments.DCFS.Infrastructure.Data;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Entities;

namespace SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Data.Repositories
{
    public class DasAccountRepository : DcfsRepository, IDasAccountRepository
    {
        public DasAccountRepository(string connectionString) 
            : base(connectionString) { }

        public void AdjustBalance(long accountId, decimal balance)
        {
            var sql = @"
                update Reference.DasAccounts
                set Balance = Balance + @balance
                where AccountId = @accountId;
                ";

            Execute(sql, new {accountId, balance});
        }

        public IEnumerable<DasAccounEntity> GetDasAccounts()
        {
            const string sql = @"
                    SELECT 
                        AccountId,
                        IsLevyPayer
                    FROM
                        Reference.DasAccounts
                ";

            return Query<DasAccounEntity>(sql);
        }
    }
}