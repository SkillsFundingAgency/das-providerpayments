namespace ProviderPayments.TestStack.Core.Workflow.IlrSubmission.Tasks
{
    internal class DataLockSubmissionTask : RunExternalTask
    {
        private const string AssemblyName = "SFA.DAS.CollectionEarnings.DataLock";
        private const string TypeName = "SFA.DAS.CollectionEarnings.DataLock.DataLockTask";

        public DataLockSubmissionTask(ILogger logger)
            : base(ComponentType.DataLockSubmission, AssemblyName, TypeName, logger)
        {
            Id = "DataLock";
            Description = "DAS Data Lock at Ilr Submission";
        }
    }
}
