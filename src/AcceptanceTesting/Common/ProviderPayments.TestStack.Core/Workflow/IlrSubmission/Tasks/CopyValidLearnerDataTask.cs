namespace ProviderPayments.TestStack.Core.Workflow.IlrSubmission.Tasks
{
    internal class CopyValidLearnerDataTask : RunTransientSqlScriptsTask
    {
        private static readonly ComponentType[] ComponentTypes = { ComponentType.EarningsCalculator};
        private static readonly string PopulateRegex = @"(?i)^([0-9]{2})\s.*Transform.*\.sql$";

        internal CopyValidLearnerDataTask(ILogger logger)
             : base(ComponentTypes, PopulateRegex, true, logger)
        {
            Id = "CopyValidLearnerData";
            Description = "Copy valid learner data";
        }
    }
}
