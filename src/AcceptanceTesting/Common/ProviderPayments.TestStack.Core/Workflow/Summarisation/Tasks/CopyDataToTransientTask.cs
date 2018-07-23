namespace ProviderPayments.TestStack.Core.Workflow.Summarisation.Tasks
{
    internal class CopyDataToTransientTask : CopyDataTask
    {
        private static readonly ComponentType[] ComponentTypes =
        {
            ComponentType.PaymentsDue,
            ComponentType.Refunds
        };

        public CopyDataToTransientTask(ILogger logger)
            : base(ComponentTypes, logger, DataCopyDirection.DedsToTransient)
        {
            Id = "CopyEarningsDataToTransient";
            Description = "Copy data to transient";
        }
    }
}
