using System;
using System.Collections.Generic;

namespace SFA.DAS.Payments.Automation.IlrBuilder
{
    public class IndividualLearningRecord
    {
        public long Ukprn { get; set; }
        public DateTime PreparationDate { get; set; }
        public string AcademicYear { get; set; }

        public List<Learner> Learners { get; set; } = new List<Learner>();
    }
}
