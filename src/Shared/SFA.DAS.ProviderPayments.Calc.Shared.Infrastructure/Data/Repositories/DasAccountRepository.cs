﻿using SFA.DAS.Payments.DCFS.Infrastructure.Data;

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
    }
}