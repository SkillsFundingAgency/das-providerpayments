using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProviderPayments.TestStack.Domain.Data;
using ProviderPayments.TestStack.Domain.Data.Entities;

namespace ProviderPayments.TestStack.Infrastructure.Data
{
    public class SqlServerAccountRepository : SqlServerRepository, IAccountRepository
    {
        public SqlServerAccountRepository()
            : base("DedsConnectionString")
        {
        }

        public async Task<IEnumerable<AccountEntity>> All()
        {
            return await Query<AccountEntity>(@"SELECT AccountId [Id]
                                                      ,AccountHashId [HashId]
                                                      ,AccountName [Name]
                                                      ,Balance
                                                      ,AccountHashId [HashId]
                                                      ,VersionId
                                                FROM dbo.DasAccounts");
        }

        public async Task<AccountEntity> Single(long id)
        {
            return (await Query<AccountEntity>(@"SELECT AccountId [Id]
                                                       ,AccountHashId [HashId]
                                                       ,AccountName [Name]
                                                       ,Balance
                                                       ,AccountHashId [HashId]
                                                       ,VersionId
                                                 FROM dbo.DasAccounts
                                                 WHERE AccountId = @Id", new { id })).SingleOrDefault();
        }

        public async Task Create(AccountEntity entity)
        {
            await Execute(@"INSERT INTO dbo.DasAccounts
                            (AccountId,AccountHashId,AccountName,Balance,VersionId)
                            VALUES
                            (@Id,@Id,@Name,@Balance,@VersionId)",
                            entity);
        }

        public async Task Update(AccountEntity entity)
        {
            await Execute(@"UPDATE dbo.DasAccounts
                            SET AccountName = @Name,
                                AccountHashId = @Id,
                                Balance = @Balance,
                                VersionId = @VersionId
                            WHERE AccountId = @Id",
                          entity);
        }

        public async Task Delete(long id)
        {
            await Execute("DELETE FROM dbo.DasAccounts WHERE AccountId = @Id", new { id });
        }

        public async Task UpdateAudit()
        {
            await Execute("INSERT INTO DasAccountsAudit (ReadDateTime,AccountsRead,CompletedSuccessfully) SELECT GETDATE(), COUNT(AccountId),1 FROM dbo.DasAccounts");
        }
    }
}
