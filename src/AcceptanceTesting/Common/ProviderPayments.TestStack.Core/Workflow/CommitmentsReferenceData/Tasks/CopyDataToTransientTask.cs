namespace ProviderPayments.TestStack.Core.Workflow.CommitmentsReferenceData.Tasks
{
    internal class CopyDataToTransientTask : CopyDataTask
    {
        private static readonly ComponentType[] ComponentTypes = { ComponentType.ReferenceCommitments };

        public CopyDataToTransientTask(ILogger logger)
            : base(ComponentTypes, logger, DataCopyDirection.DedsToTransient)
        {
            Id = "CopyCommitmentDataToTransient";
            Description = "Copy data to transient";
        }
    }
}
