using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Repositories.Interfaces;

namespace SFA.DAS.ProviderPayments.Calc.Shared.Infrastructure.Repositories
{
    public class BulkCopyTableService : IBulkCopyTables
    {
        public void InsertRows(IDataReader source, string destinationTableName, List<string> columnList, string destinationConnectionString)
        {
            using (var connection = new SqlConnection(destinationConnectionString))
            using (var scp = new SqlBulkCopy(connection))
            {
                scp.DestinationTableName = destinationTableName;
                scp.BulkCopyTimeout = 7200;
                foreach (var columnName in columnList)
                {
                    scp.ColumnMappings.Add(new SqlBulkCopyColumnMapping(columnName, columnName));
                }
                
                scp.WriteToServer(source);
            }
        }

        public void CopyTable(string sourceTableName, string sourceConnectionString, List<string> columnList,
            string destinationTableName, string destinationConnectionString, string where = "")
        {
            using (var connection = new SqlConnection(sourceConnectionString))
            {
                var sqlText = $"SELECT {string.Join(",", columnList)} FROM {sourceTableName} ";
                if (where.Length > 0)
                {
                    sqlText += where;
                }

                var command = new SqlCommand(sqlText, connection);
                using (var reader = command.ExecuteReader())
                {
                    InsertRows(reader, destinationTableName, columnList, destinationConnectionString);
                }
            }
        }
    }
}
