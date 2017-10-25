namespace ProviderPayments.TestStack.Core.Workflow.IlrSubmission.Tasks
{
    internal class SubmissionEventsTask :  RunExternalTask
    {
        private const string AssemblyName = "SFA.DAS.Provider.Events.Submission";
        private const string TypeName = "SFA.DAS.Provider.Events.Submission.SubmissionEventsTask";

        public SubmissionEventsTask(ILogger logger)
            : base(ComponentType.SubmissionEvents, AssemblyName, TypeName, logger)
        {
            Id = "SubmissionEvents";
            Description = "Submission events";
        }
    }
}
