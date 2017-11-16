namespace ProviderPayments.TestStack.Core.Workflow.Summarisation.Tasks
{
    internal class CopyPeriodEndReferenceDataTask : RunTransientSqlScriptsTask
    {
        private static readonly ComponentType[] ComponentTypes = {
            ComponentType.DataLockPeriodEnd,
            ComponentType.ManualAdjustments,
            ComponentType.PaymentsDue,
            ComponentType.LevyCalculator,
            ComponentType.CoInvestedPayments ,
            ComponentType.DataLockEvents,
            ComponentType.ProviderAdjustments,
        };
        private static readonly string PopulateRegex = @"(?i)^([0-9]{2})\s.*Populate.*\.sql$";

        public CopyPeriodEndReferenceDataTask(ILogger logger)
            : base(ComponentTypes, PopulateRegex, true, logger)
        {
            Id = "CopyPeriodEndReferenceData";
            Description = "Copy DAS Period End reference data to transient";
        }
    }
}
