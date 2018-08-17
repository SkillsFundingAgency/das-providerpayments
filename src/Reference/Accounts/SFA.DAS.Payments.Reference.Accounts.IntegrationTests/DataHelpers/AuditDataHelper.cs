using SFA.DAS.Payments.Reference.Accounts.IntegrationTests.DataHelpers.Entities;

namespace SFA.DAS.Payments.Reference.Accounts.IntegrationTests.DataHelpers
{
    internal static class AuditDataHelper
    {
        internal static AuditEntity GetLatestAccountAuditRecord()
        {
            const string query = @"
                SELECT TOP 1 * 
                FROM DasAccountsAudit 
                WHERE AuditType = 0
                ORDER BY ReadDateTime DESC";
            return DatabaseHelper.QuerySingle<AuditEntity>(query);
        }

        internal static AuditEntity GetLatestAccountLegalEntityAuditRecord()
        {
            const string query = @"
                SELECT TOP 1 * 
                FROM DasAccountsAudit 
                WHERE AuditType = 1
                ORDER BY ReadDateTime DESC";
            return DatabaseHelper.QuerySingle<AuditEntity>(query);
        }
    }
}
