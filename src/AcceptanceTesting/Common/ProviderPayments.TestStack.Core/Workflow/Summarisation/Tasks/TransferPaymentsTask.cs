namespace ProviderPayments.TestStack.Core.Workflow.Summarisation.Tasks
{
    internal class TransferPaymentsTask : RunExternalTask
    {
        private const string AssemblyName = "SFA.DAS.ProviderPayments.Calc.TransferPayments";
        private const string TypeName = "SFA.DAS.ProviderPayments.Calc.TransferPayments.TransfersTask";

        public TransferPaymentsTask(ILogger logger)
            : base(ComponentType.TransferPayments, AssemblyName, TypeName, logger)
        {
            Id = "TransferPayments";
            Description = "Determine the transfer payments that are due for the period.";
        }
    }
}