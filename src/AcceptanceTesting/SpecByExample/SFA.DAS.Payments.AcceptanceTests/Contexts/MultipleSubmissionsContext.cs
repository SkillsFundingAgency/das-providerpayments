using System.Collections.Generic;
using SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels;
using SFA.DAS.Payments.AcceptanceTests.ResultsDataModels;

namespace SFA.DAS.Payments.AcceptanceTests.Contexts
{
    public class MultipleSubmissionsContext
    {
        public MultipleSubmissionsContext()
        {
            Submissions = new List<Submission>();
            SubmissionResults = new List<LearnerResults>();
            HistoricalLearningDetails = new List<IlrLearnerReferenceData>();
        }

        public List<Submission> Submissions { get; set; }
        public List<LearnerResults> SubmissionResults { get; set; }
        public List<IlrLearnerReferenceData> HistoricalLearningDetails { get; set; }

        public void Add(Submission submission)
        {
            Submissions.Add(submission);
        }
    }
}