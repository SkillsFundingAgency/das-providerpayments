namespace ProviderPayments.TestStack.Core.Workflow.AccountsReferenceData.Tasks
{
    internal class CleanupAccountsTask : RunTransientSqlScriptsTask
    {
        private static readonly ComponentType[] ComponentTypes = {ComponentType.ReferenceAccounts};
        private static readonly string CleanupRegex = @".*\.cleanup\..*\.sql$";

        public CleanupAccountsTask(ILogger logger)
            : base(ComponentTypes, CleanupRegex, false, logger)
        {
            Id = "CleanupAccountsTask";
            Description = "Cleanup deds for accounts";
        }
    }
}