namespace ProviderPayments.TestStack.Core.Workflow.IlrSubmission.Tasks
{
    internal class CopyIlrReferenceDataTask : RunTransientSqlScriptsTask
    {
        private static readonly ComponentType[] ComponentTypes = { ComponentType.DataLockSubmission, ComponentType.EarningsCalculator, ComponentType.SubmissionEvents, ComponentType.DataLockEvents };
        private static readonly string PopulateRegex = @"(?i)^([0-9]{2})\s.*Populate.*\.sql$";

        public CopyIlrReferenceDataTask(ILogger logger)
            : base(ComponentTypes, PopulateRegex, true, logger)
        {
            Id = "CopyIlrReferenceData";
            Description = "Copy ILR reference data to transient";
        }
    }
}
