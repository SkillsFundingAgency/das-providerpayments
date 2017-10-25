namespace ProviderPayments.TestStack.Core.Workflow.AccountsReferenceData.Tasks
{
    internal class CopyDataToTransientTask : CopyDataTask
    {
        private static readonly ComponentType[] ComponentTypes = { ComponentType.ReferenceAccounts };


        public CopyDataToTransientTask(ILogger logger)
            : base(ComponentTypes, logger,DataCopyDirection.DedsToTransient)
        {
            Id = "CopyAccountsToTransient";
            Description = "Copy data to transient";
        }
    }
}
