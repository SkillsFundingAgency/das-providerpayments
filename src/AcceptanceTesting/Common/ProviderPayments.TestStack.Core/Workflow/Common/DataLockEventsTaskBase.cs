namespace ProviderPayments.TestStack.Core.Workflow.Common
{
    internal abstract class DataLockEventsTaskBase : RunExternalTask
    {
        private const string AssemblyName = "SFA.DAS.Provider.Events.DataLock";
        private const string TypeName = "SFA.DAS.Provider.Events.DataLock.DataLockEventsTask";

        protected DataLockEventsTaskBase(ILogger logger)
            : base(ComponentType.DataLockEvents, AssemblyName, TypeName, logger)
        {
        }

        protected override string[] GetOrderedSqlFiles(string sqlDirectory)
        {
            var orderedSqlFiles = base.GetOrderedSqlFiles(sqlDirectory);
            return FilterOrderedSqlFiles(orderedSqlFiles);
        }

        protected abstract string[] FilterOrderedSqlFiles(string[] orderedSqlFiles);
    }
}
