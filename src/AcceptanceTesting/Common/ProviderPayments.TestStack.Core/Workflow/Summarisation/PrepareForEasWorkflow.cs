using ProviderPayments.TestStack.Core.Workflow.Common;

namespace ProviderPayments.TestStack.Core.Workflow.Summarisation
{
    internal class PrepareForEasWorkflow : Workflow
    {
        public PrepareForEasWorkflow(ILogger logger)
            : base(logger)
        {
            SetTasks(new WorkflowTask[]
            {
                new SetCollectionPeriodTask(logger),
            });
        }
    }
}
