namespace ProviderPayments.TestStack.Core.Workflow.Summarisation.Tasks
{
    internal class PeriodEndScriptsTask : RunTransientSqlScriptsTask
    {
        private static readonly ComponentType[] ComponentTypes = { ComponentType.PeriodEndScripts };
        private static readonly string ScriptsRegex = @"^([0-9]{2})\s.*\.sql$";

        public PeriodEndScriptsTask(ILogger logger)
            : base(ComponentTypes, ScriptsRegex, true, logger)
        {
            Id = "PeriodEndScripts";
            Description = "Run period end scripts";
        }
    }
}