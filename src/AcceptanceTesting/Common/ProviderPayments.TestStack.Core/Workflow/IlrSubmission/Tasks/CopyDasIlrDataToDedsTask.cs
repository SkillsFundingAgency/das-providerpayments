namespace ProviderPayments.TestStack.Core.Workflow.IlrSubmission.Tasks
{
    internal class CopyDasIlrDataToDedsTask : CopyDataTask
    {
        private static readonly ComponentType[] ComponentTypes =
        {
            ComponentType.DataLockSubmission,
            ComponentType.EarningsCalculator,
            ComponentType.DataLockEvents,
            ComponentType.SubmissionEvents
        };

        public CopyDasIlrDataToDedsTask(ILogger logger)
            : base(ComponentTypes, logger)
        {
            Id = "CopyDasIlrDataToDeds";
            Description = "Copy DAS ILR data to DEDS";
        }
    }
}