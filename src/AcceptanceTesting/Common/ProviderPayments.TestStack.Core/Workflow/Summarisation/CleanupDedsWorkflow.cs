using ProviderPayments.TestStack.Core.Workflow.Summarisation.Tasks;

namespace ProviderPayments.TestStack.Core.Workflow.Summarisation
{
    class CleanupDedsWorkflow : Workflow
    {
        public CleanupDedsWorkflow(ILogger logger)
            : base(logger)
        {
            SetTasks(new WorkflowTask[]
            {
                new CleanupEasSubmissionDeds(logger), 
            });
        }
    }
}
