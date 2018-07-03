using System.Collections.Generic;

namespace SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Repositories.Interfaces
{
    public interface IBulkCopyTables
    {
        void CopyTable(string sourceTableName, string sourceConnectionString, List<string> columnList,
            string destinationTableName, string destinationConnectionString, string where = "");
    }
}
