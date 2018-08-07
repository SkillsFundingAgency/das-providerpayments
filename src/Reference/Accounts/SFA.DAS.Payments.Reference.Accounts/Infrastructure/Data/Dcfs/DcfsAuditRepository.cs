using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.Payments.Reference.Accounts.Infrastructure.Data.Entities;

namespace SFA.DAS.Payments.Reference.Accounts.Infrastructure.Data.Dcfs
{
    public class DcfsAuditRepository : DcfsRepository, IAuditRepository
    {
        public DcfsAuditRepository(ContextWrapper context) 
            : base(context.GetPropertyValue(ContextPropertyKeys.TransientDatabaseConnectionString))
        {
        }

        public void CreateAudit(AuditEntity entity)
        {
            const string command = @"
                INSERT INTO DasAccountsAudit (
                    ReadDateTime
                    ,AccountsRead
                    ,CompletedSuccessfully
                    ,AuditType
                ) VALUES (
                    @ReadDateTime
                    ,@AccountsRead
                    ,@CompletedSuccessfully
                    ,@AuditType
                );";

            Execute(command, entity);
        }
    }
}
