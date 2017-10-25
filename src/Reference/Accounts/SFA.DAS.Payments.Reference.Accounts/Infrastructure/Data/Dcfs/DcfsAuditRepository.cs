using System;
using SFA.DAS.Payments.DCFS.Context;

namespace SFA.DAS.Payments.Reference.Accounts.Infrastructure.Data.Dcfs
{
    public class DcfsAuditRepository : DcfsRepository, IAuditRepository
    {
        public DcfsAuditRepository(ContextWrapper context) 
            : base(context.GetPropertyValue(ContextPropertyKeys.TransientDatabaseConnectionString))
        {
        }

        public void CreateAudit(DateTime readDate, long accountsRead, bool completedSuccessfully)
        {
            Execute("INSERT INTO DasAccountsAudit (ReadDateTime,AccountsRead,CompletedSuccessfully) " +
                    "VALUES (@readDate,@accountsRead,@completedSuccessfully)",
                new {readDate, accountsRead, completedSuccessfully});
        }
    }
}
