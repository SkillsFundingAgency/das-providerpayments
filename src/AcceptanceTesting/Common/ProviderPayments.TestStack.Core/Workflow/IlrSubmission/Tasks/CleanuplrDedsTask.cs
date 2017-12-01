namespace ProviderPayments.TestStack.Core.Workflow.IlrSubmission.Tasks
{
    internal class CleanuplrDedsTask : RunTransientSqlScriptsTask
    {
        private static readonly ComponentType[] ComponentTypes = {
            ComponentType.DataLockSubmission,
            ComponentType.EarningsCalculator,
            ComponentType.SubmissionEvents,
            ComponentType.DataLockEvents 
        };

        private static readonly string CleanupRegex = @".*\.Cleanup\..*\.sql$";

        public CleanuplrDedsTask(ILogger logger)
            : base(ComponentTypes, CleanupRegex, false, logger)
        {
            Id = "CleanupDedsIlr";
            Description = "Cleanup deds for an ilr submission";
        }
    }
}