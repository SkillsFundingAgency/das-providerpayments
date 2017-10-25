namespace ProviderPayments.TestStack.Core.Workflow.Summarisation.Tasks
{
    internal class ManualAdjustmentsTask : RunExternalTask
    {
        private const string AssemblyName = "SFA.DAS.ProviderPayments.Calc.ManualAdjustments";
        private const string TypeName = "SFA.DAS.ProviderPayments.Calc.ManualAdjustments.ManualAdjustmentsTask";

        public ManualAdjustmentsTask(ILogger logger)
            : base(ComponentType.ManualAdjustments, AssemblyName, TypeName, logger)
        {
            Id = "ManualAdjustments";
            Description = "Determine the payments that are due for the period.";
        }
    }
}