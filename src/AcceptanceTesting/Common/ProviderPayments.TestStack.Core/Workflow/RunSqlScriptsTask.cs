using System.Data.SqlClient;
using ProviderPayments.TestStack.Core.Context;

namespace ProviderPayments.TestStack.Core.Workflow
{
    internal class RunSqlScriptsTask : WorkflowTask
    {
        private readonly string[] _transientSqlScripts;
        private readonly string[] _dedsSqlScripts;
        private readonly ILogger _logger;

        public RunSqlScriptsTask(string[] transientSqlScripts, string[] dedsSqlScripts, ILogger logger)
        {
            _transientSqlScripts = transientSqlScripts;
            _dedsSqlScripts = dedsSqlScripts;
            _logger = logger;
        }

        internal override void Execute(TestStackContext context)
        {
            if (_transientSqlScripts != null && _transientSqlScripts.Length > 0)
            {
                using (var connection = GetOpenTransientConnection(context))
                {
                    try
                    {
                        foreach (var sql in _transientSqlScripts)
                        {
                            ExecuteSqlScript(sql, connection, context);
                        }
                    }
                    catch (SqlException)
                    {
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            if (_dedsSqlScripts != null && _dedsSqlScripts.Length > 0)
            {
                using (var connection = GetOpenDedsConnection(context))
                {
                    try
                    {
                        foreach (var sql in _dedsSqlScripts)
                        {
                            ExecuteSqlScript(sql, connection, context);
                        }
                    }
                    catch (SqlException)
                    {
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
    }
}
