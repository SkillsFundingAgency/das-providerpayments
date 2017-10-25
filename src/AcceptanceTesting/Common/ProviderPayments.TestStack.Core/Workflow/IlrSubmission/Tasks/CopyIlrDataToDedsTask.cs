using ProviderPayments.TestStack.Core.Context;

namespace ProviderPayments.TestStack.Core.Workflow.IlrSubmission.Tasks
{
    internal class CopyIlrDataToDedsTask : RunSqlScriptsTask
    {
        private static readonly string[] TransientSqlScripts =
        {
            Properties.Resources.CopyIlrDataToDeds_1617
        };

        public CopyIlrDataToDedsTask(ILogger logger)
            : base(TransientSqlScripts, null, logger)
        {
            Id = "CopyIlrDataToDeds";
            Description = "Copy ILR data to DEDS";
        }

        internal override void Execute(TestStackContext context)
        {
            var scripts = context.OpaRulebaseYear == "1617" ? 
                    Properties.Resources.CopyIlrDataToDeds_1617 :
                    Properties.Resources.CopyIlrDataToDeds_1718 ;

            if (!string.IsNullOrEmpty(scripts))
            {
                using (var connection = GetOpenTransientConnection(context))
                {
                    ExecuteSqlScript(scripts, connection, context);
                }
            }
        }
    }
}
