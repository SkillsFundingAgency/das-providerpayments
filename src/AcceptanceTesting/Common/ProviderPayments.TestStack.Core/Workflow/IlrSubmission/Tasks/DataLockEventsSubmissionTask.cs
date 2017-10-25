using System.Linq;
using ProviderPayments.TestStack.Core.Context;
using ProviderPayments.TestStack.Core.Workflow.Common;

namespace ProviderPayments.TestStack.Core.Workflow.IlrSubmission.Tasks
{
    internal class DataLockEventsSubmissionTask : DataLockEventsTaskBase
    {
        public DataLockEventsSubmissionTask(ILogger logger)
            : base(logger)
        {
            Id = "DataLockEventsSubmission";
            Description = "DAS Data Lock Events at Ilr Submission";
        }

        protected override string[] FilterOrderedSqlFiles(string[] orderedSqlFiles)
        {
            return orderedSqlFiles.Where(x => !x.ToLower().EndsWith(".periodend.sql")).ToArray();
        }
    }
}
