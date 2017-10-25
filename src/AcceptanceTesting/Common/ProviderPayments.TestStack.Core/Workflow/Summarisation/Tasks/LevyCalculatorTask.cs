namespace ProviderPayments.TestStack.Core.Workflow.Summarisation.Tasks
{
    internal class LevyCalculatorTask : RunExternalTask
    {
        private const string AssemblyName = "SFA.DAS.ProviderPayments.Calc.LevyPayments";
        private const string TypeName = "SFA.DAS.ProviderPayments.Calc.LevyPayments.LevyPaymentsTask";

        public LevyCalculatorTask(ILogger logger)
            : base(ComponentType.LevyCalculator, AssemblyName, TypeName, logger)
        {
            Id = "LevyCalculator";
            Description = "Calculate levy payments";
        }
    }
}
