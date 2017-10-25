using System.Linq;
using ProviderPayments.TestStack.Core.Workflow.Common;

namespace ProviderPayments.TestStack.Core.Workflow.Summarisation.Tasks
{
    internal class DataLockEventsPeriodEndTask : DataLockEventsTaskBase
    {
        public DataLockEventsPeriodEndTask(ILogger logger)
            : base(logger)
        {
            Id = "DataLockEventsPeriodEnd";
            Description = "DAS Data Lock Events at period end";
        }

        protected override string[] FilterOrderedSqlFiles(string[] orderedSqlFiles)
        {
            return orderedSqlFiles.Where(x => !x.ToLower().EndsWith(".submission.sql")).ToArray();
        }
    }
}
