using System;
using System.Configuration;
using System.Text.RegularExpressions;

namespace SFA.DAS.CollectionEarnings.Calculator.IntegrationTests
{
    internal class GlobalTestContext
    {
        private const string ConnectionStringKey = "TransientConnectionString";

        private GlobalTestContext()
        {
            try
            {
                SetupConnectionString();
                SetupDatabaseName();
                SetupBracketedDatabaseName();
                SetupAsseblyDirectory();
            }
            catch (Exception ex)
            {
                throw new GlobalTestContextSetupException(ex);
            }
        }

        public string ConnectionString { get; private set; }
        public string DatabaseName { get; private set; }
        public string BracketedDatabaseName { get; private set; }
        public string AssemblyDirectory { get; private set; }



        private void SetupConnectionString()
        {
            ConnectionString = Environment.GetEnvironmentVariable(ConnectionStringKey);
            if (string.IsNullOrEmpty(ConnectionString))
            {
                ConnectionString = ConfigurationManager.AppSettings[ConnectionStringKey];
            }
        }
        private void SetupDatabaseName()
        {
            var match = Regex.Match(ConnectionString, @"database=([A-Z0-9\-_]{1,});", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                DatabaseName = match.Groups[1].Value;
                return;
            }

            match = Regex.Match(ConnectionString, @"initial catalog=([A-Z0-9\-_]{1,});", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                DatabaseName = match.Groups[1].Value;
                return;
            }

            throw new Exception("Cannot extract database name from connection");
        }
        private void SetupBracketedDatabaseName()
        {
            BracketedDatabaseName = $"[{DatabaseName}]";
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
