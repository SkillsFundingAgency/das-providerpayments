using ProviderPayments.TestStack.Core.Workflow.CommitmentsReferenceData.Tasks;

namespace ProviderPayments.TestStack.Core.Workflow.CommitmentsReferenceData
{
    internal class CommitmentsReferenceDataWorkflow : Workflow
    {
        public CommitmentsReferenceDataWorkflow(ILogger logger)
            : base(logger)
        {
            SetTasks(new WorkflowTask[]
            {
                new CopyDataToTransientTask(logger),
                new CommitmentsReferenceDataTask(logger),
                new CopyDataToDedsTask(logger)
            });
        }
    }
}