using ProviderPayments.TestStack.Core.Domain;

namespace ProviderPayments.TestStack.Core
{
    public class EnvironmentVariables
    {
        public EnvironmentVariables()
        {
            WorkingDirectory = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(typeof(EnvironmentVariables).Assembly.Location), "temp");
            IlrFileDirectory = WorkingDirectory;
            CurrentYear = "1617";
            LogLevel = "DEBUG";
            OpaRulebaseYear = "1617";

            CollectionPeriod = new CollectionPeriod
            {
                PeriodId = 1,
                Period = "R01",
                CalendarMonth = 8,
                CalendarYear = 2016,
                CollectionOpen = 1,
                ActualsSchemaPeriod = "201608"
            };
        }

        public string TransientConnectionString { get; set; }
        public string DedsDatabaseConnectionString { get; set; }
        public string WorkingDirectory { get; set; }
        public string IlrFileDirectory { get; set; }
        public string CurrentYear { get; set; }
        public string LogLevel { get; set; }
        public CollectionPeriod CollectionPeriod { get; set; }

        public IlrAimRefLookup[] IlrAimRefLookups { get; set; }

        public string CommitmentApiBaseUrl { get; set; }
        public string CommitmentApiClientToken { get; set; }

        public string AccountsApiBaseUrl { get; set; }
        public string AccountsApiClientToken { get; set; }
        public string AccountsApiClientId { get; set; }
        public string AccountsApiClientSecret { get; set; }
        public string AccountsApiIdentifierUri { get; set; }
        public string AccountsApiTenant { get; set; }
        public string OpaRulebaseYear { get; set; }
    }
}
