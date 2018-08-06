using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using FastMember;

namespace SFA.DAS.Payments.DCFS.Infrastructure.Data
{
    public abstract class DcfsRepository
    {
        private readonly string _connectionString;

        protected DcfsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected T[] Query<T>(string command, object param = null, int timeout = 180)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                try
                {
                    return connection.Query<T>(command, param,
                            commandTimeout: timeout)
                        .ToArray();
                }
                catch (SqlException e)
                {
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        protected T[] QueryByProc<T>(string command, DynamicParameters parameters = null, int timeout = 180)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                try
                {
                    return connection.Query<T>(command, parameters,
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: timeout)
                        .ToArray();
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        protected T QuerySingle<T>(string command, object param = null, int timeout = 180)
        {
            return Query<T>(command, param, timeout).SingleOrDefault();
        }

        protected void Execute(string command, object param = null, int timeout = 180)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                try
                {
                    connection.Execute(command, param,
                        commandTimeout: timeout);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        protected void ExecuteByProc(string command, object param = null, int timeout = 180)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                try
                {
                    connection.Execute(command, param,
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: timeout);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        protected void ExecuteBatch<T>(IEnumerable<T> batch, string destination)
        {
            var columns = typeof(T)
                .GetProperties()
                .Where(info => !info.IsDefined(typeof(NotMappedAttribute), false))
                .ToDictionary(p => p.Name, p => p.Name);

            ExecuteBatch(batch, destination, columns);
        }

        protected void ExecuteBatch<T>(IEnumerable<T> batch, string destination, IDictionary<string, string> columns)
        {
            using (var bcp = new SqlBulkCopy(_connectionString))
            {
                foreach (var column in columns)
                {
                    bcp.ColumnMappings.Add(column.Key, column.Value);
                }

                bcp.BulkCopyTimeout = 0;
                bcp.DestinationTableName = destination;
                bcp.BatchSize = 1000;

                using (var reader = ObjectReader.Create(batch, columns.Keys.ToArray()))
                {
                    bcp.WriteToServer(reader);
                }
            }
        }
    }
}
