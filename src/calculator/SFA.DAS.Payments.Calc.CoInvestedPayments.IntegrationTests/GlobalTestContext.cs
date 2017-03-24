using System;
using System.Configuration;
using System.Text.RegularExpressions;

namespace SFA.DAS.Payments.Calc.CoInvestedPayments.IntegrationTests
{
    internal class GlobalTestContext
    {
        private const string TransientConnectionStringKey = "TransientConnectionString";
        private const string DedsConnectionStringKey = "DedsConnectionString";


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

        public string TransientConnectionString { get; private set; }
        public string DatabaseName { get; private set; }
        public string BracketedDatabaseName { get; private set; }
        public string AssemblyDirectory { get; private set; }
        public string DedsConnectionString { get; private set; }



        private void SetupConnectionString()
        {
            TransientConnectionString = Environment.GetEnvironmentVariable(TransientConnectionStringKey);
            if (string.IsNullOrEmpty(TransientConnectionString))
            {
                TransientConnectionString = ConfigurationManager.AppSettings[TransientConnectionStringKey];
            }
            DedsConnectionString = Environment.GetEnvironmentVariable(DedsConnectionStringKey);
            if (string.IsNullOrEmpty(DedsConnectionString))
            {
                DedsConnectionString = ConfigurationManager.AppSettings[DedsConnectionStringKey];
            }
        }
        private void SetupDatabaseName()
        {
            var match = Regex.Match(TransientConnectionString, @"database=([A-Z0-9\-_]{1,});", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                DatabaseName = match.Groups[1].Value;
                return;
            }

            match = Regex.Match(TransientConnectionString, @"initial catalog=([A-Z0-9\-_]{1,});", RegexOptions.IgnoreCase);
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
