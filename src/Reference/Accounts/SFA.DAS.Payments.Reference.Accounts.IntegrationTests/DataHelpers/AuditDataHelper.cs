using SFA.DAS.Payments.Reference.Accounts.IntegrationTests.DataHelpers.Entities;

namespace SFA.DAS.Payments.Reference.Accounts.IntegrationTests.DataHelpers
{
    internal static class AuditDataHelper
    {
        internal static AuditEntity GetLatestAuditRecord()
        {
            return DatabaseHelper.QuerySingle<AuditEntity>("SELECT TOP 1 * FROM DasAccountsAudit ORDER BY ReadDateTime DESC");
        }
    }
}
