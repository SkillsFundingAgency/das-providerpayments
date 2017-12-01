using ProviderPayments.TestStack.Core.Context;
using ProviderPayments.TestStack.Core.ExecutionStatus;
using ProviderPayments.TestStack.Core.Workflow.IlrSubmission.Tasks;
using ProviderPayments.TestStack.Core.Workflow.Summarisation.Tasks;

namespace ProviderPayments.TestStack.Core.Workflow.RebuildTransient
{
    public class RebuildTransientWorkflow : Workflow
    {
        public RebuildTransientWorkflow(ILogger logger) : base(logger)
        {
            SetTasks(new WorkflowTask[]
            {
                new DataLockPeriodEndTask(logger),
                new DataLockEventsPeriodEndTask(logger),
                new ManualAdjustmentsTask(logger),
                new PaymentsDueTask(logger),
                new LevyCalculatorTask(logger),
                new CoInvestedPaymentsTask(logger),
                new ProviderAdjustmentsTask(logger),
                new ShredIlrTask(logger),
                new CalculateEarningsTask(logger),
                new DataLockSubmissionTask(logger),
                new DataLockEventsSubmissionTask(logger),
                new SubmissionEventsTask(logger),
            });
        }

        internal override void Execute(TestStackContext context, StatusWatcherBase statusWatcherBase)
        {
            foreach (var workflowTask in _tasks)
            {
                workflowTask.Prepare(context);
            }
        }
    }
}
