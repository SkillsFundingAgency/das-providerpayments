using System;
using System.Data.SqlClient;
using System.IO;
using Dapper;
using NUnit.Framework;

namespace SFA.DAS.ProviderPayments.Calc.ManualAdjustments.IntegrationTests
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
                    // Pre-req scripts
                    transientConnection.RunSqlScript(@"ExternalComponents\\PaymentsDue\\PeriodEnd.Transient.PaymentsDue.DDL.tables.sql");
                    transientConnection.RunSqlScript(@"ExternalComponents\\PeriodEnd.Transient.Reference.Accounts.ddl.tables.sql");
                    transientConnection.RunSqlScript(@"ExternalComponents\\PeriodEnd.Transient.Reference.CollectionPeriods.ddl.tables.sql");
                    transientConnection.RunSqlScript(@"ExternalComponents\\CoInvested\\PeriodEnd.Transient.CoInvestedPayments.ddl.tables.sql");
                    transientConnection.RunSqlScript(@"ExternalComponents\\Levy\\PeriodEnd.Transient.LevyPayments.ddl.tables.sql");
                    transientConnection.RunSqlScript(@"ExternalComponents\\Levy\\PeriodEnd.Transient.LevyPayments.ddl.sprocs.sql");
                    transientConnection.RunSqlScript(@"ExternalComponents\\PaymentsDue\\PeriodEnd.Transient.Reference.PaymentsDue.DDL.tables.sql");
                    transientConnection.RunSqlScript(@"ExternalComponents\\Refunds\\PeriodEnd.Transient.Reference.Refunds.DDL.tables.sql");

                    transientConnection.RunSqlScript(@"DDL\\PeriodEnd.Transient.ManualAdjustments.ddl.tables.sql");

                    // Component scripts
                    dedsConnection.RunSqlScript(@"DDL\\PeriodEnd.Deds.ManualAdjustments.ddl.tables.sql");
                    dedsConnection.RunSqlScript(@"DDL\\PeriodEnd.Deds.ManualAdjustments.ddl.procs.sql");
                    dedsConnection.RunSqlScript(@"DDL\\PeriodEnd.Deds.ManualAdjustments.ddl.tables.sql");
                    dedsConnection.RunSqlScript(@"Migration Scripts\\1_PeriodEnd.ManualAdjustments.deds.Add_Indexes.sql");
                }
                finally
                {
                    transientConnection.Close();
                    dedsConnection.Close();
                }
            }
        }
    }
}
