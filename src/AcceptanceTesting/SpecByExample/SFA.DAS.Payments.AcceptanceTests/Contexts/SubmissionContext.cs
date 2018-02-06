﻿using System.Collections.Generic;
using SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels;

namespace SFA.DAS.Payments.AcceptanceTests.Contexts
{
    public class SubmissionContext
    {
        public SubmissionContext()
        {
            Submissions = new List<Submission>();
            HistoricalLearningDetails = new List<IlrLearnerReferenceData>();
        }

        public List<Submission> Submissions { get; set; }
        public List<IlrLearnerReferenceData> HistoricalLearningDetails { get; set; }

        public void Add(Submission submission)
        {
            Submissions.Add(submission);
        }
    }
}