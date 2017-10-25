namespace ProviderPayments.TestStack.Core.Workflow.IlrSubmission.Tasks
{
    internal class CalculateEarningsTask : RunExternalTask
    {
        private const string AssemblyName = "SFA.DAS.CollectionEarnings.Calculator";
        private const string TypeName = "SFA.DAS.CollectionEarnings.Calculator.ApprenticeshipEarningsTask";

        public CalculateEarningsTask(ILogger logger)
            : base(ComponentType.EarningsCalculator, AssemblyName, TypeName, logger)
        {
            Id = "CalculateEarnings";
            Description = "Calculate earnings";
        }
    }
}
