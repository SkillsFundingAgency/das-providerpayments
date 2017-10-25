using ProviderPayments.TestStack.Core.Workflow.AccountsReferenceData.Tasks;

namespace ProviderPayments.TestStack.Core.Workflow.AccountsReferenceData
{
    internal class AccountsReferenceDataWorkflow : Workflow
    {
        public AccountsReferenceDataWorkflow(ILogger logger)
            : base(logger)
        {
            SetTasks(new WorkflowTask[]
            {
                new CopyDataToTransientTask(logger),
                new AccountsReferenceDataTask(logger),
                new CleanupAccountsTask(logger), 
                new CopyDataToDedsTask(logger)
            });
        }
    }
}