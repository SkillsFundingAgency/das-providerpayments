using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.Payments.Reference.Accounts.Infrastructure.Data.Entities;

namespace SFA.DAS.Payments.Reference.Accounts.Infrastructure.Data.Dcfs
{
    public class DcfsAccountRepository : DcfsRepository, IAccountRepository
    {
        private const string SelectByIdQuery = @"   SELECT AccountId, 
                                                        AccountHashId, 
                                                        AccountName, 
                                                        Balance, 
                                                        VersionId,
                                                        IsLevyPayer,
                                                        TransferAllowance
                                                    FROM dbo.DasAccounts
                                                    Where AccountId = @AccountId";
        private const string InsertCommand = @" INSERT INTO dbo.DasAccounts
                                                (
                                                    AccountId, 
                                                    AccountHashId, 
                                                    AccountName, 
                                                    Balance, 
                                                    VersionId,
                                                    IsLevyPayer,
                                                    TransferAllowance
                                                ) 
                                                VALUES 
                                                (
                                                    @AccountId,
                                                    @AccountHashId,
                                                    @AccountName,
                                                    @Balance,
                                                    @VersionId,
                                                    @IsLevyPayer,
                                                    @TransferAllowance
                                                )";

        private const string UpdateCommand = @" UPDATE dbo.DasAccounts 
                                                SET AccountId = @AccountId,
                                                    AccountHashId = @AccountHashId, 
                                                    AccountName = @AccountName, 
                                                    Balance = @Balance,  
                                                    VersionId = @VersionId, 
                                                    IsLevyPayer = @IsLevyPayer, 
                                                    TransferAllowance = @TransferAllowance
                                                WHERE AccountId=@AccountId";


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