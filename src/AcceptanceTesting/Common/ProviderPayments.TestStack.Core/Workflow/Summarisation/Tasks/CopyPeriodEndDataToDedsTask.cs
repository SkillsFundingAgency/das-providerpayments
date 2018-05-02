namespace ProviderPayments.TestStack.Core.Workflow.Summarisation.Tasks
{
    internal class CopyPeriodEndDataToDedsTask : CopyDataTask
    {
        private static readonly ComponentType[] ComponentTypes = 
            {
                ComponentType.DataLockPeriodEnd,
                ComponentType.PaymentsDue,
                ComponentType.ManualAdjustments,
                ComponentType.TransferPayments,
                ComponentType.LevyCalculator,
                ComponentType.CoInvestedPayments,
                ComponentType.DataLockEvents,
                ComponentType.ProviderAdjustments,
        };

        public CopyPeriodEndDataToDedsTask(ILogger logger)
            : base(ComponentTypes, logger)
        {
            Id = "CopyPeriodEndDataToDeds";
            Description = "Copy period end data to DEDS";
        }
    }
}