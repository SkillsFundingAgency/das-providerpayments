using ProviderPayments.TestStack.Core.Workflow.RebuildDedsDatabase.Tasks;

namespace ProviderPayments.TestStack.Core.Workflow.RebuildDedsDatabase
{
    internal class RebuildDedsDatabaseWorkflow : Workflow
    {
        public RebuildDedsDatabaseWorkflow(ILogger logger)
            : base(logger)
        {
            SetTasks(new WorkflowTask[]
            {
                new ExecuteDedsScriptsTask(logger),
                new ExecuteMigrationScriptsTask(logger)
            });
        }
    }
}
