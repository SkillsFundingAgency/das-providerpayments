namespace ProviderPayments.TestStack.Core.Workflow.Summarisation.Tasks
{
    internal class CoInvestedPaymentsTask : RunExternalTask
    {
        private const string AssemblyName = "SFA.DAS.Payments.Calc.CoInvestedPayments";
        private const string TypeName = "SFA.DAS.Payments.Calc.CoInvestedPayments.CoInvestedPaymentsTask";

        public CoInvestedPaymentsTask(ILogger logger)
            : base(ComponentType.CoInvestedPayments, AssemblyName, TypeName, logger)
        {
            Id = "CoInvestedPaymentsCalculator";
            Description = "Determine the co-invested payments for the period.";
        }
    }
}