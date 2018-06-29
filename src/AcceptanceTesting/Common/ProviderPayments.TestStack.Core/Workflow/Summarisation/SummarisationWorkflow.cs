using ProviderPayments.TestStack.Core.Workflow.Common;
using ProviderPayments.TestStack.Core.Workflow.Summarisation.Tasks;

namespace ProviderPayments.TestStack.Core.Workflow.Summarisation
{
    internal class SummarisationWorkflow : Workflow
    {
        public SummarisationWorkflow(ILogger logger)
            : base(logger)
        {
            SetTasks(new WorkflowTask[]
            {
                new SetCollectionPeriodTask(logger),
                new CopyDataToTransientTask(logger),
                new CopyPeriodEndReferenceDataTask(logger),
                new DataLockPeriodEndTask(logger),
                new DataLockEventsPeriodEndTask(logger),
                new ManualAdjustmentsTask(logger),
                new PaymentsDueTask(logger),
                new RefundsTask(logger),
                new TransferPaymentsTask(logger),
                new LevyCalculatorTask(logger),
                new CoInvestedPaymentsTask(logger),
                new ProviderAdjustmentsTask(logger), 
                new CleanupPeriodEndDedsTask(logger),
                new CopyPeriodEndDataToDedsTask(logger),
                new PeriodEndScriptsTask(logger)
            });
        }
    }
}
