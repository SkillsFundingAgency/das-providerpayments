using System.IO;
using System.Text.RegularExpressions;
using ProviderPayments.TestStack.Core.Context;

namespace ProviderPayments.TestStack.Core.Workflow.RebuildDedsDatabase.Tasks
{
    internal class ExecuteDedsScriptsTask : BaseRebuildDedsDatabaseTask
    {
        private readonly ILogger _logger;

        public ExecuteDedsScriptsTask(ILogger logger)
        {
            _logger = logger;

            Id = "ExecuteDedsScripts";
            Description = "Execute DEDS scripts";
        }

        internal override void Execute(TestStackContext context)
        {
            var componentDirectory = GetComponentWorkingDirectory(context);
            var sqlDirectory = Path.Combine(componentDirectory, "sql", "ddl");
            if (!Directory.Exists(sqlDirectory))
            {
                sqlDirectory = Path.Combine(componentDirectory, context.OpaRulebaseYear, "sql", "ddl");
            }
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

                        var isDedsScript = Regex.IsMatch(sqlFile, @".*\.deds\..*\.sql$", RegexOptions.IgnoreCase);
                        if (isDedsScript)
                        {
                            var sql = File.ReadAllText(sqlFile);

                            _logger.Debug($"Executing {sqlFile}");
                            ExecuteSqlScript(sql, dedsConnection, context);
                        }
                        else
                        {
                            _logger.Info($"Skipping {sqlFile} as it for transient");
                        }
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
