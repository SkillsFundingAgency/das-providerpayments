using System;

namespace SFA.DAS.Payments.Reference.Accounts.Infrastructure.Data
{
    public interface IAuditRepository
    {
        void CreateAudit(DateTime readDate, long accountsRead, bool completedSuccessfully);
    }
}