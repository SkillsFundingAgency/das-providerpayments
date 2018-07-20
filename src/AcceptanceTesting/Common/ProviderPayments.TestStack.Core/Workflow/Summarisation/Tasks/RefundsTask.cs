namespace ProviderPayments.TestStack.Core.Workflow.Summarisation.Tasks
{
    internal class RefundsTask : RunExternalTask
    {
        private const string AssemblyName = "SFA.DAS.ProviderPayments.Calc.Refunds";
        private const string TypeName = "SFA.DAS.ProviderPayments.Calc.Refunds.RefundsTask";

        public RefundsTask(ILogger logger)
            : base(ComponentType.Refunds, AssemblyName, TypeName, logger)
        {
            Id = "Refunds";
            Description = "Determine the refunds due for the period.";
        }
    }
}