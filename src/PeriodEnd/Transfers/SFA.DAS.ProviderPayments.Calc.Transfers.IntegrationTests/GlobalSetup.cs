﻿using System;
using System.Data.SqlClient;
using System.IO;
using Dapper;
using NUnit.Framework;

namespace SFA.DAS.ProviderPayments.Calc.Transfers.IntegrationTests
{
    [SetUpFixture]
    public class GlobalSetup
    {
        [OneTimeSetUp]
        public void BeforeAllTests()
        {
            SetupDatabase();
        }

        private void SetupDatabase()
        {
            using (var transientConnection = new SqlConnection(GlobalTestContext.Instance.TransientConnectionString))
            using (var dedsConnection = new SqlConnection(GlobalTestContext.Instance.DedsConnectionString))
            {
                transientConnection.Open();
                dedsConnection.Open();

                try
                {
                    transientConnection.Execute($@"
                        if not exists(select 1 from sys.servers where name = '{GlobalTestContext.Instance.LinkedServerName}')
	                        EXEC master.dbo.sp_addlinkedserver @server = N'{GlobalTestContext.Instance.LinkedServerName}', @srvproduct = '', @provider = N'SQLNCLI', @datasrc = @@SERVERNAME;");

                    // Pre-req scripts
                    RunSqlScript(@"PeriodEnd.Deds.Transfers.DDL.tables.sql", dedsConnection);
                    RunSqlScript(@"1_PeriodEnd.Deds.Transfers.Initial.sql", dedsConnection);
                    
                    // Component scripts
                    
                    RunSqlScript(@"PeriodEnd.Transient.Transfers.DDL.tables.sql", transientConnection);
                    RunSqlScript(@"PeriodEnd.Transient.Transfers.ddl.views.sql", transientConnection);
                }
                finally
                {
                    transientConnection.Close();
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
            return sql.Replace("${ILR_Deds.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                      .Replace("${ILR_Summarisation.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                      .Replace("${DAS_Commitments.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                      .Replace("${DAS_PeriodEnd.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                      .Replace("${YearOfCollection}", "1617");
        }
    }
}