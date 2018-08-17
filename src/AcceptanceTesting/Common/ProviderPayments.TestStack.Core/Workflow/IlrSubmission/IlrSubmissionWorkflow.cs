using ProviderPayments.TestStack.Core.Workflow.Common;
using ProviderPayments.TestStack.Core.Workflow.IlrSubmission.Tasks;

namespace ProviderPayments.TestStack.Core.Workflow.IlrSubmission
{
    internal class IlrSubmissionWorkflow : Workflow
    {
        public IlrSubmissionWorkflow(ILogger logger)
            : base(logger)
        {
            SetTasks(new WorkflowTask[]
            {
                new ExportIlrFileTask(logger),
                new ShredIlrTask(logger),
                new SetCollectionPeriodTask(logger), 
                new CopyValidLearnerDataTask(logger),
                new CopyIlrReferenceDataTask(logger),
                new CalculateEarningsTask(logger),
                new DataLockSubmissionTask(logger),
                new DataLockEventsSubmissionTask(logger),
                new SubmissionEventsTask(logger),
                new CleanuplrDedsTask(logger),
                new CopyIlrDataToDedsTask(logger),
                new CopyDasIlrDataToDedsTask(logger)
            });
        }
    }
}
