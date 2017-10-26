using System;
using System.Configuration;
using System.Text.RegularExpressions;

namespace SFA.DAS.CollectionEarnings.DataLock.IntegrationTests
{
    internal class GlobalTestContext
    {
        private const string SubmissionConnectionStringKey = "SubmissionConnectionString";
        private const string SubmissionDedsConnectionStringKey = "SubmissionDedsConnectionString";
        private const string PeriodEndConnectionStringKey = "PeriodEndConnectionString";
        private const string PeriodEndDedsConnectionStringKey = "PeriodEndDedsConnectionString";

        private GlobalTestContext()
        {
            try
            {
                SetupConnectionStrings();

                SubmissionDatabaseName = ExtractDatabaseNameFromConnectionString(SubmissionConnectionString);
                SubmissionDedsDatabaseName = ExtractDatabaseNameFromConnectionString(SubmissionDedsConnectionString);
                PeriodEndDatabaseName = ExtractDatabaseNameFromConnectionString(PeriodEndConnectionString);
                PeriodEndDedsDatabaseName = ExtractDatabaseNameFromConnectionString(PeriodEndDedsConnectionString);

                SetupBracketedDatabaseNames();
                SetupAsseblyDirectory();
            }
            catch (Exception ex)
            {
                throw new GlobalTestContextSetupException(ex);
            }
        }

        public string SubmissionConnectionString { get; private set; }
        public string SubmissionDedsConnectionString { get; private set; }
        public string PeriodEndConnectionString { get; private set; }
        public string PeriodEndDedsConnectionString { get; private set; }
        public string SubmissionDatabaseName { get; private set; }
        public string SubmissionDedsDatabaseName { get; private set; }
        public string PeriodEndDatabaseName { get; private set; }
        public string PeriodEndDedsDatabaseName { get; private set; }
        public string BracketedSubmissionDatabaseName { get; private set; }
        public string BracketedSubmissionDedsDatabaseName { get; private set; }
        public string BracketedPeriodEndDatabaseName { get; private set; }
        public string BracketedPeriodEndDedsDatabaseName { get; private set; }
        public string AssemblyDirectory { get; private set; }

        private void SetupConnectionStrings()
        {
            SubmissionConnectionString = Environment.GetEnvironmentVariable(SubmissionConnectionStringKey);
            if (string.IsNullOrEmpty(SubmissionConnectionString))
            {
                SubmissionConnectionString = ConfigurationManager.AppSettings[SubmissionConnectionStringKey];
            }

            PeriodEndConnectionString = Environment.GetEnvironmentVariable(PeriodEndConnectionStringKey);
            if (string.IsNullOrEmpty(PeriodEndConnectionString))
            {
                PeriodEndConnectionString = ConfigurationManager.AppSettings[PeriodEndConnectionStringKey];
            }

            SubmissionDedsConnectionString = Environment.GetEnvironmentVariable(SubmissionDedsConnectionStringKey);
            if (string.IsNullOrEmpty(SubmissionDedsConnectionString))
            {
                SubmissionDedsConnectionString = ConfigurationManager.AppSettings[SubmissionDedsConnectionStringKey];
            }

            PeriodEndDedsConnectionString = Environment.GetEnvironmentVariable(PeriodEndDedsConnectionStringKey);
            if (string.IsNullOrEmpty(PeriodEndDedsConnectionString))
            {
                PeriodEndDedsConnectionString = ConfigurationManager.AppSettings[PeriodEndDedsConnectionStringKey];
            }
        }

        private string ExtractDatabaseNameFromConnectionString(string connectionString)
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

            throw new Exception("Cannot extract database name from connection string");
        }

        private void SetupPeriodEndDedsDatabaseName()
        {
            var match = Regex.Match(PeriodEndDedsConnectionString, @"database=([A-Z0-9\-_]{1,});", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                PeriodEndDedsDatabaseName = match.Groups[1].Value;
                return;
            }

            match = Regex.Match(PeriodEndDedsConnectionString, @"initial catalog=([A-Z0-9\-_]{1,});", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                PeriodEndDedsDatabaseName = match.Groups[1].Value;
                return;
            }

            throw new Exception("Cannot extract period end deds database name from connection");
        }

        private void SetupBracketedDatabaseNames()
        {
            BracketedSubmissionDatabaseName = $"[{SubmissionDatabaseName}]";
            BracketedPeriodEndDatabaseName = $"[{PeriodEndDatabaseName}]";
            BracketedSubmissionDedsDatabaseName = $"[{SubmissionDedsDatabaseName}]";
            BracketedPeriodEndDedsDatabaseName = $"[{PeriodEndDedsDatabaseName}]";
        }

        private void SetupAsseblyDirectory()
        {
            AssemblyDirectory = System.IO.Path.GetDirectoryName(typeof(GlobalTestContext).Assembly.Location);
        }

        private static GlobalTestContext _instance;

        public static GlobalTestContext Instance
        {
            get { return _instance ?? (_instance = new GlobalTestContext()); }
        }
    }
}