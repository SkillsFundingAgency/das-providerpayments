using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.Payments.Reference.Accounts.Infrastructure.Data.Entities;

namespace SFA.DAS.Payments.Reference.Accounts.Infrastructure.Data.Dcfs
{
    public class DcfsAccountRepository : DcfsRepository, IAccountRepository
    {
        private const string Source = "dbo.DasAccounts";
        private const string Columns = "AccountId, "
                                     + "AccountHashId, "
                                     + "AccountName, "
                                     + "Balance, "
                                     + "VersionId,"
                                     + "IsLevyPayer";
        private const string SingleAccountClause = "WHERE AccountId=@AccountId";
        private const string SelectByIdQuery = "SELECT " + Columns + " FROM " + Source + " " + SingleAccountClause;
        private const string InsertCommand = "INSERT INTO " + Source + " (" + Columns + ") VALUES (@AccountId,@AccountHashId,@AccountName,@Balance,@VersionId,@IsLevyPayer)";
        private const string UpdateCommand = "UPDATE " + Source + " SET "
                                           + "AccountId=@AccountId, "
                                           + "AccountHashId=@AccountHashId, "
                                           + "AccountName=@AccountName, "
                                           + "Balance=@Balance,  "
                                           + "VersionId=@VersionId, "
                                           + "IsLevyPayer=@IsLevyPayer "
                                           + SingleAccountClause;


        public DcfsAccountRepository(ContextWrapper context)
            : base(context.GetPropertyValue(ContextPropertyKeys.TransientDatabaseConnectionString))
        {
        }

        public AccountEntity GetById(long id)
        {
            return QuerySingle<AccountEntity>(SelectByIdQuery, new { AccountId = id });
        }

        public void Insert(AccountEntity account)
        {
            Execute(InsertCommand, account);
        }

        public void Update(AccountEntity account)
        {
            Execute(UpdateCommand, account);
        }
    }
}
