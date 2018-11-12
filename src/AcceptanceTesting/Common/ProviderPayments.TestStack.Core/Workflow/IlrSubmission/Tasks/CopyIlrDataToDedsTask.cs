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
            string scripts = null;
            switch (context.OpaRulebaseYear)
            {
                case "1617":
                    scripts = Properties.Resources.CopyIlrDataToDeds_1617;
                    break;
                case "1718":
                    scripts = Properties.Resources.CopyIlrDataToDeds_1718;
                    break;
                case "1819":
                    scripts = Properties.Resources.CopyIlrDataToDeds_1819;
                    break;
            }

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
