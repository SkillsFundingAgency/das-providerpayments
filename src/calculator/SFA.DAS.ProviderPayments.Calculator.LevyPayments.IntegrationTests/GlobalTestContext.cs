using System;
using System.Configuration;

namespace SFA.DAS.ProviderPayments.Calculator.LevyPayments.IntegrationTests
{
    internal class GlobalTestContext
    {
        private const string ConnectionStringKey = "TransientConnectionString";

        private GlobalTestContext()
        {
            try
            {
                SetupConnectionString();
                SetupAsseblyDirectory();
            }
            catch (Exception ex)
            {
                throw new GlobalTestContextSetupException(ex);
            }
        }

        public string ConnectionString { get; private set; }
        public string AssemblyDirectory { get; private set; }



        private void SetupConnectionString()
        {
            ConnectionString = Environment.GetEnvironmentVariable(ConnectionStringKey);
            if (string.IsNullOrEmpty(ConnectionString))
            {
                ConnectionString = ConfigurationManager.AppSettings[ConnectionStringKey];
            }
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
