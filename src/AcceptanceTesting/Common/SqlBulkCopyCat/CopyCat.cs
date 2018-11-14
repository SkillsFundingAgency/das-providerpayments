using Dapper;
using SqlBulkCopyCat.Builder;
using SqlBulkCopyCat.Extensions;
using SqlBulkCopyCat.Model.Config;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SqlBulkCopyCat
{
    public class CopyCat
    {
        private readonly CopyCatConfig _config;
        private readonly IEnumerable<SqlRowsCopiedEventHandler> _sqlRowsCopiedEventHandlers;

        public CopyCat(CopyCatConfig copyCatConfig, IEnumerable<SqlRowsCopiedEventHandler> sqlRowsCopiedEventHandlers = null)
        {
            _config = copyCatConfig;
            _sqlRowsCopiedEventHandlers = sqlRowsCopiedEventHandlers;        
        }

        public void Copy()
        {
            SqlTransaction sqlTransaction = null;

            using (var writeConnection = new SqlConnection(_config.DestinationConnectionString))
            {
                try
                {
                    writeConnection.Open();
                    sqlTransaction = writeConnection.BeginTransaction(_config);

                    foreach (var tableMapping in _config.TableMappings.OrderBy(tm => tm.Ordinal))
                    {
                        try
                        {
                            using (var readConnection = new SqlConnection(_config.SourceConnectionString))
                            {
                                var sqlSelectCommand = tableMapping.BuildSelectSql();
                                using (var reader = readConnection.ExecuteReader(sqlSelectCommand))
                                {
                                    try
                                    {
                                        using (var bcp = SqlBulkCopyBuilder.Build(writeConnection, tableMapping,
                                            _config.SqlBulkCopySettings, sqlTransaction, _sqlRowsCopiedEventHandlers))
                                        {
                                            try
                                            {
                                                bcp.WriteToServer(reader);
                                            }
                                            catch (Exception e)
                                            {
                                                throw new Exception(
                                                    $"Exception copying to: {tableMapping.Destination} \n" +
                                                    $"Columns attempted in destination: {string.Join(", ", tableMapping.ColumnMappings.Select(x => x.Destination))}",
                                                    e);
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        throw new Exception(
                                            $"Exception copying to: {tableMapping.Destination} \n" +
                                            $"Columns attempted in destination: {string.Join(", ", tableMapping.ColumnMappings.Select(x => x.Destination))} \n" +
                                            $"Read SQL: {sqlSelectCommand}",
                                            e);
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            throw new Exception(
                                $"Exception copying to: {tableMapping.Destination} \n" +
                                $"Columns attempted in destination: {string.Join(", ", tableMapping.ColumnMappings.Select(x => x.Destination))}",
                                e);
                        }
                    }

                    if (sqlTransaction != null)
                    {
                        sqlTransaction.Commit();
                    }
                }
                catch (Exception)
                {
                    if (sqlTransaction != null)
                    {
                        sqlTransaction.Rollback();
                    }

                    throw;
                }
            }
        }
    }
}
