namespace ProviderPayments.TestStack.Core.Workflow.AccountsReferenceData.Tasks
{
    internal class AccountsReferenceDataTask : RunExternalTask
    {
        private const string AssemblyName = "SFA.DAS.Payments.Reference.Accounts";
        private const string TypeName = "SFA.DAS.Payments.Reference.Accounts.ImportAccountsTask";

        public AccountsReferenceDataTask(ILogger logger)
            : base(ComponentType.ReferenceAccounts, AssemblyName, TypeName, logger)
        {
            Id = "ReferenceAccounts";
            Description = "Populate the Accounts reference data.";
        }
    }
}