﻿using System;
using System.Collections.Generic;
using SFA.DAS.Payments.AcceptanceTests.ReferenceDataModels;
using SFA.DAS.Payments.AcceptanceTests.ResultsDataModels;

namespace SFA.DAS.Payments.AcceptanceTests.Contexts
{
    public class Submission
    {
        public Submission()
        {
            HaveSubmissionsBeenDone = false;
            IlrLearnerDetails = new List<IlrLearnerReferenceData>();
            ContractTypes = new List<ContractTypeReferenceData>();
            EmploymentStatus = new List<EmploymentStatusReferenceData>();
            LearningSupportStatus = new List<LearningSupportReferenceData>();
        }

        public bool HaveSubmissionsBeenDone { get; set; }
        public List<IlrLearnerReferenceData> IlrLearnerDetails { get; set; }
        public List<LearnerResults> SubmissionResults { get; set; }
        public List<ContractTypeReferenceData> ContractTypes { get; set; }
        public List<EmploymentStatusReferenceData> EmploymentStatus { get; set; }
        public List<LearningSupportReferenceData> LearningSupportStatus { get; set; }
        public DateTime? FirstSubmissionDate { get; set; }
        public string SubmissionPeriod { get; set; }


    }

    public class MultipleSubmissionsContext
    {
        public MultipleSubmissionsContext()
        {
            Submissions = new List<Submission>();
            SubmissionResults = new List<LearnerResults>();
        }

        public List<Submission> Submissions { get; set; }
        public List<LearnerResults> SubmissionResults { get; set; }
        public List<IlrLearnerReferenceData> HistoricalLearningDetails { get; set; }
    }
}
