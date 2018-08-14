namespace ProviderPayments.TestStack.Core.Workflow.IlrSubmission.Tasks
{
    internal class CopyDataToTransientTask : CopyDataTask
    {
        private static readonly ComponentType[] ComponentTypes =
        {
            ComponentType.PaymentsDue,
        };

        public CopyDataToTransientTask(ILogger logger)
            : base(ComponentTypes, logger, DataCopyDirection.DedsToTransient)
        {
            Id = "CopyEarningsDataToTransient";
            Description = "Copy data to transient";
        }
    }
}
