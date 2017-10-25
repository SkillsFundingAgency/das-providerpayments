namespace ProviderPayments.TestStack.Core.Workflow.CommitmentsReferenceData.Tasks
{
    internal class CommitmentsReferenceDataTask : RunExternalTask
    {
        private const string AssemblyName = "SFA.DAS.Payments.Reference.Commitments";
        private const string TypeName = "SFA.DAS.Payments.Reference.Commitments.ImportCommitmentsTask";

        public CommitmentsReferenceDataTask(ILogger logger)
            : base(ComponentType.ReferenceCommitments, AssemblyName, TypeName, logger)
        {
            Id = "ReferenceCommitments";
            Description = "Populate the Commitments reference data.";
        }
    }
}