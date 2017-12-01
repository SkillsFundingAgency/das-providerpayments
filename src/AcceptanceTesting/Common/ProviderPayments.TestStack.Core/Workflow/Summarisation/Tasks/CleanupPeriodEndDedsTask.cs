using ProviderPayments.TestStack.Core.Context;

namespace ProviderPayments.TestStack.Core.Workflow.Summarisation.Tasks
{
    internal class CleanupPeriodEndDedsTask : RunTransientSqlScriptsTask
    {
        private static readonly ComponentType[] ComponentTypes =
        {
            ComponentType.DataLockPeriodEnd,
            ComponentType.PaymentsDue,
            ComponentType.LevyCalculator,
            ComponentType.CoInvestedPayments,
            ComponentType.ManualAdjustments,
        };

        private static readonly string CleanupRegex = @".*\.Cleanup\..*\.sql$";

        public CleanupPeriodEndDedsTask(ILogger logger)
            : base(ComponentTypes, CleanupRegex, false, logger)
        {
            Id = "CleanupDedsPeriodEnd";
            Description = "Cleanup deds for period end";
        }
    }
}