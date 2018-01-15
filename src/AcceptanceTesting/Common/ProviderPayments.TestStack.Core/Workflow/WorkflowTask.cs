using System;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using Dapper;
using ProviderPayments.TestStack.Core.Context;

namespace ProviderPayments.TestStack.Core.Workflow
{
    internal abstract class WorkflowTask
    {
        internal string Id { get; set; }
        internal string Description { get; set; }

        internal virtual void Prepare(TestStackContext context)
        {
        }
        internal virtual void PreRun(TestStackContext context)
        {
        }
        internal abstract void Execute(TestStackContext context);


        protected SqlConnection GetOpenTransientConnection(TestStackContext context)
        {
            var connectionString = context.Properties[KnownContextKeys.TransientDatabaseConnectionString];
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }
        protected SqlConnection GetOpenDedsConnection(TestStackContext context)
        {
            var connectionString = context.Properties[KnownContextKeys.DedsDatabaseConnectionString];
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        protected void ExecuteSqlScript(string sql, SqlConnection connection, TestStackContext context)
        {
            var dedsConnectionString = context.Properties[KnownContextKeys.DedsDatabaseConnectionString];
            var dedsDatabaseName = ExtractDatabaseName(dedsConnectionString);

            sql = ReplaceSqlTokens(sql, dedsDatabaseName, context);

            var commands = Regex.Split(sql, @"GO\s*(\n|$|\r\n)", RegexOptions.IgnoreCase);

            foreach (var command in commands)
            {
                if (string.IsNullOrWhiteSpace(command))
                {
                    continue;
                }
                try
                {
                    connection.Execute(command);
                }
                catch (SqlException e)
                {
                    throw new Exception($"Error running SQL command: \n {command}\n\n\n", e);
                }
            }
        }
        protected string ReplaceSqlTokens(string sql, string dedsDatabaseName, TestStackContext context)
        {
            var transformedSql =  sql.Replace("${ILR_Current.FQ}", dedsDatabaseName)
                                     .Replace("${ILR_Previous.FQ}", dedsDatabaseName)
                                     .Replace("${DAS_Accounts.FQ}", dedsDatabaseName)
                                     .Replace("${DAS_Commitments.FQ}", dedsDatabaseName)
                                     .Replace("${ILR_Deds.FQ}", dedsDatabaseName)
                                     .Replace("${ILR_Summarisation.FQ}", dedsDatabaseName)
                                     .Replace("${DAS_PeriodEnd.FQ}", dedsDatabaseName)
                                     .Replace("${DAS_ProviderEvents.FQ}", dedsDatabaseName)
                                     .Replace("${YearOfCollection}", context.CurrentYear);

            var matches = Regex.Matches(transformedSql, @"\$\{.*\}");
            if (matches.Count > 0)
            {
                var missingTokens = new StringBuilder();
                foreach (Match match in matches)
                {
                    if (missingTokens.Length > 0)
                    {
                        missingTokens.Append(", ");
                    }
                    missingTokens.Append(match.Value);
                }
                throw new Exception($"Missing token translation for {missingTokens}");
            }

            return transformedSql;
        }
        protected string ExtractDatabaseName(string connectionString)
        {
            var match = Regex.Match(connectionString, @"database=([A-Z0-9\-_]{1,});", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            match = Regex.Match(connectionString, @"initial catalog=([A-Z0-9\-_]{1,});", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            throw new Exception("Cannot extract database name from connection");
        }
    }
}
