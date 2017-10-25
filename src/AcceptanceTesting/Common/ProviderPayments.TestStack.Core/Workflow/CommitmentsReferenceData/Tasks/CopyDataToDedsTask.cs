using System.IO;
using System.Text.RegularExpressions;
using ProviderPayments.TestStack.Core.Context;

namespace ProviderPayments.TestStack.Core.Workflow.CommitmentsReferenceData.Tasks
{
    internal class CopyDataToDedsTask : CopyDataTask
    {
        private static readonly ComponentType[] ComponentTypes = { ComponentType.ReferenceCommitments };

        public CopyDataToDedsTask(ILogger logger)
            : base(ComponentTypes, logger, DataCopyDirection.TransientToDeds)
        {
            Id = "CopyCommitmentDataToDeds";
            Description = "Copy data to deds";
        }

        internal override void Execute(TestStackContext context)
        {
            ClearDeds(context);
            base.Execute(context);
        }


        private void ClearDeds(TestStackContext context)
        {
            var componentDirectory = GetComponentWorkingDirectory(ComponentType.ReferenceCommitments, context);

            var sqlDirectory = Path.Combine(componentDirectory, "sql", "dml");
            if (!Directory.Exists(sqlDirectory))
            {
                sqlDirectory = Path.Combine(componentDirectory, context.OpaRulebaseYear, "sql", "dml");
            }
            if (!Directory.Exists(sqlDirectory))
            {
                return;
            }

            var cleanScriptPath = Path.Combine(sqlDirectory, "dml.commitments.cleanup.deds.sql");
            if (!File.Exists(cleanScriptPath))
            {
                return;
            }

            using (var transientConnection = GetOpenTransientConnection(context))
            using (var dedsConnection = GetOpenDedsConnection(context))
            {
                try
                {
                    var sql = File.ReadAllText(cleanScriptPath);
                    ExecuteSqlScript(sql, transientConnection, context);
                }
                finally
                {
                    dedsConnection.Close();
                    transientConnection.Close();
                }
            }
        }
    }
}
