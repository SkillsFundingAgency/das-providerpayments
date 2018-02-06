using SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels;

namespace SFA.DAS.Payments.AcceptanceTests.ExecutionManagers
{
    public class ProviderSubmissionDetails
    {
        public string ProviderId { get; set; }
        public IlrLearnerReferenceData[] LearnerDetails { get; set; }
        public long Ukprn { get; set; }
    }
}
