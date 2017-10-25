namespace ProviderPayments.TestStack.Core.Context
{
    internal static class KnownContextKeys
    {
        internal const string TransientDatabaseConnectionString = "TransientDatabaseConnectionString";
        internal const string DedsDatabaseConnectionString = "DedsDatabaseConnectionString";
        internal const string LogLevel = "LogLevel";
        internal const string YearOfCollection = "YearOfCollection";
        internal const string DataLockEventsSource = "DataLockEventsSource";

        internal const string CommitmentApiBaseUrl = "DAS.Payments.Commitments.BaseUrl";
        internal const string CommitmentApiClientToken = "DAS.Payments.Commitments.ClientToken";

        internal const string AccountsApiBaseUrl = "DAS.Payments.Accounts.BaseUrl";
        internal const string AccountsApiClientToken = "DAS.Payments.Accounts.ClientToken";
        public const string AccountsApiClientId = "DAS.Payments.Accounts.ClientId";
        public const string AccountsApiClientSecret = "DAS.Payments.Accounts.ClientSecret";
        public const string AccountsApiIdentifierUri = "DAS.Payments.Accounts.IdentifierUri";
        public const string AccountsApiTenant = "DAS.Payments.Accounts.Tenant";

        internal const string OpaRulebaseYear = "OpaRulebaseYear";

    }
}
