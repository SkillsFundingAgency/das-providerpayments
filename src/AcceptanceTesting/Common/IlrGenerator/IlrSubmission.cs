using System;

namespace IlrGenerator
{
    public class IlrSubmission
    {
        public IlrSubmission()
        {
            AcademicYear = "1617";
            PreperationDate = DateTime.Now;
        }
        public string AcademicYear { get; set; }
        public DateTime PreperationDate { get; set; }
        public long Ukprn { get; set; }
        public Learner[] Learners { get; set; }
    }
}