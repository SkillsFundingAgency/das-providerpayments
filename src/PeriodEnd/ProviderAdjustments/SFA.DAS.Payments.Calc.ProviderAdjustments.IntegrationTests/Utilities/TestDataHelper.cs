using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Dapper;
using SFA.DAS.Payments.Calc.ProviderAdjustments.Infrastructure.Data.Entities;

namespace SFA.DAS.Payments.Calc.ProviderAdjustments.IntegrationTests.Tools
{
    public class TestDataHelper
    {
        private static readonly string[] PeriodEndCopyReferenceDataScripts =
        {
            "01 PeriodEnd.Populate.Reference.CollectionPeriods.dml.sql",
            "05 PeriodEnd.ProviderAdjustments.Populate.Reference.Providers.dml.sql",
            "06 PeriodEnd.ProviderAdjustments.Populate.Reference.Current.dml.sql",
            "07 PeriodEnd.ProviderAdjustments.Populate.Reference.History.dml.sql"
        };

        internal static void Clean()
        {
            Execute(@"
                    DECLARE @SQL NVARCHAR(MAX) = ''

                    SELECT @SQL = (
                        SELECT 'TRUNCATE TABLE [' + s.name + '].[' + o.name + ']' + CHAR(13)
                        FROM sys.objects o WITH (NOWAIT)
                        JOIN sys.schemas s WITH (NOWAIT) ON o.[schema_id] = s.[schema_id]
                        WHERE o.[type] = 'U'
                            AND s.name IN ('ProviderAdjustments', 'Reference')
                        FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)')

                    EXEC sys.sp_executesql @SQL                
                ");

            Execute(@"
                    DECLARE @SQL NVARCHAR(MAX) = ''

                    SELECT @SQL = (
                        SELECT 'TRUNCATE TABLE [' + s.name + '].[' + o.name + ']' + CHAR(13)
                        FROM sys.objects o WITH (NOWAIT)
                        JOIN sys.schemas s WITH (NOWAIT) ON o.[schema_id] = s.[schema_id]
                        WHERE o.[type] = 'U'
                            AND s.name IN ('dbo', 'ProviderAdjustments')
                            AND o.name NOT IN ('Collection_Period_Mapping', 'Payment_Types')
                        FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)')

                    EXEC sys.sp_executesql @SQL                
                ", null, false);
        }

        internal static void CopyReferenceData()
        {
            foreach (var script in PeriodEndCopyReferenceDataScripts)
            {
                var sql = File.ReadAllText($@"{AppDomain.CurrentDomain.BaseDirectory}\Utilities\Sql\Copy Reference Data\{script}");

                var commands = ReplaceSqlTokens(sql).Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var command in commands)
                {
                    Execute(command);
                }
            }
        }

        internal static void AddProvider(long ukprn)
        {
            var submissionId = Guid.NewGuid();

            Execute("INSERT INTO dbo.EAS_Submission "
                    + "(Submission_Id, UKPRN, CollectionPeriod, ProviderName, UpdatedOn, DeclarationChecked, NilReturn) "
                    + "VALUES "
                    + "(@submissionId, @ukprn, 1, 'Provider', GETDATE(), 1, 0)",
                    new { submissionId, ukprn }, false);
        }

        internal static void AddProviderAdjustmentsSubmission(long ukprn, decimal amount = 1000.00m, int currentPeriod = 12)
        {
            var submissionId = Guid.NewGuid();

            for (var period = 1; period <= currentPeriod; period++)
            {
                Execute("INSERT INTO dbo.EAS_Submission "
                        + "(Submission_Id, UKPRN, CollectionPeriod, ProviderName, UpdatedOn, DeclarationChecked, NilReturn) "
                        + "VALUES "
                        + "(@submissionId, @ukprn, @period, 'Provider', GETDATE(), 1, 0)",
                        new { submissionId, ukprn, period }, false);

                Execute("INSERT INTO dbo.EAS_Submission_Values "
                        + "(Submission_Id, CollectionPeriod, Payment_Id, PaymentValue) "
                        + "VALUES "
                        + "(@submissionId, @period, 58, @amount)",
                        new { submissionId, period, amount }, false);
            }
        }

        internal static void AddPreviousProviderAdjustments(long ukprn, decimal amount = 500.00m, int currentPeriod = 12, int academicYear = 1617)
        {
            var submissionId = Guid.NewGuid();

            for (var period = 1; period <= currentPeriod; period++)
            {
                Execute("INSERT INTO ProviderAdjustments.Payments "
                        + "(Ukprn, SubmissionId, SubmissionCollectionPeriod, SubmissionAcademicYear, PaymentType, PaymentTypeName, Amount, CollectionPeriodName, CollectionPeriodMonth, CollectionPeriodYear) "
                        + "VALUES "
                        + "(@ukprn, @submissionId, @period, @academicYear, 58, 'Audit Adjustments: 16-18 Levy Apprenticeships - Provider', @amount, '1617-R09', 4, 2017)",
                        new { ukprn, submissionId, period, academicYear, amount }, false);
            }
        }

        internal static PaymentEntity[] GetPaymentsForProvider(long ukprn)
        {
            return Query<PaymentEntity>("SELECT * FROM ProviderAdjustments.Payments WHERE Ukprn = @ukprn", new { ukprn });
        }

        private static void Execute(string command, object param = null, bool inTransient = true)
        {
            var connectionString = inTransient
                ? GlobalTestContext.Instance.TransientConnectionString
                : GlobalTestContext.Instance.DedsConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    connection.Execute(command, param);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private static T[] Query<T>(string command, object param = null)
        {
            using (var connection = new SqlConnection(GlobalTestContext.Instance.TransientConnectionString))
            {
                connection.Open();
                try
                {
                    return connection.Query<T>(command, param)?.ToArray();
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private static string ReplaceSqlTokens(string sql)
        {
            return sql.Replace("${EAS_Deds.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                      .Replace("${ILR_Summarisation.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                      .Replace("${DAS_PeriodEnd.FQ}", GlobalTestContext.Instance.BracketedDatabaseName)
                      .Replace("${YearOfCollection}", "1617");
        }
    }
}