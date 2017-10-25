namespace ProviderPayments.TestStack.Core.Workflow.AccountsReferenceData.Tasks
{
    internal class CopyDataToDedsTask : CopyDataTask
    {
       
        private static readonly ComponentType[] ComponentTypes = { ComponentType.ReferenceAccounts};


        public CopyDataToDedsTask(ILogger logger)
            : base(ComponentTypes,logger,DataCopyDirection.TransientToDeds)
        {
            Id = "CopyAccountsToDeds";
            Description = "Copy data to deds";
        }
    }
}
