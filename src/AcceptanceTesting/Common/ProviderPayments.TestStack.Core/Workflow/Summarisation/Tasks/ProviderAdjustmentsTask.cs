namespace ProviderPayments.TestStack.Core.Workflow.Summarisation.Tasks
{
    class ProviderAdjustmentsTask : RunExternalTask
    {
        private const string AssemblyName = "SFA.DAS.Payments.Calc.ProviderAdjustments";
        private const string TypeName = "SFA.DAS.Payments.Calc.ProviderAdjustments.ProviderAdjustmentsTask";

        public ProviderAdjustmentsTask(ILogger logger) 
            : base(ComponentType.ProviderAdjustments, AssemblyName, TypeName, logger)
        {
            Id = "ProviderAdjustments";
            Description = "ProcessProviderAdjustments";
        }
    }
}
