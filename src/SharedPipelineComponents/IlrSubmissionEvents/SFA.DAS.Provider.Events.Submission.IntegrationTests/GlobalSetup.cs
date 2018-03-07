using System.Data.SqlClient;
using Dapper;
using NUnit.Framework;
using SFA.DAS.Provider.Events.Submission.IntegrationTests.TestContext;

namespace SFA.DAS.Provider.Events.Submission.IntegrationTests
{
    [SetUpFixture]
    public class GlobalSetup
    {

        [OneTimeSetUp]
        public void BeforeAllTests()
        {
            SetupDedsDatabase();
            SetupTransientDatabase();
        }


        private void SetupDedsDatabase()
        {
            using (var connection = new SqlConnection(GlobalTestContext.Current.DedsDatabaseConnectionString))
            {
                connection.RunDbSetupSqlScriptFile("submissions.deds.ddl.tables.sql", GlobalTestContext.Current.DedsDatabaseNameBracketed, GlobalTestContext.Current.LinkedServerName);
                connection.RunDbSetupSqlScriptFile("~2_Submissions.Deds.AddColumns.sql", GlobalTestContext.Current.DedsDatabaseNameBracketed, GlobalTestContext.Current.LinkedServerName);
            }
        }

        private void SetupTransientDatabase()
        {
            using (var connection = new SqlConnection(GlobalTestContext.Current.TransientDatabaseConnectionString))
            {
                connection.Execute($@"
                        if not exists(select 1 from sys.servers where name = '{GlobalTestContext.Current.LinkedServerName}')
	                        EXEC master.dbo.sp_addlinkedserver @server = N'{GlobalTestContext.Current.LinkedServerName}', @srvproduct = '', @provider = N'SQLNCLI', @datasrc = @@SERVERNAME;");

                connection.RunDbSetupSqlScriptFile("datalock.transient.ddl.tables.sql", GlobalTestContext.Current.DedsDatabaseNameBracketed, GlobalTestContext.Current.LinkedServerName);
                connection.RunDbSetupSqlScriptFile("ilr.transient.ddl.tables.sql", GlobalTestContext.Current.DedsDatabaseNameBracketed, GlobalTestContext.Current.LinkedServerName);

                connection.RunDbSetupSqlScriptFile("submissions.transient.ddl.tables.sql", GlobalTestContext.Current.DedsDatabaseNameBracketed, GlobalTestContext.Current.LinkedServerName);
                connection.RunDbSetupSqlScriptFile("submissions.transient.ddl.functions.sql", GlobalTestContext.Current.DedsDatabaseNameBracketed, GlobalTestContext.Current.LinkedServerName);
                connection.RunDbSetupSqlScriptFile("submissions.transient.ddl.sprocs.sql", GlobalTestContext.Current.DedsDatabaseNameBracketed, GlobalTestContext.Current.LinkedServerName);
                connection.RunDbSetupSqlScriptFile("submissions.transient.ddl.views.sql", GlobalTestContext.Current.DedsDatabaseNameBracketed, GlobalTestContext.Current.LinkedServerName);
            }
        }

    }
}
