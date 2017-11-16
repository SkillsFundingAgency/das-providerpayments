using ProviderPayments.TestStack.Core.Workflow.Common;
using ProviderPayments.TestStack.Core.Workflow.Summarisation.Tasks;

namespace ProviderPayments.TestStack.Core.Workflow.Summarisation
{
    internal class PrepareForEasWorkflow : Workflow
    {
        public PrepareForEasWorkflow(ILogger logger)
            : base(logger)
        {
            SetTasks(new WorkflowTask[]
            {
                new CleanupPeriodEndDedsTask(logger), 
                new SetCollectionPeriodTask(logger),
            });
        }
    }
}
