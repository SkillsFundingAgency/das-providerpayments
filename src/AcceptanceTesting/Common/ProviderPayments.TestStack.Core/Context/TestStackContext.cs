using System.Collections.Generic;
using CS.Common.External.Interfaces;

namespace ProviderPayments.TestStack.Core.Context
{
    public class TestStackContext : IExternalContext
    {
        public TestStackContext()
        {
            Properties = new Dictionary<string, string>();
        }

        public string TransientConnectionString
        {
            get { return GetPropertyValueOrDefault(KnownContextKeys.TransientDatabaseConnectionString); }
            set { Properties[KnownContextKeys.TransientDatabaseConnectionString] = value; }
        }
        public string DedsDatabaseConnectionString
        {
            get { return GetPropertyValueOrDefault(KnownContextKeys.DedsDatabaseConnectionString); }
            set { Properties[KnownContextKeys.DedsDatabaseConnectionString] = value; }
        }
        public string LogLevel
        {
            get { return GetPropertyValueOrDefault(KnownContextKeys.LogLevel); }
            set { Properties[KnownContextKeys.LogLevel] = value; }
        }
        public string CurrentYear
        {
            get { return GetPropertyValueOrDefault(KnownContextKeys.YearOfCollection); }
            set { Properties[KnownContextKeys.YearOfCollection] = value; }
        }

        public string OpaRulebaseYear
        {
            get { return GetPropertyValueOrDefault(KnownContextKeys.OpaRulebaseYear); }
            set { Properties[KnownContextKeys.OpaRulebaseYear] = value; }
        }

        public string RequestContent
        {
            get { return GetPropertyValueOrDefault(TestStackContextKeys.RequestContent); }
            set { Properties[TestStackContextKeys.RequestContent] = value; }
        }
        public bool SubmissionIsIlrFile
        {
            get { return bool.Parse(GetPropertyValueOrDefault(TestStackContextKeys.SubmissionIsIlrFile, "false")); }
            set { Properties[TestStackContextKeys.SubmissionIsIlrFile] = value.ToString(); }
        }
        public string WorkingDirectory
        {
            get { return GetPropertyValueOrDefault(TestStackContextKeys.WorkingDirectory); }
            set { Properties[TestStackContextKeys.WorkingDirectory] = value; }
        }
        public string IlrFileDirectory
        {
            get { return GetPropertyValueOrDefault(TestStackContextKeys.IlrFileDirectory); }
            set { Properties[TestStackContextKeys.IlrFileDirectory] = value; }
        }
        public string IlrFilePath
        {
            get { return GetPropertyValueOrDefault(TestStackContextKeys.IlrFilePath); }
            set { Properties[TestStackContextKeys.IlrFilePath] = value; }
        }
        public string CollectionPeriod
        {
            get { return GetPropertyValueOrDefault(TestStackContextKeys.CollectionPeriod); }
            set { Properties[TestStackContextKeys.CollectionPeriod] = value; }
        }

        public string IlrAimRefLookups
        {
            get { return GetPropertyValueOrDefault(TestStackContextKeys.AimRefLookups); }
            set { Properties[TestStackContextKeys.AimRefLookups] = value; }
        }

        public string DataLockEventsSource
        {
            get { return GetPropertyValueOrDefault(KnownContextKeys.DataLockEventsSource); }
            set { Properties[KnownContextKeys.DataLockEventsSource] = value; }
        }

        public string CommitmentApiBaseUrl
        {
            get { return GetPropertyValueOrDefault(KnownContextKeys.CommitmentApiBaseUrl); }
            set { Properties[KnownContextKeys.CommitmentApiBaseUrl] = value; }
        }
        public string CommitmentApiClientToken
        {
            get { return GetPropertyValueOrDefault(KnownContextKeys.CommitmentApiClientToken); }
            set { Properties[KnownContextKeys.CommitmentApiClientToken] = value; }
        }

        public string AccountsApiBaseUrl
        {
            get { return GetPropertyValueOrDefault(KnownContextKeys.AccountsApiBaseUrl); }
            set { Properties[KnownContextKeys.AccountsApiBaseUrl] = value; }
        }
        public string AccountsApiClientToken
        {
            get { return GetPropertyValueOrDefault(KnownContextKeys.AccountsApiClientToken); }
            set { Properties[KnownContextKeys.AccountsApiClientToken] = value; }
        }
        public string AccountsApiClientId
        {
            get { return GetPropertyValueOrDefault(KnownContextKeys.AccountsApiClientId); }
            set { Properties[KnownContextKeys.AccountsApiClientId] = value; }
        }
        public string AccountsApiClientSecret
        {
            get { return GetPropertyValueOrDefault(KnownContextKeys.AccountsApiClientSecret); }
            set { Properties[KnownContextKeys.AccountsApiClientSecret] = value; }
        }
        public string AccountsApiIdentifierUri
        {
            get { return GetPropertyValueOrDefault(KnownContextKeys.AccountsApiIdentifierUri); }
            set { Properties[KnownContextKeys.AccountsApiIdentifierUri] = value; }
        }
        public string AccountsApiTenant
        {
            get { return GetPropertyValueOrDefault(KnownContextKeys.AccountsApiTenant); }
            set { Properties[KnownContextKeys.AccountsApiTenant] = value; }
        }

        public IDictionary<string, string> Properties { get; set; }

        private string GetPropertyValueOrDefault(string key, string defaultValue = "")
        {
            return !Properties.ContainsKey(key) ? defaultValue : Properties[key];
        }
    }
}
