using System;
using System.Data.SqlClient;
using System.IO;
using Dapper;
using NUnit.Framework;
using SFA.DAS.Payments.Reference.Commitments.Infrastructure.DependencyResolution;
using SFA.DAS.Payments.Reference.Commitments.IntegrationTests.StubbedInfrastructure;

namespace SFA.DAS.Payments.Reference.Commitments.IntegrationTests
{

    [SetUpFixture]
    public class GlobalSetup
    {
        [OneTimeSetUp]
        public void BeforeAllTests()
        {
            SetupDatabase();
            SetupEventsApiStub();
        }

        private void SetupDatabase()
        {
            using (var connection = new SqlConnection(GlobalTestContext.Instance.TransientConnectionString))
            {
                connection.Open();
                try
                {
                    // Pre-req scripts

                    // Component scripts
                    RunSqlScript(@"ddl.transient.commitments.tables.sql", connection);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        private void RunSqlScript(string fileName, SqlConnection connection)
        {
            var path = Path.Combine(GlobalTestContext.Instance.AssemblyDirectory, "DbSetupScripts", fileName);
            var sql = ReplaceSqlTokens(File.ReadAllText(path));
            var commands = sql.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var command in commands)
            {
                connection.Execute(command);
            }
        }
        private string ReplaceSqlTokens(string sql)
        {
            return sql.Replace("${ILR_Current.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                      .Replace("${ILR_Previous.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                      .Replace("${DAS_Accounts.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                      .Replace("${DAS_Commitments.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                      .Replace("${ILR_Summarisation.FQ}", GlobalTestContext.Instance.BracketedDatabaseName);
        }

        private void SetupEventsApiStub()
        {
            ApiClientFactory.Instance = new IntegrationApiClientFactory();
        }
    }
}
