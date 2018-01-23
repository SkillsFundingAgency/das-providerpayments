using System.IO;
using System.Text.RegularExpressions;
using ProviderPayments.TestStack.Core.Context;

namespace ProviderPayments.TestStack.Core.Workflow.RebuildDedsDatabase.Tasks
{
    internal class ExecuteMigrationScriptsTask : BaseRebuildDedsDatabaseTask
    {
        private readonly ILogger _logger;

        public ExecuteMigrationScriptsTask(ILogger logger)
        {
            _logger = logger;

            Id = "ExecuteMigrationScriptsTask";
            Description = "Execute ExecuteMigrationScriptsTask scripts";
        }

        internal override void Execute(TestStackContext context)
        {
            var componentDirectory = GetComponentWorkingDirectory(context);
            var sqlDirectory = Path.Combine(componentDirectory, "sql", "migrationscripts");
            if (!Directory.Exists(sqlDirectory))
            {
                return;
            }

            using (var dedsConnection = GetOpenDedsConnection(context))
            {
                try
                {
                    foreach (var sqlFile in Directory.GetFiles(sqlDirectory))
                    {
                        _logger.Debug($"Found script {sqlFile}");
                        var sql = File.ReadAllText(sqlFile);

                        _logger.Debug($"Executing {sqlFile}");
                        ExecuteSqlScript(sql, dedsConnection, context);

                    }
                }
                finally
                {
                    dedsConnection.Close();
                }
            }
        }
    }
}
