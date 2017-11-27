using ProviderPayments.TestStack.Core.Context;

namespace ProviderPayments.TestStack.Core.Workflow.Summarisation.Tasks
{
    class CleanupEasSubmissionDeds : RunSqlScriptsTask
    {
        public CleanupEasSubmissionDeds(ILogger logger) : 
            base(new string[0], new string[0], logger)
        {
        }

        internal override void Execute(TestStackContext context)
        {
            using (var connection = GetOpenDedsConnection(context))
            {
                try
                {
                    ExecuteSqlScript(Properties.Resources.EAS_PeriodEnd_Cleanup_Deds_DML, connection, context);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
