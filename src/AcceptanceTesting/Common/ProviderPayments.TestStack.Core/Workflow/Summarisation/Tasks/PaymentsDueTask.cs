namespace ProviderPayments.TestStack.Core.Workflow.Summarisation.Tasks
{
    internal class PaymentsDueTask : RunExternalTask
    {
        private const string AssemblyName = "SFA.DAS.ProviderPayments.Calc.PaymentsDue";
        private const string TypeName = "SFA.DAS.ProviderPayments.Calc.PaymentsDue.PaymentsDueTask";

        public PaymentsDueTask(ILogger logger)
            : base(ComponentType.PaymentsDue, AssemblyName, TypeName, logger)
        {
            Id = "PaymentsDue";
            Description = "Determine the payments that are due for the period.";
        }
    }
}