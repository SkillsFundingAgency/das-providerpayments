namespace ProviderPayments.TestStack.Core.Workflow.Summarisation.Tasks
{
    internal class DataLockPeriodEndTask : RunExternalTask
    {
        private const string AssemblyName = "SFA.DAS.CollectionEarnings.DataLock";
        private const string TypeName = "SFA.DAS.CollectionEarnings.DataLock.DataLockTask";

        public DataLockPeriodEndTask(ILogger logger)
            : base(ComponentType.DataLockPeriodEnd, AssemblyName, TypeName, logger)
        {
            Id = "DataLock";
            Description = "DAS Data Lock at Ilr Period End";
        }
    }
}