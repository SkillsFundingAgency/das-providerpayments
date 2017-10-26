using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Dapper;
using SFA.DAS.CollectionEarnings.Calculator.Infrastructure.Data.Entities;

namespace SFA.DAS.CollectionEarnings.Calculator.IntegrationTests.Tools
{
    internal static class TestDataHelper
    {
        private readonly static Random _random = new Random();

        internal static void AddAccount(string id, string name = null, decimal balance = 999999999)
        {
            if (name == null)
            {
                name = id;
            }

            Execute("INSERT INTO dbo.DasAccounts (AccountId, AccountName, LevyBalance) VALUES (@id, @name, @balance)", new { id, name, balance });
        }
        
        internal static void Clean()
        {
            Execute(@"
                    DECLARE @SQL NVARCHAR(MAX) = ''

                    SELECT @SQL = (
                        SELECT 'TRUNCATE TABLE [' + s.name + '].[' + o.name + ']' + CHAR(13)
                        FROM sys.objects o WITH (NOWAIT)
                        JOIN sys.schemas s WITH (NOWAIT) ON o.[schema_id] = s.[schema_id]
                        WHERE o.[type] = 'U'
                            AND s.name IN ('Input', 'Valid', 'Invalid', 'Earnings', 'Rulebase')
                        FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)')

                    EXEC sys.sp_executesql @SQL                
                ");
        }

        internal static ApprenticeshipPriceEpisodeEntity[] GetApprenticeshipPriceEpisodes()
        {
            return Query<ApprenticeshipPriceEpisodeEntity>("SELECT * FROM [Rulebase].[AEC_ApprenticeshipPriceEpisode]");
        }

        internal static ApprenticeshipPriceEpisodePeriodisedValuesEntity[] GetApprenticeshipPriceEpisodePeriodisedValues()
        {
            return Query<ApprenticeshipPriceEpisodePeriodisedValuesEntity>("SELECT * FROM [Rulebase].[AEC_ApprenticeshipPriceEpisode_PeriodisedValues]");
        }

        internal static ApprenticeshipPriceEpisodePeriodEntity[] GetApprenticeshipPriceEpisodePeriods()
        {
            return Query<ApprenticeshipPriceEpisodePeriodEntity>("SELECT * FROM [Rulebase].[AEC_ApprenticeshipPriceEpisode_Period]");
        }

        internal static void ExecuteScript(string script)
        {
            var scriptCommand = File.ReadAllText($@"{AppDomain.CurrentDomain.BaseDirectory}\Tools\Sql\{script}");

            Execute(scriptCommand);
        }

        private static int Count(string tablename)
        {
            using (var connection = new SqlConnection(GlobalTestContext.Instance.ConnectionString))
            {
                connection.Open();
                try
                {
                    return connection.ExecuteScalar<int>($"SELECT count(*) FROM {tablename}");
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private static void Execute(string command, object param = null)
        {
            using (var connection = new SqlConnection(GlobalTestContext.Instance.ConnectionString))
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
            using (var connection = new SqlConnection(GlobalTestContext.Instance.ConnectionString))
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
    }
}